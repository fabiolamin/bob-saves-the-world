using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Equipments.Weapons.Shooting
{
    [System.Serializable]
    public struct WeaponController
    {
        public Weapon Weapon;
        public Transform WeaponHandPosition;
    }

    public abstract class CharacterShooting : MonoBehaviour
    {
        private List<Weapon> _weapons = new List<Weapon>();

        private Coroutine _shootingCoroutine;

        [SerializeField] private Animator _characterAnimator;
        [SerializeField] private WeaponController[] _weaponControllers;
        [SerializeField] private UnityEvent<float, float> _onCurrentWeaponShoot;
        [SerializeField] private UnityEvent<Sprite> _onCurrentWeaponUpdated;

        protected bool isHoldingShootingTrigger = false;

        public bool IsShooting { get; protected set; } = false;
        public bool IsReadyToShoot { get; protected set; } = true;
        public Weapon CurrentWeapon { get; private set; }

        private void Start()
        {
            InstantiateWeapons();
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
            _characterAnimator.runtimeAnimatorController = CurrentWeapon.WeaponData.AnimatorController;
            _onCurrentWeaponUpdated?.Invoke(CurrentWeapon.WeaponData.Icon);
            _onCurrentWeaponShoot?.Invoke(CurrentWeapon.WeaponData.CurrentAmmo, CurrentWeapon.WeaponData.MaxAmmo);
        }

        protected virtual void CheckShooting()
        {
            if (CanShoot())
            {
                if (_shootingCoroutine is not null)
                    StopCoroutine(_shootingCoroutine);

                _shootingCoroutine = StartCoroutine(GetReadyToShoot());
            }
        }

        public bool CanShoot()
        {
            return IsReadyToShoot && !IsShooting && isHoldingShootingTrigger && CurrentWeapon.CanShoot;
        }

        private IEnumerator GetReadyToShoot()
        {
            IsShooting = true;

            while (isHoldingShootingTrigger)
            {
                Shoot();

                yield return new WaitForSeconds(CurrentWeapon.WeaponData.ShootingInterval);

                if (CurrentWeapon.CurrentProjectile == null)
                    CurrentWeapon.SetProjectile();
            }

            IsShooting = false;
        }

        protected virtual void Shoot()
        {
            CurrentWeapon.Shoot(GetShootingOrigin(), GetShootingDirection());
            _onCurrentWeaponShoot?.Invoke(CurrentWeapon.WeaponData.CurrentAmmo, CurrentWeapon.WeaponData.MaxAmmo);
        }

        public void SwitchWeapon(int deltaPos)
        {
            if (IsShooting) return;

            var currentWeaponIndex = _weapons.IndexOf(CurrentWeapon);
            currentWeaponIndex += deltaPos;

            if (currentWeaponIndex > _weapons.Count - 1)
                currentWeaponIndex = 0;
            else if (currentWeaponIndex < 0)
                currentWeaponIndex = _weapons.Count - 1;

            ActivateCurrentWeapon(false);
            SetCurrentWeapon(_weapons[currentWeaponIndex]);
            ActivateCurrentWeapon(true);

            CurrentWeapon.CheckProjectileLoading();

            if (!CurrentWeapon.CanShoot)
                StopShooting();
        }

        private void ActivateCurrentWeapon(bool isActive)
        {
            CurrentWeapon.WeaponData.IsSelected = isActive;
            CurrentWeapon.gameObject.SetActive(isActive);
        }

        protected abstract Vector3 GetShootingOrigin();
        protected abstract Vector3 GetShootingDirection();
    }
}

