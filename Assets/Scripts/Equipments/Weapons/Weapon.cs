using BSTW.Data.Equipments.Weapons;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Equipments.Weapons
{
    public class Weapon : MonoBehaviour
    {
        private int _currentAmmo;

        [SerializeField] private WeaponData _weaponData;
        [SerializeField] private UnityEvent _onShoot;

        public WeaponData WeaponData => _weaponData;
        public bool CanShoot { get { return _currentAmmo > 0; } }

        public event Action _onWeaponStop;

        public void Awake()
        {
            _currentAmmo = _weaponData.MaxAmmo;
        }

        public void Shoot()
        {
            if (!_weaponData.IsSelected) return;

            _onShoot?.Invoke();
            UpdateCurrentAmmo(-1);
        }

        public void UpdateCurrentAmmo(int amount)
        {
            _currentAmmo = Mathf.Clamp(_currentAmmo + amount, 0, _weaponData.MaxAmmo);

            if (_currentAmmo <= 0)
                _onWeaponStop?.Invoke();
        }
    }
}

