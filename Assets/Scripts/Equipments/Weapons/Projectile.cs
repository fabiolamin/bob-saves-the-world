using BSTW.Data.Equipments.Weapons;
using BSTW.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Equipments.Weapons
{
    public class Projectile : MonoBehaviour
    {
        protected string targetName;
        protected float damage;
        protected List<ProjectileTarget> targets = new List<ProjectileTarget>();

        [SerializeField] private ProjectileData _projectileData;
        [SerializeField] private Rigidbody _projectileRb;
        [SerializeField] private Collider _projectileCollider;
        [SerializeField] private ObjectPooling _hitVFXPooling;

        [SerializeField] private UnityEvent _onShot;

        protected Rigidbody projectileRb => _projectileRb;

        private void OnCollisionEnter(Collision collision)
        {
            CheckTarget(collision);

            gameObject.SetActive(false);
        }

        private void CheckTarget(Collision collision)
        {
            var projectileTarget = collision.collider.GetComponent<ProjectileTarget>();
            if (projectileTarget is null) return;

            targets.Clear();
            targets.Add(projectileTarget);

            if (_projectileData.HitRadius > 0f)
            {
                SetTargets(transform.position);
            }

            ApplyHitForce(transform.position);

            HitTarget(projectileTarget, damage, _hitVFXPooling != null ? _hitVFXPooling.GetObject() : null,
            transform.position);
        }

        protected virtual void HitTarget(ProjectileTarget projectileTarget, float damage, GameObject vfxGO, Vector3 point)
        {
            if (_projectileData.HitRadius > 0f)
            {
                projectileTarget.Hit(0f, vfxGO, point);

                foreach (ProjectileTarget target in targets)
                {
                    if (target.tag.Equals(targetName))
                    {
                        target.Hit(damage, null, Vector3.zero);
                    }
                }

                return;
            }

            var hitDamage = projectileTarget.tag.Equals(targetName) ? damage : 0f;
            projectileTarget.Hit(hitDamage, vfxGO, point);
        }

        private void SetTargets(Vector3 point)
        {
            var colliders = Physics.OverlapSphere(point, _projectileData.HitRadius);

            foreach (Collider collider in colliders)
            {
                var target = collider.GetComponent<ProjectileTarget>();

                if (target == null) return;
                if (targets.Contains(target)) return;

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
                    _projectileData.HitForce,
                    point, _projectileData.HitRadius,
                    _projectileData.HitUpwards);
                }
            }
        }

        public void SetUpProjectile(string targetName, float damage, Transform origin)
        {
            this.targetName = targetName;
            this.damage = damage;

            EnablePhysics(false);
            transform.SetParent(origin);
            transform.position = origin.position;
            transform.rotation = origin.localRotation;
        }

        public void MoveTowards(Vector3 origin, Vector3 target)
        {
            _onShot?.Invoke();
            transform.LookAt(target);
            _projectileRb.velocity = (target - origin).normalized * _projectileData.Speed;
        }

        public void EnablePhysics(bool isEnabled)
        {
            _projectileCollider.enabled = isEnabled;
            _projectileRb.isKinematic = !isEnabled;
            _projectileRb.constraints = isEnabled ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
        }
    }
}

