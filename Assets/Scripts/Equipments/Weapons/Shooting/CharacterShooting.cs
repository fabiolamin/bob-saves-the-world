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

        [SerializeField] private UnityEvent _onShot;
        [SerializeField] private UnityEvent<float, float> _onCurrentWeaponAmmoUpdated;
        [SerializeField] private UnityEvent<Sprite> _onCurrentWeaponUpdated;
        [SerializeField] private UnityEvent _onCriticalAmmoStarted;
        [SerializeField] private UnityEvent _onCriticalAmmoFinished;
        [SerializeField] public UnityEvent _onCharacterHitTarget;
        [SerializeField] public UnityEvent _onTargetKilled;


        protected bool shootingTriggered = false;
        protected AudioSource shootingAudioSource => _shootingAudioSource;

        public bool IsShooting { get; protected set; } = false;
        public bool IsReadyToShoot { get; protected set; } = true;
        public Weapon CurrentWeapon { get; private set; }
        public List<Weapon> Weapons { get; private set; } = new List<Weapon>();

        private void Awake()
        {
            InstantiateWeapons();
        }

        protected virtual void InstantiateWeapons()
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

        protected void SetCurrentWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;

            if (_characterAnimator != null)
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
            return IsReadyToShoot && !IsShooting && shootingTriggered &&
            CurrentWeapon.CanShoot;
        }

        private IEnumerator GetReadyToShoot()
        {
            IsShooting = true;

            while (shootingTriggered)
            {
                if (CurrentWeapon.CurrentProjectile != null)
                {
                    Shoot();

                    yield return new WaitForSeconds(CurrentWeapon.WeaponData.ShootingInterval);
                }

                CurrentWeapon.SetProjectile();

                yield return null;
            }

            IsShooting = false;
        }

        protected virtual void Shoot()
        {
            CurrentWeapon.Shoot(GetShootingOrigin(), GetShootingDirection());
        }

        public virtual void TriggerShootingEffects()
        {
            _onShot?.Invoke();
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

            CheckWeaponProjectileLoading();

            PlayReloadSFX();
            CheckCriticalAmmo();

            if (!CurrentWeapon.CanShoot)
                StopShooting();
        }

        protected void ActivateCurrentWeapon(bool isActive)
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

        public virtual void OnCharacterHitTarget(bool isTargetAlive)
        {
            _onCharacterHitTarget?.Invoke();

            if (!isTargetAlive)
            {
                _onTargetKilled?.Invoke();
            }
        }

        public void CheckWeaponProjectileLoading()
        {
            if (CurrentWeapon == null) return;

            CurrentWeapon.CheckProjectileLoading();
        }

        protected abstract Vector3 GetShootingOrigin();
        protected abstract Vector3 GetShootingDirection();
    }
}

