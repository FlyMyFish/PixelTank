using UnityEngine;

namespace Basic.Weapon
{
    public class MediumWeapon : AbsWeapon
    {
        public MediumWeapon(MonoBehaviour tank) : base(MediumScale, tank)
        {
        }
    }
}