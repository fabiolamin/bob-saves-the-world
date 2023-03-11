using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Utils
{
    public class LoopInterval : MonoBehaviour
    {
        [SerializeField] private float _minInterval;
        [SerializeField] private float _maxInterval;

        [SerializeField] private bool _random;

        [SerializeField] private UnityEvent _onIntervalEnded;

        private void Awake()
        {
            StartCoroutine(StartAudioInterval());
        }

        private IEnumerator StartAudioInterval()
        {
            while (true)
            {
                var interval = _random ? Random.Range(_minInterval, _maxInterval) : _maxInterval;

                yield return new WaitForSeconds(interval);

                _onIntervalEnded.Invoke();
            }
        }
    }
}

