using UnityEngine;

namespace BSTW.Data.Equipments
{
    [CreateAssetMenu(fileName = "new JetBackpack Data", menuName = "Data/Equipments/new JetBackpack Data")]
    public class JetBackpackData : ScriptableObject
    {
        [SerializeField] private float _maxFuelAmount = 5f;
        [SerializeField] private float _fuelIncreaseSpeed = 5f;
        [SerializeField] private float _fuelDecreaseSpeed = 5f;
        [SerializeField] private float _rechargeDuration = 2f;

        public float CurrentFuelAmount { get; set; }
        public float MaxFuelAmount => _maxFuelAmount;
        public float FuelIncreaseSpeed => _fuelIncreaseSpeed;
        public float FuelDecreaseSpeed => _fuelDecreaseSpeed;
        public float RechargeDuration => _rechargeDuration;
    }
}

