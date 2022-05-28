using BSTW.Data.Equipments;
using UnityEngine;

namespace BSTW.Equipments
{
    public class JetBackpack : MonoBehaviour
    {
        private float _rechargeDurationAux = 0f;

        private bool _canBeFueled = false;

        [SerializeField] private JetBackpackData _jetBackpackData;
        [SerializeField] private JetBackpackUser _jetBackpackUser;

        private void Awake()
        {
            _jetBackpackData.CurrentFuelAmount = _jetBackpackData.MaxFuelAmount;
        }

        private void Update()
        {
            UpdateJetBackpackFuel();
        }

        private void UpdateJetBackpackFuel()
        {
            if (_jetBackpackUser.IsFlying)
            {
                _jetBackpackData.CurrentFuelAmount = Mathf.Clamp(
                _jetBackpackData.CurrentFuelAmount - Time.deltaTime * _jetBackpackData.FuelDecreaseSpeed,
                0f,
                _jetBackpackData.MaxFuelAmount);
            }
            else
            {
                if (_canBeFueled)
                    _jetBackpackData.CurrentFuelAmount = Mathf.Clamp(
                    _jetBackpackData.CurrentFuelAmount + Time.deltaTime * _jetBackpackData.FuelIncreaseSpeed,
                    0f,
                    _jetBackpackData.MaxFuelAmount);
            }

            CheckJatBackpackRecharge();

            _jetBackpackUser.HasFuel = _jetBackpackData.CurrentFuelAmount > 0f;
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
    }
}

