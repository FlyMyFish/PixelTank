using System.Collections;
using Factory;
using Item;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller
{
    public class EnemyTankController : MonoBehaviour
    {
        public GameObject armor;

        public GameObject weapon;

        public GameObject engineLight;

        public GameObject engineMedium;

        public GameObject engineHeavy;

        public GameObject foreBox;

        public GameObject lifeBar;

        private GameObject _curEngine;
        private Animator _armorAnim, _weaponAnim;
        private bool _isRunning;
        private ForeBox _foreBox;
        private bool _blocked;
        private Transform _lifeBarTransForm;
        private ITank _tank;
        private EngineController _engineController;
        private Vector3 _forward = new Vector3(0, 0, 0);
        private static readonly int Type = Animator.StringToHash("Type");

        private void Start()
        {
            var engineType = Random.Range(0, 10) % 3;
            var armorType = Random.Range(0, 10) % 3;
            var weaponType = Random.Range(0, 10) % 3;
            _tank = TankFactory.CreateTankByParam(armorType, weaponType, engineType, this);

            _tank.Die += () =>
            {
                GameObject o;
                (o = gameObject).SetActive(false);
                Destroy(o);
            };
            _curEngine = engineType switch
            {
                1 => engineMedium,
                2 => engineHeavy,
                _ => engineLight
            };
            engineLight.SetActive(false);
            engineMedium.SetActive(false);
            engineHeavy.SetActive(false);
            _curEngine.SetActive(true);
            _armorAnim = armor.GetComponentInChildren<Animator>();
            _engineController = _curEngine.GetComponentInChildren<EngineController>();
            _weaponAnim = weapon.GetComponentInChildren<Animator>();
            _foreBox = foreBox.GetComponentInChildren<ForeBox>();
            _foreBox.OnTriggerChanged += b =>
            {
                _blocked = b;
                if (_blocked)
                {
                    RandomTurnAround();
                }
            };
            _lifeBarTransForm = lifeBar.transform;
            _armorAnim.SetInteger(Type, armorType);
            _weaponAnim.SetInteger(Type, weaponType);
            EnemyAi();
        }

        private void Update()
        {
            transform.eulerAngles = _forward;
            if (_isRunning && !_blocked)
            {
                Move();
            }

            _engineController.SetTrigger(_isRunning ? "Move" : "Idle");
            _lifeBarTransForm.localScale = new Vector3(_tank.CurrentLife() / _tank.MaxLife(), 0.2f, 1);
        }

        private void Move()
        {
            transform.Translate(Vector3.up * _tank.MoveSpeed() * Time.deltaTime);
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

        private void FireBullet()
        {
            _tank.Fire();
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

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Bullet"))
            {
                var bulletItem = col.gameObject.GetComponent<BulletItem>();
                if (!gameObject.CompareTag(bulletItem.From))
                {
                    _tank.TakenDamage(bulletItem.BulletDamage);
                }
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