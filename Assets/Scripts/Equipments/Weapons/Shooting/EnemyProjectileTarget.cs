using BSTW.Utils;
using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class EnemyProjectileTarget : ProjectileTarget
    {
        [SerializeField] private Transform _defaultHitPoint;

        public override void Hit(Hit hit, GameObject vfx, Vector3 point, Projectile projectile)
        {
            OnHit?.Invoke(hit);

            if (projectile != null)
                projectile.OnProjectileHit?.Invoke();

            PlayHitEffects(vfx, point == Vector3.zero ? _defaultHitPoint.position : point, projectile);
        }
    }
}

