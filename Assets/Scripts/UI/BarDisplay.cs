using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BSTW.UI
{
    public class BarDisplay : MonoBehaviour
    {
        private Coroutine _updateBarSmoothlyCoroutine;

        public Image Bar;

        [SerializeField] private float _smoothlySpeed = 5f;
        [SerializeField] private float _maxValue = 100f;

        public void UpdateBar(float currentValue, float maxValue)
        {
            if (!Bar.gameObject.activeSelf) return;

            Bar.fillAmount = currentValue / maxValue;
        }

        public void UpdateBarSmoothly(float currentValue, float maxValue)
        {
            if (!Bar.gameObject.activeSelf) return;

            if(_updateBarSmoothlyCoroutine is not null)
            {
                StopCoroutine(_updateBarSmoothlyCoroutine);
            }

            _updateBarSmoothlyCoroutine = StartCoroutine(StartUpdateBarSmoothly(Bar.fillAmount, currentValue/ _maxValue));
        }

        private IEnumerator StartUpdateBarSmoothly(float startValue, float endValue)
        {
            float elapsedTime = 0;

            while (elapsedTime < _smoothlySpeed)
            {
                Bar.fillAmount = Mathf.Lerp(startValue, endValue, elapsedTime / _smoothlySpeed);
                elapsedTime += Time.deltaTime;

                yield return null;
            }
        }
    }
}

