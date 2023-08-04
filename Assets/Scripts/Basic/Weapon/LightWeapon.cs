using UnityEngine;

namespace Basic.Weapon
{
    public class LightWeapon : AbsWeapon
    {
        public LightWeapon(MonoBehaviour tank) : base(LightScale,tank)
        {
        }
    }
}