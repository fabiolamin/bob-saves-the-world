using BSTW.Environment;
using BSTW.Utils;
using UnityEngine;

namespace BSTW.Equipments.Weapons
{
    public class ExplosionProjectile : Projectile
    {
        private bool _exploded;

        [SerializeField] private Explosion _explosion;

        protected override void HitTarget()
        {
            if (_exploded) return;    

            _explosion.SetExplosion(targetNames, hit.Damage, projectileData.HitRadius, projectileData.HitForce, projectileData.HitUpwards);
            projectileTarget.Hit(new Hit(HitType.Explosion, 0f, true), hitVFXPooling?.GetObject(), transform.position, this);

            _exploded = true;
        }

        public override void GetReadyToMove(Vector3 origin, Vector3 target)
        {
            base.GetReadyToMove(origin, target);

            _exploded = false;
        }
    }
}