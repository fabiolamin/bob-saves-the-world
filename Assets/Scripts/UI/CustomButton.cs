using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BSTW.UI
{
    public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private TextMeshProUGUI _buttonTMP;
        private Vector3 _defaultSize;

        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _pointerOverColor;
        [SerializeField] private AudioClip _pointerOverAudio;
        [SerializeField] private AudioClip _pointerClickAudio;
        [SerializeField] private float _scaleMultiplier = 1.1f;
        [SerializeField] private AudioSource _audioSource;

        private void Awake()
        {
            _defaultSize = transform.localScale;
            _buttonTMP = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = _defaultSize * _scaleMultiplier;
            _buttonTMP.color = _pointerOverColor;

            if (_audioSource != null)
                _audioSource.PlayOneShot(_pointerOverAudio);
            else
                AudioSource.PlayClipAtPoint(_pointerOverAudio, Camera.main.transform.position, 1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _defaultSize;
            _buttonTMP.color = _defaultColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            transform.localScale = _defaultSize;
            _buttonTMP.color = _defaultColor;

            if (_audioSource != null)
                _audioSource.PlayOneShot(_pointerClickAudio);
            else
                AudioSource.PlayClipAtPoint(_pointerClickAudio, Camera.main.transform.position, 1f);
        }
    }

}
