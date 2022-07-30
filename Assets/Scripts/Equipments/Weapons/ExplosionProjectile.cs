using BSTW.Equipments.Weapons.Shooting;
using BSTW.Player;
using System.Collections.Generic;
using UnityEngine;

namespace BSTW.Equipments.Weapons
{
    public class ExplosionProjectile : Projectile
    {
        private PlayerCameraShake _playerCameraShake;
        private List<ProjectileTarget> targets = new List<ProjectileTarget>();

        private void Awake()
        {
            _playerCameraShake = FindObjectOfType<PlayerCameraShake>();
        }

        protected override void HitTarget()
        {
            _playerCameraShake.CalculateShakeBasedOnDistance(transform.position);

            targets.Clear();

            SetTargets(transform.position);

            projectileTarget.Hit(0f, hitVFXPooling?.GetObject(), transform.position);

            foreach (var target in targets)
                target.Hit(damage, null, Vector3.zero);

            ApplyHitForce(transform.position);
        }

        private void SetTargets(Vector3 point)
        {
            var colliders = Physics.OverlapSphere(point, projectileData.HitRadius, LayerMask.GetMask(targetNames));

            foreach (Collider collider in colliders)
            {
                var target = collider.GetComponent<ProjectileTarget>();

                if (target != null && !targets.Contains(target))
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

