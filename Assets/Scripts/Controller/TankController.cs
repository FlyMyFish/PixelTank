using System.Collections;
using Factory;
using Item;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller
{
    public class TankController : MonoBehaviour
    {
        private ITank _tank;
        private GameObject _objectEngine;
        private GameObject _objectArmor;
        private GameObject _objectWeapon;
        private GameObject _lifeBar;
        private Transform _lifeBarTransForm;
        private Animator _engineAnim;
        private bool _isRunning;
        private bool _player;

        private Camera _gameCamera;
        private Vector3 _cameraPlayerOffset;

        private Vector3 _forward = new Vector3(0, 0, 0);
        private bool _blocked;

        private void Awake()
        {
            _gameCamera = Camera.main;
            _player = gameObject.CompareTag("Player");
            _tank ??= _player ? TankFactory.CreateLightTank(this) : TankFactory.CreatRandomTank(this);

            _tank.Die += () =>
            {
                GameObject o;
                (o = gameObject).SetActive(false);
                Destroy(o);
            };
            _objectEngine = gameObject.transform.GetChild(0).gameObject;
            _objectArmor = gameObject.transform.GetChild(1).gameObject;
            _objectWeapon = gameObject.transform.GetChild(2).gameObject;
            _lifeBar = gameObject.transform.GetChild(4).gameObject;
            _lifeBarTransForm = _lifeBar.transform;
            _engineAnim = _objectEngine.GetComponent<Animator>();
        }

        private void Start()
        {
            if (_player)
            {
                _cameraPlayerOffset = _gameCamera.transform.position - transform.position;
            }
            else
            {
                EnemyAi();
            }
        }

        private void Update()
        {
            if (_player)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    _isRunning = true;
                    TurnLeft();
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    _isRunning = true;
                    TurnDown();
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    _isRunning = true;
                    TurnRight();
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    _isRunning = true;
                    TurnUp();
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    FireBullet();
                }
            }

            transform.eulerAngles = _forward;
            if (_isRunning && !_blocked)
            {
                Move();
            }

            _engineAnim.SetTrigger(_isRunning ? "Move" : "Idle");
            _lifeBarTransForm.localScale = new Vector3(_tank.CurrentLife() / _tank.MaxLife(), 0.2f, 1);
            if (_player)
            {
                _isRunning = false;
            }
        }

        private void Move()
        {
            transform.Translate(Vector3.up * _tank.MoveSpeed() * Time.deltaTime);
            if (_player)
            {
                _gameCamera.transform.position = transform.position + _cameraPlayerOffset;
            }
        }

        private void FireBullet()
        {
            _tank.Fire();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Bullet"))
            {
                var bulletItem = col.gameObject.GetComponent<BulletItem>();
                _tank.TakenDamage(bulletItem.BulletDamage);
            }
            else
            {
                _blocked = col.gameObject.CompareTag("GameBuild");
            }

            if (!_player)
            {
                RandomTurnAround();
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("GameBuild"))
            {
                _blocked = false;
            }
        }

        private void EnemyAi()
        {
            TurnDown();
            _isRunning = true;
            //默认Move状态
            //随机时间定时转向
            StartCoroutine(AttemptTurnAround());
            //定时射击(添加危险性0-1,危险性为1则表示每次射击必定成功)
            StartCoroutine(AttemptFire());
        }

        private IEnumerator AttemptTurnAround()
        {
            while (gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(Random.Range(0, 8));
                RandomTurnAround();
            }
        }

        private IEnumerator AttemptFire()
        {
            while (gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(1);
                if (Random.Range(0, 100) <= 70)
                {
                    FireBullet();
                }
            }
        }

        private void RandomTurnAround()
        {
            if (Random.Range(0, 100) <= 70)
            {
                _forward.z = Random.Range(0, 4) % 4 * 90;
            }
        }

        private void OnDestroy()
        {
            GameSessionController.GetInstance().OnEnemyDestroy(gameObject);
        }

        private void TurnLeft()
        {
            _forward.z = 90;
        }

        private void TurnRight()
        {
            _forward.z = 270;
        }

        private void TurnUp()
        {
            _forward.z = 0;
        }

        private void TurnDown()
        {
            _forward.z = 180;
        }
    }
}