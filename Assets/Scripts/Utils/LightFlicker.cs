using System.Collections;
using UnityEngine;

namespace BSTW.Utils
{
    public class LightFlicker : MonoBehaviour
    {
        private Coroutine _flickCoroutine;

        [SerializeField] private Light _light;
        [SerializeField] private float _time;

        private void Awake()
        {
            _light.enabled = false;
        }

        public void StartFlick()
        {
            if (_flickCoroutine != null)
            {
                StopCoroutine(StartFlickCoroutine());
            }

            _flickCoroutine = StartCoroutine(StartFlickCoroutine());
        }

        private IEnumerator StartFlickCoroutine()
        {
            _light.enabled = true;

            yield return new WaitForSeconds(_time);

            _light.enabled = false;
        }
    }
}

