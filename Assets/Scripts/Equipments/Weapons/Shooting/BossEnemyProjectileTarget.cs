using BSTW.Utils;
using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class BossEnemyProjectileTarget : EnemyProjectileTarget
    {
        [SerializeField] private float _additionalHitDamage = 0f;

        public override void Hit(Hit hit, GameObject vfx, Vector3 point, Projectile projectile)
        {
            hit.Damage += _additionalHitDamage;

            base.Hit(hit, vfx, point, projectile);
        }
    }
}

