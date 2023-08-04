using System;

namespace Basic.Engine
{
    public interface IEngine : IConsumable
    {
        float SpeedScale();
        float Speed();
        int LeftHitCount();
        int MaxHitCount();

        void OnHit();
    }
}