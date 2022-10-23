using BSTW.Utils;
using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class PropProjectileTarget : ProjectileTarget
    {
        public override void Hit(Hit hit, GameObject vfx, Vector3 point, Projectile projectile)
        {
            PlayHitEffects(vfx, point, projectile);
            OnHit?.Invoke(hit);
        }
    }
}

