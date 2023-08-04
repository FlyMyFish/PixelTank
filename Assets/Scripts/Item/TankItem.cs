using System;
using Basic;
using Basic.Armor;
using Basic.Engine;
using Basic.Weapon;
using UnityEngine;

namespace Item
{
    public class TankItem : ITank
    {
        private IArmor _armor, _mediumArmor, _heavyArmor;
        private IWeapon _weapon, _mediumWeapon, _heavyWeapon;
        private IEngine _engine, _mediumEngine, _heavyEngine;
        public event Action<IConsumable> OnEmpty;
        public event Action OnUpGrade;
        public event Action Die;

        public TankItem(IArmor armor, IWeapon weapon, IEngine engine)
        {
            _armor = armor;
            _armor.Die += TankDie;
            _weapon = weapon;
            _engine = engine;
        }

        public void UpGrade(IConsumable type)
        {
            type.OnEmpty += Empty;
            switch (type)
            {
                case IArmor armor:
                    UpGradeArmor(armor);
                    break;
                case IEngine engine:
                    UpGradeEngine(engine);
                    break;
                case IWeapon weapon:
                    UpGradeWeapon(weapon);
                    break;
            }

            OnUpGrade?.Invoke();
        }

        private void UpGradeWeapon(IWeapon weapon)
        {
            switch (weapon)
            {
                case HeavyWeapon heavyWeapon:
                    _heavyWeapon = heavyWeapon;
                    break;
                case MediumWeapon mediumWeapon:
                    _mediumWeapon = mediumWeapon;
                    break;
            }
        }

        private void UpGradeArmor(IArmor armor)
        {
            switch (armor)
            {
                case HeavyArmor heavyArmor:
                    _heavyArmor = heavyArmor;
                    return;
                case MediumArmor mediumArmor:
                    _mediumArmor = mediumArmor;
                    return;
                default:
                    _armor.Full(armor);
                    break;
            }
        }

        private void UpGradeEngine(IEngine engine)
        {
            switch (engine)
            {
                case HeavyEngine heavyEngine:
                    _heavyEngine = heavyEngine;
                    break;
                case MediumEngine mediumEngine:
                    _mediumEngine = mediumEngine;
                    break;
            }
        }

        public void Full(IConsumable type)
        {
            CurArmor().Full(type);
            CurWeapon().Full(type);
            CurEngine().Full(type);
        }

        private void Empty(IConsumable type)
        {
            switch (type)
            {
                case HeavyArmor heavyArmor:
                    _heavyArmor = null;
                    break;
                case MediumArmor mediumArmor:
                    _mediumArmor = null;
                    break;
                case HeavyEngine heavyEngine:
                    _heavyEngine = null;
                    break;
                case MediumEngine mediumEngine:
                    _mediumEngine = null;
                    break;
                case HeavyWeapon heavyWeapon:
                    _heavyWeapon = null;
                    break;
                case MediumWeapon mediumWeapon:
                    _mediumWeapon = null;
                    break;
            }

            OnEmpty?.Invoke(type);
        }

        public float CurrentLife()
        {
            return CurArmor().CurrentLife();
        }

        public float MaxLife()
        {
            return CurArmor().MaxLife();
        }

        public float CurDefence()
        {
            return CurArmor().CurDefence();
        }

        public float CurWeight()
        {
            return CurArmor().CurWeight();
        }

        public void TakenDamage(float damage)
        {
            CurArmor().TakenDamage(damage);
            OnHit();
        }

        public float SpeedScale()
        {
            return CurEngine().SpeedScale();
        }

        public float Speed()
        {
            return CurEngine().Speed();
        }

        public int LeftHitCount()
        {
            return CurEngine().LeftHitCount();
        }

        public int MaxHitCount()
        {
            return CurEngine().MaxHitCount();
        }

        public void OnHit()
        {
            CurEngine().OnHit();
        }

        public float Damage()
        {
            return CurWeapon().Damage();
        }

        public int MaxBulletCount()
        {
            return CurWeapon().MaxBulletCount();
        }

        public int CurBulletCount()
        {
            return CurWeapon().CurBulletCount();
        }

        public float FireCooldown()
        {
            return CurWeapon().FireCooldown();
        }

        public bool Fire()
        {
            return CurWeapon().Fire();
        }

        public float MoveSpeed()
        {
            return Speed() / CurWeight();
        }

        private void TankDie()
        {
            Die?.Invoke();
        }

        private IArmor CurArmor()
        {
            if (_heavyArmor != null) return _heavyArmor;
            return _mediumArmor ?? _armor;
        }

        private IEngine CurEngine()
        {
            if (_heavyEngine != null) return _heavyEngine;
            return _mediumEngine ?? _engine;
        }

        private IWeapon CurWeapon()
        {
            if (_heavyWeapon != null) return _heavyWeapon;
            return _mediumWeapon ?? _weapon;
        }
    }
}