using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using BSTW.Equipments.Weapons.Shooting;
using System.Collections;

namespace BSTW.Player
{
    public class PlayerShooting : CharacterShooting
    {
        private Coroutine _hitMarkerCoroutine;

        [SerializeField] private GameObject _aimImage;
        [SerializeField] private GameObject _hitMarker;
        [SerializeField] private float _hitMarkerDuration = 0.1f;

        [SerializeField] private UnityEvent<bool> _onPlayerAim;

        [SerializeField] private AudioClip _emptyGunSFX;

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

            if (CurrentWeapon.WeaponData.CurrentAmmo <= 0f && value.started)
                shootingAudioSource.PlayOneShot(_emptyGunSFX);

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
        }

        public override void OnCharacterHitTarget(bool isTargetAlive)
        {
            base.OnCharacterHitTarget(isTargetAlive);

            _hitMarker.SetActive(true);

            if(_hitMarkerCoroutine != null)
            {
                StopCoroutine(_hitMarkerCoroutine);
            }

            _hitMarkerCoroutine = StartCoroutine(DisableHitMarker());
        }

        private IEnumerator DisableHitMarker()
        {
            var currenteDuration = 0f;

            while(currenteDuration < _hitMarkerDuration)
            {
                currenteDuration += Time.deltaTime;
                yield return null;
            }

            _hitMarker.SetActive(false);
        }
    }
}

