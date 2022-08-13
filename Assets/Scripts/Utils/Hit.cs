namespace BSTW.Utils
{
    public enum HitType
    {
        Melee,
        Projectile,
        Explosion
    }

    [System.Serializable]
    public struct Hit
    {
        public HitType HitType;
        public float Damage;

        public Hit(HitType hitType, float damage)
        {
            HitType = hitType;
            Damage = damage;
        }
    }
}

