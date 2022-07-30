using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using BSTW.Data.Player;

namespace BSTW.Player
{
    public class PlayerCameraShake : MonoBehaviour
    {
        private List<CinemachineBasicMultiChannelPerlin> _cameraNoises = new List<CinemachineBasicMultiChannelPerlin>();

        private float _startingIntensity;
        private float _shakeTimer;
        private float _shakeTimerTotal;

        [SerializeField] private CinemachineFreeLook _cinemachineCamera;
        [SerializeField] private int _rigsAmount = 3;

        [SerializeField] private float _maxIntensity = 10f;
        [SerializeField] private float _maxTimer = 3f;

        [Header("Shake based on distance")]
        [SerializeField] private float _intensityDistanceConstant = 50f;
        [SerializeField] private float _timerDistanceConstant = 25f;
        [SerializeField] private float _maxShakeDistance = 50f;

        private void Awake()
        {
            for (int i = 0; i < _rigsAmount; i++)
            {
                _cameraNoises.Add(_cinemachineCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>());
            }
        }

        private void Update()
        {
            ShakeCamera();
        }

        private void ShakeCamera()
        {
            if (_shakeTimer > 0f)
            {
                _shakeTimer -= Time.deltaTime;

                _cameraNoises.ForEach(n => n.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, (1 - (_shakeTimer / _shakeTimerTotal))));
            }
        }

        public void StartShakeCamera(float intensity, float timer)
        {
            var realIntensity = Mathf.Clamp(intensity, 0f, _maxIntensity);
            var realTimer = Mathf.Clamp(timer, 0f, _maxTimer);

            _cameraNoises.ForEach(n => n.m_AmplitudeGain = realIntensity);
            _startingIntensity = realIntensity;
            _shakeTimerTotal = realTimer;
            _shakeTimer = realTimer;
        }

        public void StartShakeCamera(CameraShakeData data)
        {
            StartShakeCamera(data.Intensity, data.Timer);
        }

        public void CalculateShakeBasedOnDistance(Vector3 origin)
        {
            var distance = Vector3.Distance(origin, transform.position);

            if (distance > _maxShakeDistance) return;

            var intensity = _intensityDistanceConstant * (1 / distance);
            var timer = _timerDistanceConstant * (1 / distance);

            StartShakeCamera(intensity, timer);
        }
    }
}

