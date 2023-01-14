using System.Collections;
using UnityEngine;

namespace BSTW.Utils
{
    public class TimeChanger : MonoBehaviour
    {
        private Coroutine _timeScaleCoroutine;

        [SerializeField] private float _delay = 0.2f;
        [SerializeField] private float _minTimeScale = 0.2f;
        [SerializeField] private float _duration = 1f;

        public void ChangeTimeScale()
        {
            if (_timeScaleCoroutine != null)
                StopCoroutine(_timeScaleCoroutine);

            _timeScaleCoroutine = StartCoroutine(ChangeTimeScaleCoroutine());
        }

        private IEnumerator ChangeTimeScaleCoroutine()
        {
            yield return new WaitForSeconds(_delay);

            Time.timeScale = _minTimeScale;

            yield return new WaitForSeconds(_duration);

            Time.timeScale = 1f;
        }
    }
}

