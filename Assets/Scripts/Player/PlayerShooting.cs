using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using BSTW.Equipments.Weapons.Shooting;

namespace BSTW.Player
{
    public class PlayerShooting : CharacterShooting
    {
        [SerializeField] private GameObject _aimImage;
        [SerializeField] private UnityEvent<bool> _onPlayerAim;

        public bool IsAiming { get; private set; } = false;

        public void OnAim(InputAction.CallbackContext value)
        {
            IsAiming = value.action.IsPressed();

            if (!IsReadyToShoot) return;

            _aimImage.SetActive(IsAiming || isHoldingShootingTrigger);
            _onPlayerAim?.Invoke(IsAiming);
        }

        public void OnShoot(InputAction.CallbackContext value)
        {
            isHoldingShootingTrigger = value.action.IsPressed();

            if (!IsReadyToShoot) return;

            CheckShooting();
        }

        public void OnSwitchWeapon(InputAction.CallbackContext value)
        {
            if (!IsReadyToShoot) return;

            if (!hasSwitchedWeapon)
                hasSwitchedWeapon = value.started;

            CheckWeaponSwitch();
        }

        protected override void CheckShooting()
        {
            _aimImage.SetActive(IsAiming || isHoldingShootingTrigger);

            base.CheckShooting();
        }

        private void CheckWeaponSwitch()
        {
            if (hasSwitchedWeapon && !PlayerMovement.IsRolling)
                SwitchWeapon();
        }

        public void EnablePlayerShooting(bool isEnabled)
        {
            IsReadyToShoot = isEnabled;
            _aimImage.SetActive(false);
            _onPlayerAim?.Invoke(enabled && IsAiming);
        }

        protected override Vector3 GetShootingOrigin()
        {
            return Camera.main.transform.position;
        }

        protected override Vector3 GetShootingDirection()
        {
            return Camera.main.transform.forward;
        }
    }
}

