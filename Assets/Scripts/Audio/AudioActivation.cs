using UnityEngine;
using System.Linq;

namespace BSTW.Audio
{
    public class AudioActivation : MonoBehaviour
    {
        private AudioSource[] _audioSources;

        private void Awake()
        {
            _audioSources = GetComponentsInChildren<AudioSource>();
        }

        public void PauseAudio()
        {
            _audioSources.ToList().ForEach(a => a.Pause());
        }

        public void UnpauseAudio()
        {
            _audioSources.ToList().ForEach(a => a.UnPause());
        }
    }
}

