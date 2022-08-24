using BSTW.Equipments.Weapons;
using BSTW.Player;
using UnityEngine;
using System.Linq;
using BSTW.Data.Equipments.Weapons;

namespace BSTW.Environment.Items
{
    public class AmmoItem : Item
    {
        private PlayerShooting _playerShooting;
        private Weapon _characterSelectedWeapon;

        [SerializeField] private WeaponData _weaponData;

        private void Start()
        {
            _playerShooting = collector.GetComponent<PlayerShooting>();

            _characterSelectedWeapon = _playerShooting.Weapons.FirstOrDefault(w => w.WeaponData == _weaponData);
        }

        protected override bool CanBeCollected()
        {
            return _characterSelectedWeapon.WeaponData.CanLoadWeapon;
        }

        protected override void OnCollected()
        {
            AudioSource.PlayClipAtPoint(_weaponData.ReloadSFX, collector.transform.position, 1f);
            _characterSelectedWeapon.LoadCurrentAmmo((int)bonusAmount);
        }
    }
}

