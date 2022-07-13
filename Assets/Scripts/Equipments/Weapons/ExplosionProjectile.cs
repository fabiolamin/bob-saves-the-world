using System.Collections.Generic;
using UnityEngine;

namespace BSTW.Equipments.Weapons
{
    public class ExplosionProjectile : Projectile
    {
        private List<ProjectileTarget> targets = new List<ProjectileTarget>();

        protected override void CheckTarget(Collision collision)
        {
            targets.Clear();

            SetTargets(transform.position);

            projectileTarget.Hit(hitVFXPooling?.GetObject(), transform.position);

            foreach (var target in targets)
                target.Hit(damage);

            ApplyHitForce(transform.position);
        }

        private void SetTargets(Vector3 point)
        {
            var colliders = Physics.OverlapSphere(point, projectileData.HitRadius, LayerMask.GetMask(targetNames));

            foreach (Collider collider in colliders)
            {
                var target = collider.GetComponent<ProjectileTarget>();

                if (target == null && targets.Contains(target)) return;

                targets.Add(target);
            }
        }

        private void ApplyHitForce(Vector3 point)
        {
            foreach (ProjectileTarget target in targets)
            {
                if (target.ProjectileTargetRb != null)
                {
                    target.ProjectileTargetRb.AddExplosionForce(
                    projectileData.HitForce,
                    point, projectileData.HitRadius,
                    projectileData.HitUpwards);
                }
            }
        }
    }
}

