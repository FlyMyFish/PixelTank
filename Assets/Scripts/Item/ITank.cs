using Basic;
using Basic.Armor;
using Basic.Engine;
using Basic.Weapon;

namespace Item
{
    public interface ITank : IArmor, IEngine, IWeapon, IUpgrade
    {
        float MoveSpeed();
    }
}