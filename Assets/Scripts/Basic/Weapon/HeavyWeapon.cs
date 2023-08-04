using UnityEngine;

namespace Basic.Weapon
{
    public class HeavyWeapon : AbsWeapon
    {
        public HeavyWeapon(MonoBehaviour tank) : base(HeavyScale,tank)
        {
        }
    }
}