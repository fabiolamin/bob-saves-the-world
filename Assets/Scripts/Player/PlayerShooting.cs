using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;

namespace BSTW.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        private bool _isHoldingShootingTrigger = false;
       
        [SerializeField] private GameObject _aimImage;
        [SerializeField] private float _shootingDelay = 1.5f;
        [SerializeField] private float _weaponDelay = 1.5f;
        [SerializeField] private float _weaponShootingDistance = 500f;
        [SerializeField] private UnityEvent<bool> _onPlayerAim;

        public static bool IsAiming { get; private set; } = false;
        public static bool IsShooting { get; private set; } = false;

        public void OnAim(InputAction.CallbackContext value)
        {
            IsAiming = value.action.IsPressed();
            _aimImage.SetActive(IsAiming);
            _onPlayerAim?.Invoke(IsAiming);
        }

        public void OnShoot(InputAction.CallbackContext value)
        {
            _isHoldingShootingTrigger = value.action.IsPressed();

            if (!IsShooting && _isHoldingShootingTrigger)
                StartCoroutine(GetReadyToShoot());
        }

        private IEnumerator GetReadyToShoot()
        {
            IsShooting = true;

            while (_isHoldingShootingTrigger)
            {
                Shoot();

                yield return new WaitForSeconds(_weaponDelay);
            }

            IsShooting = false;
        }

        private void Shoot()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _weaponShootingDistance))
            {
                //Hit target
            }

        }
    }
}

