using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class PropProjectileTarget : ProjectileTarget
    {
        public override void Hit(float damage, GameObject vfx, Vector3 point)
        {
            PlayHitEffects(vfx, point);
            OnHit?.Invoke(damage);
        }
    }
}

