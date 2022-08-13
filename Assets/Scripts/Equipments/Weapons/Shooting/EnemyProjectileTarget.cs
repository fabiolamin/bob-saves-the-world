using BSTW.Utils;
using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class EnemyProjectileTarget : ProjectileTarget
    {
        [SerializeField] private Transform _defaultHitPoint;

        public override void Hit(Hit hit, GameObject vfx, Vector3 point)
        {
            OnHit?.Invoke(hit);

            PlayHitEffects(vfx, point == Vector3.zero ? _defaultHitPoint.position : point);
        }
    }
}

