using System;

namespace Basic.Armor
{
    public interface IArmor : IConsumable
    {
        float CurrentLife();
        float MaxLife();
        float CurDefence();
        float CurWeight();
        void TakenDamage(float damage);

        event Action Die;
    }
}