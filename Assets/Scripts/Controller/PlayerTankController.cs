using Factory;
using Item;
using UnityEngine;

namespace Controller
{
    public class PlayerTankController : MonoBehaviour
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
        private Camera _gameCamera;
        private Vector3 _cameraPlayerOffset;
        private static readonly int Type = Animator.StringToHash("Type");

        private void Awake()
        {
            _gameCamera = Camera.main;
            _tank = TankFactory.CreateLightTank(this);

            _tank.Die += () =>
            {
                GameObject o;
                (o = gameObject).SetActive(false);
                Destroy(o);
            };
            _curEngine = engineLight;
            _armorAnim = armor.GetComponentInChildren<Animator>();
            _engineController = _curEngine.GetComponentInChildren<EngineController>();
            _weaponAnim = weapon.GetComponentInChildren<Animator>();
            _foreBox = foreBox.GetComponentInChildren<ForeBox>();
            _foreBox.OnTriggerChanged += b => { _blocked = b; };
            _lifeBarTransForm = lifeBar.transform;
        }

        void Start()
        {
            _cameraPlayerOffset = _gameCamera.transform.position - transform.position;
        }

        void Update()
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

            transform.eulerAngles = _forward;
            if (_isRunning && !_blocked)
            {
                Move();
            }

            _engineController.SetTrigger(_isRunning ? "Move" : "Idle");

            _lifeBarTransForm.localScale = new Vector3(_tank.CurrentLife() / _tank.MaxLife(), 0.2f, 1);

            _isRunning = false;
        }

        private void Move()
        {
            var curTransform = transform;
            curTransform.Translate(Vector3.up * _tank.MoveSpeed() * Time.deltaTime);
            _gameCamera.transform.position = curTransform.position + _cameraPlayerOffset;
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