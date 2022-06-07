using UnityEngine;
using UnityEngine.UI;

namespace BSTW.UI
{
    public class ImageSwitch : MonoBehaviour
    {
        [SerializeField] private Image _currentImage;

        public void Switch(Sprite sprite)
        {
            _currentImage.sprite = sprite;
        }
    }
}

