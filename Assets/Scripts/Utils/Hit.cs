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
        public bool CanKnockDown;

        public Hit(HitType hitType, float damage, bool canKnockDown)
        {
            HitType = hitType;
            Damage = damage;
            CanKnockDown = canKnockDown;
        }
    }
}

