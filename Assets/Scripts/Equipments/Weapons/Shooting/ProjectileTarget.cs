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

        [SerializeField] private ObjectPooling _audioSourcePooling;

        [SerializeField] private AudioClip[] _audioHitClips;

        [SerializeField] private UnityEvent<Hit> _onHit;

        protected UnityEvent<Hit> OnHit => _onHit;

        public Rigidbody ProjectileTargetRb => _projectileTargetRb;

        public virtual void Hit(Hit hit, GameObject vfx, Vector3 point, Projectile projectile)
        {
            _onHit?.Invoke(hit);

            PlayHitEffects(vfx, point, projectile);
        }

        protected void PlayHitEffects(GameObject vfx, Vector3 point, Projectile projectile)
        {
            PlayHitVFX(vfx, point, projectile);
            PlayHitSFX(point);
        }

        private void PlayHitVFX(GameObject vfx, Vector3 point, Projectile projectile)
        {
            var vfxGO = vfx == null ? _hitVFXPooling.GetObject() : vfx;

            vfxGO.transform.position = point;
            if (projectile != null)
                vfxGO.transform.forward += -projectile.transform.forward;

            var particles = vfxGO.GetComponent<ParticleSystem>();
            particles.Play();

            StartCoroutine(DisableHitVFX(particles));
        }

        private IEnumerator DisableHitVFX(ParticleSystem particles)
        {
            yield return new WaitUntil(() => particles.isStopped);

            particles.gameObject.SetActive(false);
        }

        private void PlayHitSFX(Vector3 point)
        {
            if (_audioSourcePooling == null || _audioHitClips.Length == 0) return;

            var randomSFX = _audioHitClips[Random.Range(0, _audioHitClips.Length)];

            var audioSource = _audioSourcePooling.GetObject().GetComponent<AudioSource>();
            audioSource.transform.position = point;
            audioSource.PlayOneShot(randomSFX);

            StartCoroutine(DisableSFX(audioSource));
        }

        private IEnumerator DisableSFX(AudioSource audioSource)
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);

            audioSource.gameObject.SetActive(false);
        }
    }
}


