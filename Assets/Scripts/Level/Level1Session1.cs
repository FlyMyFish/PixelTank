using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    public class Level1Session1 : MonoBehaviour, ILevelSession
    {
        public List<GameObject> walls;
        public GameObject bornPointPrefabs;
        private readonly List<int> _wallIndex = new List<int>();
        private readonly List<Vector3> _wallPositions = new List<Vector3>();
        private LevelCreator _levelCreator;
        private const int MaxEnemy = 10;
        private int _leftEnemy = 10; //出生过的坦克数量
        private int _curEnemyCount; //当前活动中的坦克数量；
        private int _killedEnemyCount; //损毁的坦克数量
        public event Action OnSessionStart;
        public event Action OnSessionEnd;
        private readonly List<IBornPoint> _bornPoints = new List<IBornPoint>();
        private readonly List<Vector3> _bornPosition = new List<Vector3>();

        private readonly List<GameObject> _enemyTanks = new List<GameObject>();

        private void Awake()
        {
            _enemyTanks.Add(Resources.Load<GameObject>("Prefabs/EnemyGreenTank"));
            _enemyTanks.Add(Resources.Load<GameObject>("Prefabs/EnemyOrangeTank"));
            _enemyTanks.Add(Resources.Load<GameObject>("Prefabs/EnemyYellowTank"));
            for (var j = 4; j < 25; j++)
            {
                if (j % 4 == 0)
                {
                    for (var i = -10; i < 11; i++)
                    {
                        if (j == 24 && Math.Abs(i) == 10)
                        {
                            _wallIndex.Add(i == -10 ? 5 : 6);
                        }
                        else
                        {
                            _wallIndex.Add(Math.Abs(i) % 3);
                        }

                        _wallPositions.Add(new Vector3(i, j, 0));
                    }
                }
                else
                {
                    _wallIndex.Add(3);
                    _wallIndex.Add(4);
                    _wallPositions.Add(new Vector3(-10, j, 0));
                    _wallPositions.Add(new Vector3(10, j, 0));
                }
            }


            _levelCreator = new LevelCreator(walls, _wallIndex, _wallPositions);
            _bornPosition.Add(new Vector3(-15, 25, 0));
            _bornPosition.Add(new Vector3(15, 25, 0));
            _bornPosition.Add(new Vector3(0, 25, 0));
            OnSessionStart?.Invoke();
        }

        // Start is called before the first frame update
        private void Start()
        {
            CreateBornPoint();
            _levelCreator.CreateMap(this);
            AiCreateEnemy();
        }

        private void AiCreateEnemy()
        {
            IEnumerator CreateTask()
            {
                while (_leftEnemy > 0)
                {
                    yield return new WaitForSeconds(5);

                    foreach (var bornPoint in _bornPoints)
                    {
                        if (!bornPoint.NoTankInPoint() || _leftEnemy <= 0) continue;
                        var tankIndex = Random.Range(0, 10) % 3;
                        Instantiate(_enemyTanks[tankIndex], bornPoint.PointPosition(), Quaternion.identity).transform
                                .parent =
                            transform;
                        _curEnemyCount++;
                        _leftEnemy--;
                    }
                }
            }

            StartCoroutine(CreateTask());
        }

        private void CreateBornPoint()
        {
            foreach (var point in _bornPosition)
            {
                var bornPoint = Instantiate(bornPointPrefabs, point, Quaternion.identity);
                bornPoint.transform.parent = transform;
                _bornPoints.Add(bornPoint.GetComponent<BornPoint>());
            }
        }

        public void OnEnemyDestroy(GameObject o)
        {
            if (o.CompareTag("Enemy"))
            {
                CreateBox(o.transform.position);
                _killedEnemyCount++;
                _curEnemyCount--;
                if (_curEnemyCount <= 0 && _leftEnemy <= 0)
                {
                    OnSessionEnd?.Invoke();
                    GameObject session;
                    (session = gameObject).SetActive(false);
                    Destroy(session);
                }
            }
            else if (o.CompareTag("Player"))
            {
                GameSessionController.GetInstance().GameOver();
            }
        }

        public void CreateBox(Vector3 position)
        {
            if (Random.Range(0, 100) > 10)
            {
                Debug.Log("CreateBox");

                var obj = Resources.Load<GameObject>("Prefabs/UpgradeBox");
                var boxPosition = new Vector3(position.x, position.y, 0);
                Instantiate(obj, boxPosition, Quaternion.identity);
            }
        }

        public int LeftEnemyCount() => _leftEnemy;

        public int CurEnemyCount() => _curEnemyCount;

        public int MaxEnemyCount() => MaxEnemy;

        public int KilledEnemyCount() => _killedEnemyCount;

        public MonoBehaviour GetContext() => this;

        public Vector3 PlayerBornPoint() => new Vector3(0, 1, 0);

        public void DestroySession()
        {
            gameObject.SetActive(false);
            Destroy(this);
        }

        public void Reset()
        {
            _curEnemyCount = 0;
            _killedEnemyCount = 0;
        }
    }
}