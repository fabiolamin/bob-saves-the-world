using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class ObstacleProjectileTarget : ProjectileTarget
    {
        public override void Hit(float damage, GameObject vfx, Vector3 point)
        {
            if (point == Vector3.zero) return;

            PlayHitVFXOnPoint(vfx, point);
        }
    }
}

