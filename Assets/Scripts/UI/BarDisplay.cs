using UnityEngine;
using UnityEngine.UI;

namespace BSTW.UI
{
    public class BarDisplay : MonoBehaviour
    {
        public Image Bar;

        public void UpdateBar(float currentValue, float maxValue)
        {
            Bar.fillAmount = currentValue / maxValue;
        }
    }
}

