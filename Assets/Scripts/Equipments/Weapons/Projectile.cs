using BSTW.Data.Equipments.Weapons;
using BSTW.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Equipments.Weapons
{
    public class Projectile : MonoBehaviour
    {
        protected float damage;

        [SerializeField] private Rigidbody _projectileRb;
        [SerializeField] private Collider _projectileCollider;
        [SerializeField] private ObjectPooling _hitVFXPooling;

        [SerializeField] private UnityEvent _onShot;

        [SerializeField] private float _speed = 5f;

        private void OnCollisionEnter(Collision collision)
        {
            CheckTarget(collision);

            gameObject.SetActive(false);
        }

        private void CheckTarget(Collision collision)
        {
            var projectileTarget = collision.collider.GetComponent<ProjectileTarget>();
            if (projectileTarget is null) return;

            HitTarget(projectileTarget, damage, _hitVFXPooling != null ? _hitVFXPooling.GetObject() : null,
            transform.position);
        }

        protected virtual void HitTarget(ProjectileTarget projectileTarget, float damage, GameObject vfxGO, Vector3 point)
        {
            projectileTarget.Hit(damage, vfxGO, point);
        }

        public void SetUpProjectile(float damage, Transform origin)
        {
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
            _projectileRb.velocity = (target - origin).normalized * _speed;
        }

        public void EnablePhysics(bool isEnabled)
        {
            _projectileCollider.enabled = isEnabled;
            _projectileRb.isKinematic = !isEnabled;
            _projectileRb.constraints = isEnabled ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
        }
    }
}

