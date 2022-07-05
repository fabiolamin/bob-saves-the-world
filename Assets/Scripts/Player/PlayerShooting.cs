using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using BSTW.Equipments.Weapons;

namespace BSTW.Player
{
    [System.Serializable]
    public struct WeaponController
    {
        public Weapon Weapon;
        public Transform WeaponHandPosition;
    }

    public class PlayerShooting : MonoBehaviour
    {
        private List<Weapon> _weapons = new List<Weapon>();

        private Coroutine _shootingCoroutine;

        private bool _isHoldingShootingTrigger = false;
        private bool _hasSwitchedWeapon = false;

        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private WeaponController[] _weaponControllers;
        [SerializeField] private GameObject _aimImage;
        [SerializeField] private UnityEvent<bool> _onPlayerAim;
        [SerializeField] private UnityEvent<float, float> _onCurrentWeaponShoot;
        [SerializeField] private UnityEvent<Sprite> _onCurrentWeaponUpdated;

        public static bool IsAiming { get; private set; } = false;
        public static bool IsShooting { get; private set; } = false;
        public static bool IsReadyToShoot { get; private set; } = true;

        public Weapon CurrentWeapon { get; private set; }

        private void Start()
        {
            InstantiateWeapons();
        }

        public void OnAim(InputAction.CallbackContext value)
        {
            IsAiming = value.action.IsPressed();

            if (!IsReadyToShoot) return;

            _aimImage.SetActive(IsAiming || _isHoldingShootingTrigger);
            _onPlayerAim?.Invoke(IsAiming);
        }

        public void OnShoot(InputAction.CallbackContext value)
        {
            _isHoldingShootingTrigger = value.action.IsPressed();

            if (!IsReadyToShoot) return;

            CheckShooting();
        }

        public void CheckShooting()
        {
            _aimImage.SetActive(IsAiming || _isHoldingShootingTrigger);

            if (CanShoot())
            {
                if (_shootingCoroutine is not null)
                    StopCoroutine(_shootingCoroutine);

                _shootingCoroutine = StartCoroutine(GetReadyToShoot());
            }
        }

        private bool CanShoot()
        {
            return IsReadyToShoot && !IsShooting && _isHoldingShootingTrigger && CurrentWeapon.CanShoot;
        }

        public void OnSwitchWeapon(InputAction.CallbackContext value)
        {
            if (!IsReadyToShoot) return;

            if (!_hasSwitchedWeapon)
                _hasSwitchedWeapon = value.started;

            CheckWeaponSwitch();
        }

        public void CheckWeaponSwitch()
        {
            if (_hasSwitchedWeapon && !PlayerMovement.IsRolling)
                SwitchWeapon();
        }

        private void InstantiateWeapons()
        {
            foreach (var weaponController in _weaponControllers)
            {
                Weapon newWeapon = Instantiate(weaponController.Weapon);

                newWeapon.SetUpWeapon(weaponController);

                newWeapon.OnWeaponStop += StopShooting;

                newWeapon.gameObject.SetActive(weaponController.Weapon.WeaponData.IsSelected);

                if (newWeapon.WeaponData.IsSelected)
                    SetCurrentWeapon(newWeapon);

                _weapons.Add(newWeapon);
            }
        }

        public void StopShooting()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);

            IsShooting = false;
        }

        private void SetCurrentWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            _playerAnimator.runtimeAnimatorController = CurrentWeapon.WeaponData.AnimatorController;
            _onCurrentWeaponUpdated?.Invoke(CurrentWeapon.WeaponData.Icon);
            _onCurrentWeaponShoot?.Invoke(CurrentWeapon.WeaponData.CurrentAmmo, CurrentWeapon.WeaponData.MaxAmmo);
        }

        private IEnumerator GetReadyToShoot()
        {
            IsShooting = true;

            while (_isHoldingShootingTrigger)
            {
                Shoot();

                yield return new WaitForSeconds(CurrentWeapon.WeaponData.ShootingInterval);
            }

            IsShooting = false;
        }

        private void Shoot()
        {
            CurrentWeapon.Shoot(Camera.main.transform.position, Camera.main.transform.forward);
            _onCurrentWeaponShoot?.Invoke(CurrentWeapon.WeaponData.CurrentAmmo, CurrentWeapon.WeaponData.MaxAmmo);
        }

        private void SwitchWeapon()
        {
            var currentWeaponIndex = _weapons.IndexOf(CurrentWeapon);
            currentWeaponIndex++;
            currentWeaponIndex %= _weapons.Count;

            ActivateCurrentWeapon(false);
            SetCurrentWeapon(_weapons[currentWeaponIndex]);
            ActivateCurrentWeapon(true);

            CurrentWeapon.CheckProjectileLoading();

            if (!CurrentWeapon.CanShoot)
                StopShooting();

            _hasSwitchedWeapon = false;
        }

        private void ActivateCurrentWeapon(bool isActive)
        {
            CurrentWeapon.WeaponData.IsSelected = isActive;
            CurrentWeapon.gameObject.SetActive(isActive);
        }

        public void EnablePlayerShooting(bool isEnabled)
        {
            IsReadyToShoot = isEnabled;
            _aimImage.SetActive(false);
            _onPlayerAim?.Invoke(enabled && IsAiming);
        }
    }
}

