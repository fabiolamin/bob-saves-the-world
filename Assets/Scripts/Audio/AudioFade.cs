using System.Collections;
using UnityEngine;

namespace BSTW.Audio
{
    public class AudioFade : MonoBehaviour
    {
        private float _startVolume;
        private Coroutine _fadeOutCoroutine;
        private Coroutine _fadeInCoroutine;

        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private float _fadeOutTime;
        [SerializeField] private float _fadeInTime;

        private void Awake()
        {
            _startVolume = _audioSource.volume;
        }

        public void StartFadeOut()
        {
            if (_fadeOutCoroutine != null)
                StopCoroutine(_fadeOutCoroutine);

            if (_fadeInCoroutine != null)
                StopCoroutine(_fadeInCoroutine);

            _fadeOutCoroutine = StartCoroutine(FadeOut());
        }

        public void StartFadeIn()
        {
            if (_fadeOutCoroutine != null)
                StopCoroutine(_fadeOutCoroutine);

            if (_fadeInCoroutine != null)
                StopCoroutine(_fadeInCoroutine);

            _fadeInCoroutine = StartCoroutine(FadeIn());
        }

        private IEnumerator FadeOut()
        {
            while (_audioSource.volume > 0)
            {
                _audioSource.volume -= _startVolume * Time.deltaTime / _fadeOutTime;

                yield return null;
            }

            _audioSource.Stop();
            _audioSource.volume = _startVolume;
        }

        private IEnumerator FadeIn()
        {
            _audioSource.volume = 0f;
            _audioSource.Play();

            while (_audioSource.volume < _startVolume)
            {
                _audioSource.volume += _startVolume * Time.deltaTime / _fadeInTime;

                yield return null;
            }

            _audioSource.volume = _startVolume;
        }
    }
}

