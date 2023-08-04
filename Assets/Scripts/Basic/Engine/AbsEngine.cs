using System;

namespace Basic.Engine
{
    public abstract class AbsEngine : IEngine
    {
        protected const float SpeedScaleLight = 1, SpeedScaleMedium = 1.5f, SpeedScaleHeavy = 2f;

        private readonly float _scale;

        //初始化为-1，表示不可摧毁
        private readonly int _maxHitCount;
        protected int LeftHit;
        public event Action<IConsumable> OnEmpty;

        protected AbsEngine(float scale, int maxHitCount)
        {
            _scale = scale;
            _maxHitCount = maxHitCount;
            LeftHit = _maxHitCount;
        }

        public float SpeedScale()
        {
            return _scale;
        }

        public float Speed()
        {
            return _scale * BasicAttributeManager.Speed;
        }

        public int LeftHitCount()
        {
            return LeftHit == -1 ? 1 : LeftHit;
        }

        public int MaxHitCount()
        {
            return _maxHitCount;
        }

        public void OnHit()
        {
            LeftHit--;
            if (LeftHit <= 0)
            {
                OnEmpty?.Invoke(this);
            }
        }

        public void Full(IConsumable type)
        {
            if (type is IEngine)
            {
                LeftHit = MaxHitCount();
            }
        }
    }
}