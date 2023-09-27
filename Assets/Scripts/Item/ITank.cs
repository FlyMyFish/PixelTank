using System;
using Basic;
using Basic.Armor;
using Basic.Engine;
using Basic.Weapon;

namespace Item
{
    public interface ITank : IArmor, IEngine, IWeapon, IUpgrade
    {
        float MoveSpeed();
        public event Action OnUpGrade;
        
        public event Action<int> OnArmorUpGrade;
        public event Action<int> OnWeaponUpGrade;
        public event Action<int> OnEngineUpGrade;
    }
}