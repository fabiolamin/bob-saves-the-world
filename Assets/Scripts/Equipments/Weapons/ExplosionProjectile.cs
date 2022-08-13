using BSTW.Environment;
using BSTW.Utils;
using UnityEngine;

namespace BSTW.Equipments.Weapons
{
    public class ExplosionProjectile : Projectile
    {
        [SerializeField] private Explosion _explosion;

        protected override void HitTarget()
        {
            _explosion.SetExplosion(targetNames, hit.Damage, projectileData.HitRadius, projectileData.HitForce, projectileData.HitUpwards);
            projectileTarget.Hit(new Hit(HitType.Explosion, 0f), hitVFXPooling?.GetObject(), transform.position);
        }
    }
}

