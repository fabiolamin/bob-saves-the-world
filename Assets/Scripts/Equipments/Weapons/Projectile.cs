using BSTW.Data.Equipments.Weapons;
using BSTW.Equipments.Weapons.Shooting;
using BSTW.Utils;
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
        protected float damage;

        [SerializeField] private ProjectileData _projectileData;
        [SerializeField] private Rigidbody _projectileRb;
        [SerializeField] private Collider _projectileCollider;
        [SerializeField] private ObjectPooling _hitVFXPooling;

        [SerializeField] private UnityEvent _onShot;

        protected Rigidbody projectileRb => _projectileRb;

        private void OnCollisionEnter(Collision collision)
        {
            projectileTarget = collision.collider.GetComponent<ProjectileTarget>();
            if (projectileTarget == null) return;

            CheckTarget(collision);

            _canMove = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            MoveTowardsTarget();
        }

        protected virtual void CheckTarget(Collision collision)
        {
            projectileTarget.Hit(damage, null, transform.position);
        }

        private void MoveTowardsTarget()
        {
            if (_canMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * projectileData.Speed);
            }
        }

        public void SetUpProjectile(int layer, string[] targetNames, float damage, Transform origin)
        {
            gameObject.layer = layer;
            this.targetNames = targetNames;
            this.damage = damage;

            EnablePhysics(false);
            transform.SetParent(origin);
            transform.position = origin.position;
            transform.rotation = origin.localRotation;
        }

        public void GetReadyToMove(Vector3 origin, Vector3 target)
        {
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

