using BSTW.Data.Equipments.Weapons;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Equipments.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponData _weaponData;
        [SerializeField] private UnityEvent _onShoot;

        public WeaponData WeaponData => _weaponData;

        public void Shoot()
        {
            _onShoot?.Invoke();
        }
    }
}

