using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Environment
{
    public class ExplodingProp : MonoBehaviour
    {
        private Coroutine _explodeOnCriticalCoroutine;

        [SerializeField] private Explosion _explosion;
        [SerializeField] private ParticleSystem _explosionVFX;
        [SerializeField] private AudioSource _propAudioSource;
        [SerializeField] private Vector3 _explosionVFXScale;
        [SerializeField] private UnityEvent _onExplosion;

        [SerializeField] private float _timeToExplodeOnCritical = 2f;
        [SerializeField] private float _damage = 30f;
        [SerializeField] private float _radius = 5f;
        [SerializeField] private float _force = 50f;
        [SerializeField] private float _upwardsForce = 50f;

        public void Explode()
        {
            var vfx = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
            vfx.transform.localScale = _explosionVFXScale;
            vfx.Play();

            _onExplosion?.Invoke();
            _explosion.SetExplosion(_damage, _radius, _force, _upwardsForce);

            if (_explodeOnCriticalCoroutine != null)
                StopCoroutine(_explodeOnCriticalCoroutine);

            _propAudioSource.Stop();
        }

        public void SetTimeToExplodeOnCritical()
        {
            if (_explodeOnCriticalCoroutine != null) return;

            _explodeOnCriticalCoroutine = StartCoroutine(ExplodeOnCritical());

            if (!_propAudioSource.isPlaying)
                _propAudioSource.Play();
        }

        private IEnumerator ExplodeOnCritical()
        {
            yield return new WaitForSeconds(_timeToExplodeOnCritical);

            Explode();
        }
    }
}

