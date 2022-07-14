using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class PlayerProjectileTarget : ProjectileTarget
    {
        public override void Hit(float damage, GameObject vfx, Vector3 point)
        {
            OnHit?.Invoke(damage);
        }
    }
}

