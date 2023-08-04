using System;

namespace Basic.Armor
{
    public class AbsArmor : IArmor
    {
        protected static readonly ArmorScale HeavyScale = new ArmorScale(1.5f, 1.5f, 1.5f);
        protected static readonly ArmorScale MediumScale = new ArmorScale(1.2f, 1.2f, 1.2f);
        protected static readonly ArmorScale LightScale = new ArmorScale(1, 1, 1);
        protected float CurLife;
        private readonly ArmorScale _scale;
        public event Action<IConsumable> OnEmpty;
        public event Action Die;

        protected AbsArmor(ArmorScale scale)
        {
            _scale = scale;
            CurLife = BasicAttributeManager.Life * _scale.LifeScale;
        }

        public float CurrentLife()
        {
            return CurLife;
        }

        public float MaxLife()
        {
            return BasicAttributeManager.Life * _scale.LifeScale;
        }

        public float CurDefence()
        {
            return BasicAttributeManager.Armor * _scale.DefenceScale;
        }

        public float CurWeight()
        {
            return BasicAttributeManager.Weight * _scale.WeightScale;
        }

        public void TakenDamage(float damage)
        {
            CurLife -= damage - CurDefence();
            CurLife = Math.Max(CurLife, 0);
            if (CurLife <= 0)
            {
                OnEmpty?.Invoke(this);
                Die?.Invoke();
            }
        }

        public void Full(IConsumable type)
        {
            if (type is IArmor)
            {
                CurLife = MaxLife();
            }
        }
    }
}