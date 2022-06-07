using UnityEngine;
using TMPro;

namespace BSTW.UI
{
    public class ValueLimitDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueText;
        public void UpdateLimit(float currentValue, float maxValue)
        {
            _valueText.text = string.Format("{0}/{1}", currentValue, maxValue);
        }
    }
}

