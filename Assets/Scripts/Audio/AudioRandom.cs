using UnityEngine;

namespace BSTW.Audio
{
    public class AudioRandom : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _sounds;

        public void PlayRandomSound()
        {
            _audioSource.clip = _sounds[Random.Range(0, _sounds.Length)];
            _audioSource.Play();
        }
    }
}

