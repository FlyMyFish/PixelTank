using System;
using UnityEngine;

namespace Basic.Weapon
{
    public interface IWeapon : IConsumable
    {
        float Damage();
        int MaxBulletCount();
        int CurBulletCount();
        float FireCooldown();
        bool Fire();
    }
}