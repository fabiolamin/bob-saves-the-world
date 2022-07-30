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
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private PlayerCameraShake _playerCameraShake;

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
            if (value.started && IsReadyToShoot)
            {
                int deltaPos = (int)value.ReadValue<Vector2>().normalized.y;

                if (!PlayerMovement.IsRolling)
                    SwitchWeapon(deltaPos);
            }
        }

        protected override void CheckShooting()
        {
            _aimImage.SetActive(IsAiming || isHoldingShootingTrigger);

            base.CheckShooting();
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

        protected override void Shoot()
        {
            _playerCameraShake.StartShakeCamera(CurrentWeapon.WeaponData.CameraShakeData);

            base.Shoot();

            if (CurrentWeapon.WeaponData.AudioClip != null)
            {
                _audioSource.PlayOneShot(CurrentWeapon.WeaponData.AudioClip);
            }
        }
    }
}

