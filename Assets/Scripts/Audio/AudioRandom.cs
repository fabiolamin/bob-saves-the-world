using UnityEngine;

namespace BSTW.Audio
{
    public class AudioRandom : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _sounds;
        [SerializeField] private bool _hasInterval = false;

        public void PlayRandomSound()
        {
            if (_audioSource.isPlaying && _hasInterval) return;

            _audioSource.clip = _sounds[Random.Range(0, _sounds.Length)];
            _audioSource.Play();
        }
    }
}

