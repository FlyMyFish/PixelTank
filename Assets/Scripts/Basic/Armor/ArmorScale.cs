namespace Basic.Armor
{
    public struct ArmorScale
    {
        public float LifeScale { private set; get; }
        public float DefenceScale { private set; get; }
        public float WeightScale { private set; get; }

        public ArmorScale(float lifeScale, float defenceScale, float weightScale)
        {
            LifeScale = lifeScale;
            DefenceScale = defenceScale;
            WeightScale = weightScale;
        }
    }
}