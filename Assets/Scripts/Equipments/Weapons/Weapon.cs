using BSTW.Data.Equipments.Weapons;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Equipments.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponData _weaponData;
        [SerializeField] private UnityEvent _onShoot;

        public WeaponData WeaponData => _weaponData;
        public bool CanShoot { get { return _weaponData.CurrentAmmo > 0; } }

        public event Action _onWeaponStop;

        public void Awake()
        {
            _weaponData.CurrentAmmo = _weaponData.MaxAmmo;
        }

        public void Shoot()
        {
            if (!_weaponData.IsSelected) return;

            _onShoot?.Invoke();
            UpdateCurrentAmmo(-1);
        }

        public void UpdateCurrentAmmo(int amount)
        {
            _weaponData.CurrentAmmo = Mathf.Clamp(_weaponData.CurrentAmmo + amount, 0, _weaponData.MaxAmmo);

            if (_weaponData.CurrentAmmo <= 0)
                _onWeaponStop?.Invoke();
        }
    }
}

