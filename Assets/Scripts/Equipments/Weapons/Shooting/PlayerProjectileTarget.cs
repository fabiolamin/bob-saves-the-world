using BSTW.Utils;
using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class PlayerProjectileTarget : ProjectileTarget
    {
        public override void Hit(Hit hit, GameObject vfx, Vector3 point)
        {
            OnHit?.Invoke(hit);
        }
    }
}

