using BSTW.Data.Equipments;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Equipments
{
    public class JetBackpack : MonoBehaviour
    {
        private float _rechargeDurationAux = 0f;

        private bool _canBeFueled = true;

        [SerializeField] private JetBackpackData _jetBackpackData;
        [SerializeField] private JetBackpackUser _jetBackpackUser;
        [SerializeField] private ParticleSystem _jetBackpackVFX;
        [SerializeField] private UnityEvent<float, float> _onJetBackpackFuelUpdated;
        [SerializeField] private UnityEvent _onJetBackpackActivated;
        [SerializeField] private UnityEvent _onJetBackpackDeactivate;
        [SerializeField] private UnityEvent _onFuelObtained;

        public JetBackpackData JetBackpackData => _jetBackpackData;

        private void Awake()
        {
            _jetBackpackData.CurrentFuelAmount = _jetBackpackData.MaxFuelAmount;
        }

        private void Update()
        {
            CheckJetBackpackFuel();
        }

        private void CheckJetBackpackFuel()
        {
            if (_jetBackpackUser.IsFlying)
            {
                SetFuelSpeed();
            }
            else
            {
                if (_canBeFueled)
                {
                    SetFuelSpeed(false);
                }
            }

            CheckJatBackpackRecharge();

            _jetBackpackUser.HasFuel = _jetBackpackData.CurrentFuelAmount > 0f;

            ActivateJetBackpackFlame(_jetBackpackUser.HasFuel && _jetBackpackUser.IsFlying);
        }

        private void SetFuelSpeed(bool isFlying = true)
        {
            var modifier = -1;
            var speed = _jetBackpackData.FuelDecreaseSpeed;

            if (!isFlying)
            {
                modifier = 1;
                speed = _jetBackpackData.FuelIncreaseSpeed;
            }

            UpdateFuel(Time.deltaTime * speed * modifier);
        }

        private void CheckJatBackpackRecharge()
        {
            if (_jetBackpackData.CurrentFuelAmount <= 0f)
            {
                _canBeFueled = false;
                _rechargeDurationAux += Time.deltaTime;

                if (_rechargeDurationAux >= _jetBackpackData.RechargeDuration)
                {
                    _canBeFueled = true;
                    _rechargeDurationAux = 0f;
                }
            }
        }

        private void ActivateJetBackpackFlame(bool isActive)
        {
            if (_jetBackpackVFX.isPlaying == isActive) return;

            if (isActive)
                _onJetBackpackActivated.Invoke();
            else
                _onJetBackpackDeactivate.Invoke();
        }

        private void UpdateFuel(float amount)
        {
            _jetBackpackData.UpdateFuel(amount);

            _onJetBackpackFuelUpdated?.Invoke(_jetBackpackData.CurrentFuelAmount, _jetBackpackData.MaxFuelAmount);
        }

        public void GetFuel(float amount)
        {
            _canBeFueled = true;
            _onFuelObtained?.Invoke();
            UpdateFuel(amount);
        }
    }
}

