using System;
using System.Collections;
using Item;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Basic.Weapon
{
    public class AbsWeapon : IWeapon
    {
        protected static readonly (float damageScale, float cooldownScale, int maxMumBullet) LightScale = (1, 1, -1),
            MediumScale = (1.5f, 1.2f, 50),
            HeavyScale = (2, 1.5f, 80);

        private readonly (float damageScale, float cooldownScale, int maxMumBullet) _scale;
        private int _curBullet;
        private bool _fireCooldown;
        private Vector3 _bulletPositionOffset = new Vector3(0, 0, 0);
        private readonly GameObject _bulletPrefab;
        private readonly MonoBehaviour _tank;
        private readonly AudioClip _shootSound;
        private readonly AudioSource _shootSoundAudioSource;
        public event Action<IConsumable> OnEmpty;

        protected AbsWeapon((float damageScale, float cooldownScale, int maxMumBullet) scale, MonoBehaviour tank)
        {
            _scale = scale;
            _curBullet = scale.maxMumBullet;
            _tank = tank;
            _bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
            _shootSound = Resources.Load<AudioClip>("Sounds/Tank_shoot");
            _shootSoundAudioSource = _tank.GetComponent<AudioSource>();
        }

        public float Damage()
        {
            return BasicAttributeManager.Damage * _scale.damageScale;
        }

        public int MaxBulletCount()
        {
            return _scale.maxMumBullet;
        }

        public int CurBulletCount()
        {
            return _curBullet;
        }

        public float FireCooldown()
        {
            return BasicAttributeManager.FireCoolDown * _scale.cooldownScale;
        }

        public bool Fire()
        {
            if (_fireCooldown) return false;
            if (_tank.CompareTag("Player"))
            {
                _shootSoundAudioSource.PlayOneShot(_shootSound);
                _curBullet--;
                if (_curBullet <= 0)
                {
                    OnEmpty?.Invoke(this);
                }
            }

            var bullet = Object.Instantiate(_bulletPrefab, BulletPosition(), Quaternion.identity);
            var bulletItem = bullet.GetComponent<BulletItem>();
            bulletItem.BulletDamage = Damage();
            bulletItem.From = _tank.tag;
            bullet.transform.eulerAngles = _tank.transform.eulerAngles;
            bullet.GetComponent<Rigidbody2D>().velocity = BulletForward() * BasicAttributeManager.BulletSpeed;
            _tank.StartCoroutine(WeaponCoolDown());
            return true;
        }

        private IEnumerator WeaponCoolDown()
        {
            _fireCooldown = true;
            var curTimeTemp = Time.unscaledTime * 1000;
            yield return new WaitUntil(() => Time.unscaledTime * 1000 - curTimeTemp >= FireCooldown());
            _fireCooldown = false;
        }

        public void Full(IConsumable type)
        {
            if (type is IWeapon)
            {
                _curBullet = MaxBulletCount();
            }
        }

        private Vector3 BulletPosition()
        {
            switch (_tank.transform.eulerAngles.z)
            {
                case 0:
                    _bulletPositionOffset.x = 0;
                    _bulletPositionOffset.y = 1f;
                    break;
                case 90:
                    _bulletPositionOffset.x = -1f;
                    _bulletPositionOffset.y = 0;
                    break;
                case 180:
                    _bulletPositionOffset.x = 0;
                    _bulletPositionOffset.y = -1f;
                    break;
                case 270:
                    _bulletPositionOffset.x = 1f;
                    _bulletPositionOffset.y = 0;
                    break;
            }

            return _tank.transform.position + _bulletPositionOffset;
        }

        private Vector2 BulletForward()
        {
            return _tank.transform.eulerAngles.z switch
            {
                0 => Vector2.up,
                90 => Vector2.left,
                180 => Vector2.down,
                270 => Vector2.right,
                _ => Vector2.up
            };
        }
    }
}