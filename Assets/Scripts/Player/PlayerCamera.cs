using Cinemachine;
using UnityEngine;

namespace BSTW.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        private float _defaultCameraFOV;
        private float _defaultCameraXSpeed;
        private float _defaultCameraYSpeed;

        [SerializeField] private CinemachineFreeLook _cinemachineCamera;
        [SerializeField] private float _cameraFOVOnAiming = 30f;
        [SerializeField] private float _switchCameraFOVSpeed = 10f;
        [SerializeField] private float _cameraXSpeedOnAiming = 2f;
        [SerializeField] private float _cameraYSpeedOnAiming = 2f;

        private void Awake()
        {
            _defaultCameraFOV = _cinemachineCamera.m_Lens.FieldOfView;
            _defaultCameraXSpeed = _cinemachineCamera.m_XAxis.m_MaxSpeed;
            _defaultCameraYSpeed = _cinemachineCamera.m_YAxis.m_MaxSpeed;
        }

        private void Update()
        {
            UpdateCameraFOVOnAiming();
        }

        public void UpdateCameraSpeedOnAiming(bool hasAimed)
        {
            _cinemachineCamera.m_XAxis.m_MaxSpeed = hasAimed ? _cameraXSpeedOnAiming : _defaultCameraXSpeed;
            _cinemachineCamera.m_YAxis.m_MaxSpeed = hasAimed ? _cameraYSpeedOnAiming : _defaultCameraYSpeed;
        }

        private void UpdateCameraFOVOnAiming()
        {
            var targetFOV = PlayerShooting.IsAiming && PlayerShooting.IsReadyToShoot ? _cameraFOVOnAiming : _defaultCameraFOV;

            _cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(
                _cinemachineCamera.m_Lens.FieldOfView,
                targetFOV,
                _switchCameraFOVSpeed * Time.deltaTime);
        }
    }
}

