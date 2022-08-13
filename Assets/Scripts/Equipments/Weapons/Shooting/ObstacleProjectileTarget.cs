using BSTW.Utils;
using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class ObstacleProjectileTarget : ProjectileTarget
    {
        public override void Hit(Hit hit, GameObject vfx, Vector3 point)
        {
            if (point == Vector3.zero) return;

            PlayHitEffects(vfx, point);
        }
    }
}

