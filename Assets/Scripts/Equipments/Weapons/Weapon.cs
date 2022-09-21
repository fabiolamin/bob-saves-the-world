using BSTW.Data.Equipments.Weapons;
using BSTW.Equipments.Weapons.Shooting;
using BSTW.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Equipments.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponData _weaponData;
        [SerializeField] private UnityEvent _onShoot;

        [SerializeField] private ObjectPooling _projectilePooling;
        [SerializeField] private ObjectPooling _bulletShellPooling;
        [SerializeField] private Transform _projectileOrigin;
        [SerializeField] private Transform _bulletShellOrigin;

        [SerializeField] private bool _infiniteAmmo = false;

        protected CharacterShooting characterShooting;
        protected Queue<Projectile> projectiles = new Queue<Projectile>();
        protected bool isProjectileLoaded = true;
        protected Coroutine projectileLoadingCoroutine;

        public WeaponData WeaponData => _weaponData;
        public bool CanShoot { get { return _weaponData.CurrentAmmo > 0 || _infiniteAmmo; } }

        public Projectile CurrentProjectile { get; protected set; }

        public event Action OnWeaponStop;

        public void SetUpWeapon(CharacterShooting characterShooting, WeaponController weaponController)
        {
            this.characterShooting = characterShooting;

            _weaponData.CurrentAmmo = _weaponData.MaxAmmo;

            transform.position = weaponController.WeaponHandPosition.position;
            transform.localRotation = weaponController.WeaponHandPosition.rotation;
            transform.SetParent(weaponController.WeaponHandPosition);

            FillProjectilesQueue();
            SetProjectile();
        }

        private void FillProjectilesQueue()
        {
            for (int i = 0; i < WeaponData.CurrentAmmo; i++)
            {
                var projectile = _projectilePooling.GetObject().GetComponent<Projectile>();

                projectiles.Enqueue(projectile);
                projectile.SetUpProjectile(gameObject.layer, WeaponData.Targets, WeaponData.HitDamage, _projectileOrigin);
            }

            projectiles.ToList().ForEach(p => p.gameObject.SetActive(false));
        }

        public void Shoot(Vector3 origin, Vector3 direction)
        {
            if (!_weaponData.IsSelected || !isProjectileLoaded) return;

            RaycastHit target;

            if (Physics.Raycast(origin, direction, out target, _weaponData.ShootingDistance, LayerMask.GetMask(_weaponData.Targets)))
            {
                _onShoot?.Invoke();
                CurrentProjectile.GetReadyToMove(_projectileOrigin.position, target.point);
                CurrentProjectile = null;
                isProjectileLoaded = false;
                UpdateCurrentAmmo(-1);

                if (_bulletShellPooling != null && _bulletShellOrigin != null)
                {
                    var bulletShell = _bulletShellPooling.GetObject().GetComponent<BulletShell>();
                    bulletShell.gameObject.SetActive(true);
                    bulletShell.Weapon = transform;
                    bulletShell.transform.position = _bulletShellOrigin.position;
                    bulletShell.MoveBulletShell();
                }
            }
        }

        public virtual void SetProjectile()
        {
            if (projectiles.Count > 0)
            {
                CurrentProjectile = projectiles.Dequeue();
                isProjectileLoaded = true;
            }
        }

        private void StartProjectileLoading()
        {
            if (projectileLoadingCoroutine != null)
            {
                StopCoroutine(projectileLoadingCoroutine);
            }

            projectileLoadingCoroutine = StartCoroutine(LoadProjectile());
        }

        protected virtual IEnumerator LoadProjectile()
        {
            isProjectileLoaded = false;

            yield return new WaitForSeconds(WeaponData.ShootingInterval);

            isProjectileLoaded = true;
            SetProjectile();
        }

        private void UpdateCurrentAmmo(int amount)
        {
            _weaponData.CurrentAmmo = Mathf.Clamp(_weaponData.CurrentAmmo + amount, 0, _weaponData.MaxAmmo);

            characterShooting.OnWeaponAmmoUpdated();

            if (_weaponData.CurrentAmmo <= 0)
                OnWeaponStop?.Invoke();
        }

        public virtual void LoadCurrentAmmo(int amount)
        {
            if (!_weaponData.CanLoadWeapon) return;

            UpdateCurrentAmmo(amount);
            FillProjectilesQueue();
        }

        public void CheckProjectileLoading()
        {
            if (isProjectileLoaded) return;

            StartProjectileLoading();
        }
    }
}

