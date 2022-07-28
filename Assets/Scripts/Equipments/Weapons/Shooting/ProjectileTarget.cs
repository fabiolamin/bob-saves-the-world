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

        [SerializeField] private UnityEvent<float> _onHit;

        protected UnityEvent<float> OnHit => _onHit;

        public Rigidbody ProjectileTargetRb => _projectileTargetRb;

        public virtual void Hit(float damage, GameObject vfx, Vector3 point)
        {
            _onHit?.Invoke(damage);

            PlayHitEffects(vfx, point);
        }

        protected void PlayHitEffects(GameObject vfx, Vector3 point)
        {
            PlayHitVFX(vfx, point);
            PlayHitSFX(point);
        }

        private void PlayHitVFX(GameObject vfx, Vector3 point)
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


