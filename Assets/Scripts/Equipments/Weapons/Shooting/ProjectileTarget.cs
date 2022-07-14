using BSTW.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class ProjectileTarget : MonoBehaviour
    {
        [SerializeField] private Rigidbody _projectileTargetRb = null;
        [SerializeField] private ObjectPooling _hitVFXPooling;
        [SerializeField] private UnityEvent<float> _onHit;

        protected UnityEvent<float> OnHit => _onHit;

        public Rigidbody ProjectileTargetRb => _projectileTargetRb;

        public virtual void Hit(float damage, GameObject vfx, Vector3 point)
        {
            _onHit?.Invoke(damage);

            PlayHitVFXOnPoint(vfx, point);
        }

        protected void PlayHitVFXOnPoint(GameObject vfx, Vector3 point)
        {
            var vfxGO = vfx == null ? _hitVFXPooling.GetObject() : vfx;
            vfxGO.transform.position = point;

            var particles = vfxGO.GetComponent<ParticleSystem>();
            particles.Play();

            StartCoroutine(DisableHitVFX(particles));
        }

        private IEnumerator DisableHitVFX(ParticleSystem particles)
        {
            yield return new WaitUntil(() => particles.isStopped);

            particles.gameObject.SetActive(false);
        }
    }
}

