using BSTW.Data.Equipments.Weapons;
using BSTW.Player;
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
        [SerializeField] private Transform _projectileOrigin;

        protected Projectile currentProjectile;
        protected Queue<Projectile> projectiles = new Queue<Projectile>();

        public WeaponData WeaponData => _weaponData;
        public bool CanShoot { get { return _weaponData.CurrentAmmo > 0; } }

        public event Action OnWeaponStop;

        public void SetUpWeapon(WeaponController weaponController)
        {
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

                projectile.EnablePhysics(false);
                projectile.transform.SetParent(_projectileOrigin);
                projectile.transform.position = _projectileOrigin.position;
                projectile.transform.rotation = _projectileOrigin.localRotation;
            }

            projectiles.ToList().ForEach(p => p.gameObject.SetActive(false));
        }

        public void Shoot(Vector3 origin, Vector3 direction)
        {
            if (!_weaponData.IsSelected) return;

            RaycastHit target;

            if (Physics.Raycast(origin, direction, out target, _weaponData.ShootingDistance))
            {
                _onShoot?.Invoke();
                ShootProjectile(target);
                UpdateCurrentAmmo(-1);
            }
        }

        protected virtual void SetProjectile()
        {
            if (projectiles.Count > 0)
            {
                currentProjectile = projectiles.Dequeue();
            }
        }

        private void ShootProjectile(RaycastHit target)
        {
            currentProjectile.gameObject.SetActive(true);
            currentProjectile.transform.SetParent(null);
            currentProjectile.EnablePhysics(true);
            currentProjectile.MoveTowards(_projectileOrigin.position, target.point);

            StartCoroutine(LoadProjectile());
        }

        protected virtual IEnumerator LoadProjectile()
        {
            yield return new WaitUntil(() => !currentProjectile.gameObject.activeSelf);

            SetProjectile();
        }

        private void UpdateCurrentAmmo(int amount)
        {
            _weaponData.CurrentAmmo = Mathf.Clamp(_weaponData.CurrentAmmo + amount, 0, _weaponData.MaxAmmo);

            if (_weaponData.CurrentAmmo <= 0)
                OnWeaponStop?.Invoke();
        }

        public virtual void LoadCurrentAmmo(int amount)
        {
            UpdateCurrentAmmo(amount);
            FillProjectilesQueue();
        }
    }
}

