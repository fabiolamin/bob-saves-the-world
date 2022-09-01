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
        private Coroutine _shootingCoroutine;

        [SerializeField] private Animator _characterAnimator;
        [SerializeField] private AudioSource _shootingAudioSource;
        [SerializeField] private WeaponController[] _weaponControllers;
        [SerializeField] private UnityEvent<float, float> _onCurrentWeaponAmmoUpdated;
        [SerializeField] private UnityEvent<Sprite> _onCurrentWeaponUpdated;
        [SerializeField] private UnityEvent _onCriticalAmmoStarted;
        [SerializeField] private UnityEvent _onCriticalAmmoFinished;

        protected bool isHoldingShootingTrigger = false;
        protected AudioSource shootingAudioSource => _shootingAudioSource;

        public bool IsShooting { get; protected set; } = false;
        public bool IsReadyToShoot { get; protected set; } = true;
        public Weapon CurrentWeapon { get; private set; }
        public List<Weapon> Weapons { get; private set; } = new List<Weapon>();

        private void Awake()
        {
            InstantiateWeapons();
        }

        private void InstantiateWeapons()
        {
            foreach (var weaponController in _weaponControllers)
            {
                Weapon newWeapon = Instantiate(weaponController.Weapon);

                newWeapon.SetUpWeapon(this, weaponController);

                newWeapon.OnWeaponStop += StopShooting;

                newWeapon.gameObject.SetActive(weaponController.Weapon.WeaponData.IsSelected);

                if (newWeapon.WeaponData.IsSelected)
                    SetCurrentWeapon(newWeapon);

                Weapons.Add(newWeapon);
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
            OnWeaponAmmoUpdated();
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

            PlayShootSFX();
        }

        private void CheckCriticalAmmo()
        {
            if (CurrentWeapon.WeaponData.CurrentAmmo <= (CurrentWeapon.WeaponData.MaxAmmo * CurrentWeapon.WeaponData.CriticalAmmoPercentage))
                _onCriticalAmmoStarted?.Invoke();
            else
                _onCriticalAmmoFinished?.Invoke();
        }

        public void SwitchWeapon(int deltaPos)
        {
            if (IsShooting) return;

            var currentWeaponIndex = Weapons.IndexOf(CurrentWeapon);
            currentWeaponIndex += deltaPos;

            if (currentWeaponIndex > Weapons.Count - 1)
                currentWeaponIndex = 0;
            else if (currentWeaponIndex < 0)
                currentWeaponIndex = Weapons.Count - 1;

            ActivateCurrentWeapon(false);
            SetCurrentWeapon(Weapons[currentWeaponIndex]);
            ActivateCurrentWeapon(true);

            CurrentWeapon.CheckProjectileLoading();

            PlayReloadSFX();
            CheckCriticalAmmo();

            if (!CurrentWeapon.CanShoot)
                StopShooting();
        }

        private void ActivateCurrentWeapon(bool isActive)
        {
            CurrentWeapon.WeaponData.IsSelected = isActive;
            CurrentWeapon.gameObject.SetActive(isActive);
        }

        public void PlayShootSFX()
        {
            if (CurrentWeapon != null && CurrentWeapon.WeaponData.ShootSFX != null)
            {
                _shootingAudioSource.PlayOneShot(CurrentWeapon.WeaponData.ShootSFX);
            }
        }

        public void PlayReloadSFX()
        {
            if (CurrentWeapon != null && CurrentWeapon.WeaponData.ReloadSFX != null && CurrentWeapon.WeaponData.IsSelected)
            {
                _shootingAudioSource.Stop();
                _shootingAudioSource.PlayOneShot(CurrentWeapon.WeaponData.ReloadSFX);
            }
        }

        public void OnWeaponAmmoUpdated()
        {
            _onCurrentWeaponAmmoUpdated?.Invoke(CurrentWeapon.WeaponData.CurrentAmmo, CurrentWeapon.WeaponData.MaxAmmo);

            CheckCriticalAmmo();
        }

        protected abstract Vector3 GetShootingOrigin();
        protected abstract Vector3 GetShootingDirection();
    }
}

