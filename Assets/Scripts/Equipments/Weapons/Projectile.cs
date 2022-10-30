using BSTW.Data.Equipments.Weapons;
using BSTW.Equipments.Weapons.Shooting;
using BSTW.Utils;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Equipments.Weapons
{
    public class Projectile : MonoBehaviour
    {
        private bool _canMove = false;
        private Vector3 _targetPosition;

        protected ProjectileData projectileData => _projectileData;
        protected ProjectileTarget projectileTarget;
        protected ObjectPooling hitVFXPooling => _hitVFXPooling;
        protected string[] targetNames;
        protected Hit hit;

        [SerializeField] private ProjectileData _projectileData;
        [SerializeField] private Rigidbody _projectileRb;
        [SerializeField] private ObjectPooling _hitVFXPooling;
        [SerializeField] private UnityEvent _onShot;

        [Header("Collider")]
        [SerializeField] private SphereCollider _projectileCollider;
        [SerializeField] private float _increaseColliderDist = 2f;
        [SerializeField] private float _defaultColliderRadius = 0.5f;
        [SerializeField] private float _maxColliderRadius = 2f;

        public Action OnProjectileHit;

        protected Rigidbody projectileRb => _projectileRb;

        private void OnCollisionEnter(Collision collision)
        {
            projectileTarget = collision.collider.GetComponent<ProjectileTarget>();
            if (projectileTarget == null) return;

            _canMove = false;

            HitTarget();

            gameObject.SetActive(false);
        }

        private void Update()
        {
            MoveTowardsTarget();
        }

        protected virtual void HitTarget()
        {
            if (projectileTarget.ProjectileTargetRb != null)
                projectileTarget.ProjectileTargetRb.AddForceAtPosition(transform.forward * projectileData.HitForce, transform.position);

            projectileTarget.Hit(hit, null, transform.position, this);
        }

        private void MoveTowardsTarget()
        {
            if (_canMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * projectileData.Speed);

                if (IsProjectileNearTarget())
                {
                    _projectileCollider.radius = _maxColliderRadius;
                }

                if (!_projectileData.RapidFire)
                    _targetPosition += transform.forward * projectileData.Speed;
            }
        }

        private bool IsProjectileNearTarget()
        {
            return Vector3.Distance(transform.position, _targetPosition) < _increaseColliderDist;
        }

        public void SetUpProjectile(int layer, string[] targetNames, Hit hit, Transform origin)
        {
            _canMove = false;
            gameObject.layer = layer;
            this.targetNames = targetNames;
            this.hit = hit;

            EnablePhysics(false);
            transform.SetParent(origin);
            transform.position = origin.position;
            transform.rotation = origin.localRotation;
        }

        public void GetReadyToMove(Vector3 origin, Vector3 target)
        {
            _projectileCollider.radius = _defaultColliderRadius;

            EnablePhysics(true);
            gameObject.SetActive(true);
            transform.SetParent(null);

            _onShot?.Invoke();

            transform.LookAt(target);
            transform.position = origin;

            _targetPosition = target;
            _canMove = true;
        }

        private void EnablePhysics(bool isEnabled)
        {
            _projectileCollider.enabled = isEnabled;
            _projectileRb.isKinematic = !isEnabled;
            _projectileRb.constraints = isEnabled ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
        }
    }
}

