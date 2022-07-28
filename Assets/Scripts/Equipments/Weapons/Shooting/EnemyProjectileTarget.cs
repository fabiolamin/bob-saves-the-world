using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class EnemyProjectileTarget : ProjectileTarget
    {
        [SerializeField] private Transform _defaultHitPoint;

        public override void Hit(float damage, GameObject vfx, Vector3 point)
        {
            OnHit?.Invoke(damage);

            PlayHitEffects(vfx, point == Vector3.zero ? _defaultHitPoint.position : point);
        }
    }
}

