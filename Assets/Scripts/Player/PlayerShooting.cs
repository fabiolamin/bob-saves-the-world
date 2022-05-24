using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace BSTW.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        private float _defaultCameraFOV;

        [SerializeField] private CinemachineFreeLook _cinemachineCamera;
        [SerializeField] private float _cameraFOVWhenAiming = 30f;
        [SerializeField] private float _switchCameraFOVSpeed = 10f;
        [SerializeField] private GameObject _aimImage;

        public static bool IsAiming { get; private set; } = false;

        private void Awake()
        {
            _defaultCameraFOV = _cinemachineCamera.m_Lens.FieldOfView;
        }

        private void Update()
        {
            UpdateCameraFOVOnAiming();
        }

        public void OnAim(InputAction.CallbackContext value)
        {
            IsAiming = value.action.IsPressed();
            _aimImage.SetActive(IsAiming);
        }

        private void UpdateCameraFOVOnAiming()
        {
            var targetFOV = IsAiming ? _cameraFOVWhenAiming : _defaultCameraFOV;

            _cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(
                _cinemachineCamera.m_Lens.FieldOfView,
                targetFOV,
                _switchCameraFOVSpeed * Time.deltaTime);
        }
    }
}

