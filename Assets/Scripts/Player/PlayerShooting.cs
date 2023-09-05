using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using BSTW.Equipments.Weapons.Shooting;
using System.Collections;
using BSTW.Game;
using BSTW.Utils;
using System.Linq;

namespace BSTW.Player
{
    public class PlayerShooting : CharacterShooting
    {
        private Coroutine _hitMarkerCoroutine;
        private GameManager _gameManager;

        [SerializeField] private GameObject _aimImage;
        [SerializeField] private GameObject _hitMarker;
        [SerializeField] private float _hitMarkerDuration = 0.1f;

        [SerializeField] private UnityEvent<bool> _onPlayerAim;

        [SerializeField] private AudioClip _emptyGunSFX;

        [SerializeField] private PlayerCameraShake _playerCameraShake;
        [SerializeField] private GamepadRumbleController _gamepadRumbleController;

        public bool IsAiming { get; private set; } = false;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        public void OnAim(InputAction.CallbackContext value)
        {
            if (_gameManager.IsGamePaused || _gameManager.IsGameFinished) return;

            IsAiming = value.action.IsPressed();

            if (!IsReadyToShoot) return;

            _aimImage.SetActive(IsAiming || shootingTriggered);
            _onPlayerAim?.Invoke(IsAiming);
        }

        public void OnShoot(InputAction.CallbackContext value)
        {
            if (_gameManager.IsGamePaused || _gameManager.IsGameFinished) return;

            if (CurrentWeapon != null)
            {
                shootingTriggered = CurrentWeapon.WeaponData.HoldShootingTrigger ? value.action.IsPressed() : value.started;
            }

            if (CurrentWeapon.WeaponData.CurrentAmmo <= 0f && value.started)
                shootingAudioSource.PlayOneShot(_emptyGunSFX);

            if (!IsReadyToShoot) return;

            CheckShooting();
        }

        public void OnSwitchWeapon(InputAction.CallbackContext value)
        {
            if (_gameManager.IsGamePaused || _gameManager.IsGameFinished) return;

            if (value.started && IsReadyToShoot)
            {
                int deltaPos = (int)value.ReadValue<Vector2>().normalized.y;

                if (!PlayerMovement.IsRolling)
                    SwitchWeapon(deltaPos);
            }
        }

        protected override void CheckShooting()
        {
            _aimImage.SetActive(IsAiming || shootingTriggered);

            base.CheckShooting();
        }

        public void EnablePlayerShooting(bool isEnabled)
        {
            IsReadyToShoot = isEnabled;
            _aimImage.SetActive(false);
            _onPlayerAim?.Invoke(isEnabled && IsAiming);
        }

        protected override Vector3 GetShootingOrigin()
        {
            return Camera.main.transform.position;
        }

        protected override Vector3 GetShootingDirection()
        {
            return Camera.main.transform.forward;
        }

        public override void OnCharacterHitTarget(bool isTargetAlive)
        {
            base.OnCharacterHitTarget(isTargetAlive);

            _hitMarker.SetActive(true);

            if (_hitMarkerCoroutine != null)
            {
                StopCoroutine(_hitMarkerCoroutine);
            }

            _hitMarkerCoroutine = StartCoroutine(DisableHitMarker());
        }

        private IEnumerator DisableHitMarker()
        {
            var currenteDuration = 0f;

            while (currenteDuration < _hitMarkerDuration)
            {
                currenteDuration += Time.deltaTime;
                yield return null;
            }

            _hitMarker.SetActive(false);
        }

        public void ResetPlayerShooting()
        {
            IsAiming = false;
            IsShooting = false;
            shootingTriggered = false;
            IsReadyToShoot = true;

            _aimImage.SetActive(IsAiming);
            _onPlayerAim?.Invoke(IsAiming);
        }

        public override void TriggerShootingEffects()
        {
            base.TriggerShootingEffects();

            _playerCameraShake.StartShakeCamera(CurrentWeapon.WeaponData.CameraShakeData);
            _gamepadRumbleController.StartGamepadRumble(CurrentWeapon.WeaponData.GamepadRumbleData);
        }

        protected override void InstantiateWeapons()
        {
            base.InstantiateWeapons();

            Weapons.ToList().ForEach(w => { w.WeaponData.IsSelected = false; w.gameObject.SetActive(false); });

            var weapon = Weapons[0];

            weapon.gameObject.SetActive(true);

            SetCurrentWeapon(weapon);
        }
    }
}

