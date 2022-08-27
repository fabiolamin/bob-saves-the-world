using UnityEngine;

namespace BSTW.Audio
{
    public class FootStep : MonoBehaviour
    {
        private int _soundIndex = 0;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _sounds;

        private void OnTriggerEnter(Collider other)
        {
            PlayFootStepSFX();
        }

        private void PlayFootStepSFX()
        {
            _audioSource.clip = _sounds[_soundIndex];
            _audioSource.Play();

            _soundIndex++;
            _soundIndex %= _sounds.Length;
        }
    }
}

