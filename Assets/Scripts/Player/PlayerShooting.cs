using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private WeaponController[] _weaponControllers;
        [SerializeField] private GameObject _aimImage;
        [SerializeField] private UnityEvent<bool> _onPlayerAim;

        public static bool IsAiming { get; private set; } = false;
        public static bool IsShooting { get; private set; } = false;
        public Weapon CurrentWeapon { get; private set; }

        private void Awake()
        {
            InstantiateWeapons();
        }

        public void OnAim(InputAction.CallbackContext value)
        {
            IsAiming = value.action.IsPressed();
            _aimImage.SetActive(IsAiming || _isHoldingShootingTrigger);
            _onPlayerAim?.Invoke(IsAiming);
        }

        public void OnShoot(InputAction.CallbackContext value)
        {
            _isHoldingShootingTrigger = value.action.IsPressed();

            _aimImage.SetActive(IsAiming || _isHoldingShootingTrigger);

            if (!IsShooting && _isHoldingShootingTrigger && CurrentWeapon.CanShoot)
                _shootingCoroutine = StartCoroutine(GetReadyToShoot());
        }

        public void OnSwitchWeapon(InputAction.CallbackContext value)
        {
            if (value.started && !IsShooting)
                SwitchWeapon();
        }

        private void InstantiateWeapons()
        {
            foreach (var weaponController in _weaponControllers)
            {
                Weapon newWeapon = Instantiate(weaponController.Weapon);

                newWeapon.transform.position = weaponController.WeaponHandPosition.position;
                newWeapon.transform.localRotation = weaponController.WeaponHandPosition.rotation;
                newWeapon.transform.SetParent(weaponController.WeaponHandPosition);

                newWeapon._onWeaponStop += StopShooting;

                newWeapon.gameObject.SetActive(weaponController.Weapon.WeaponData.IsSelected);

                if (newWeapon.WeaponData.IsSelected)
                    SetCurrentWeapon(newWeapon);

                _weapons.Add(newWeapon);
            }
        }

        public void StopShooting()
        {
            StopCoroutine(_shootingCoroutine);
            IsShooting = false;
        }

        private void SetCurrentWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            _playerAnimator.runtimeAnimatorController = CurrentWeapon.WeaponData.AnimatorController;
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
            RaycastHit hit;

            CurrentWeapon.Shoot();

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, CurrentWeapon.WeaponData.ShootingDistance))
            {
                HitTarget(hit);
            }
        }

        private void HitTarget(RaycastHit hit)
        {
            var bulletTarget = hit.collider.GetComponent<BulletTarget>();
            if (bulletTarget is null) return;

            bulletTarget.Hit(CurrentWeapon.WeaponData.BulletDamage, hit.point);
        }

        private void SwitchWeapon()
        {
            var currentWeaponIndex = _weapons.IndexOf(CurrentWeapon);
            currentWeaponIndex++;
            currentWeaponIndex %= _weapons.Count;

            ActivateCurrentWeapon(false);
            SetCurrentWeapon(_weapons[currentWeaponIndex]);
            ActivateCurrentWeapon(true);
        }

        private void ActivateCurrentWeapon(bool isActive)
        {
            CurrentWeapon.WeaponData.IsSelected = isActive;
            CurrentWeapon.gameObject.SetActive(isActive);
        }
    }
}

