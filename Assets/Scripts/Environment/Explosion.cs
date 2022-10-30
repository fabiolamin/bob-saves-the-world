using BSTW.Equipments.Weapons.Shooting;
using BSTW.Player;
using BSTW.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BSTW.Environment
{
    public class Explosion : MonoBehaviour
    {
        private PlayerCameraShake _playerCameraShake;
        private List<ProjectileTarget> _targets = new List<ProjectileTarget>();


        [Header("Damage based on distance")]
        [SerializeField] private float _damageDistanceConstant = 50f;
        [SerializeField] private float _maxDamageDistance = 50f;

        private void Awake()
        {
            _playerCameraShake = FindObjectOfType<PlayerCameraShake>();
        }

        public void SetExplosion(float damage, float radius, float force, float upwardsForce)
        {
            Explode(GetColliders(radius), damage, radius, force, upwardsForce);
        }

        public void SetExplosion(string[] targetLayers, float damage, float radius, float force, float upwardsForce)
        {
            Explode(GetColliders(targetLayers, radius), damage, radius, force, upwardsForce);
        }

        private void Explode(Collider[] colliders, float damage, float radius, float force, float upwardsForce)
        {
            _playerCameraShake.CalculateShakeBasedOnDistance(transform.position);

            AddTargets(colliders);

            foreach (ProjectileTarget target in _targets)
            {
                target.Hit(
                new Hit(HitType.Explosion, Utilities.GetValueBasedOnDistace(
                transform.position,
                target.transform.position,
                _damageDistanceConstant,
                damage,
                _maxDamageDistance), true),
                null,
                Vector3.zero, null);

                if (target.ProjectileTargetRb != null)
                {
                    target.ProjectileTargetRb.AddExplosionForce(
                    force,
                    transform.position, radius,
                    upwardsForce);
                }
            }
        }

        private Collider[] GetColliders(float radius)
        {
            return Physics.OverlapSphere(transform.position, radius);
        }

        private Collider[] GetColliders(string[] targetLayers, float radius)
        {
            return Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask(targetLayers));
        }

        private void AddTargets(Collider[] colliders)
        {
            _targets.Clear();

            foreach (Collider collider in colliders)
            {
                var target = collider.GetComponent<ProjectileTarget>();

                if (target != null && !_targets.Contains(target))
                    _targets.Add(target);
            }
        }
    }
}

