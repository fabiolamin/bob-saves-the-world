using UnityEngine;

namespace BSTW.Data.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "new Weapon Data", menuName = "Data/Equipments/new Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        private int _currentAmmo;

        [SerializeField] private int _maxAmmo = 400;
        [SerializeField] private float _bulletDamage = 20f;
        [SerializeField] private float _shootingInterval = 0.5f;
        [SerializeField] private float _shootingDistance = 500f;
        [SerializeField] private bool _isSelected = false;

        public int CurrentAmmo => _currentAmmo;
        public int MaxAmmo => _maxAmmo;
        public float BulletDamage => _bulletDamage;
        public float ShootingInterval => _shootingInterval;

        public float ShootingDistance => _shootingDistance;
        public bool IsSelected => _isSelected;
    }
}

