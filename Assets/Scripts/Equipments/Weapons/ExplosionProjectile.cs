using BSTW.Environment;
using UnityEngine;

namespace BSTW.Equipments.Weapons
{
    public class ExplosionProjectile : Projectile
    {
        [SerializeField] private Explosion _explosion;

        protected override void HitTarget()
        {
            _explosion.SetExplosion(targetNames, damage, projectileData.HitRadius, projectileData.HitForce, projectileData.HitUpwards);
            projectileTarget.Hit(0f, hitVFXPooling?.GetObject(), transform.position);
        }
    }
}

