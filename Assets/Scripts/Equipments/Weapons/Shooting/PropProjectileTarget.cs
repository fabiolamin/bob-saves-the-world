using BSTW.Utils;
using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class PropProjectileTarget : ProjectileTarget
    {
        public override void Hit(Hit hit, GameObject vfx, Vector3 point)
        {
            PlayHitEffects(vfx, point);
            OnHit?.Invoke(hit);
        }
    }
}

