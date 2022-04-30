using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BSTW.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Vector2 _movement;
        private bool _canMove = false;
        private bool isMoving = false;

        [SerializeField] private Rigidbody _playerRb;
        [SerializeField] private Transform _thirdPersonCamera;

        [SerializeField] private float _waitingTimeToMove = 1f;
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _jumpingSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _maxGroundDistance = 0.5f;

        private void Awake()
        {
            StartCoroutine(WaitToMove(_waitingTimeToMove));
        }

        private void FixedUpdate()
        {
            RotatePlayer();
            MovePlayer();
        }

        private void RotatePlayer()
        {
            if (isMoving)
            {
                var newRotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(_thirdPersonCamera.transform.forward),
                Time.deltaTime * _rotationSpeed);

                newRotation.x = 0f;
                newRotation.z = 0f;

                transform.rotation = newRotation;
            }
        }

        private IEnumerator WaitToMove(float duration)
        {
            yield return new WaitForSeconds(duration);

            _canMove = true;
        }

        private void MovePlayer()
        {
            if (_canMove)
            {
                var forward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
                var right = new Vector3(transform.right.x, 0f, transform.right.z).normalized;
                _playerRb.velocity = (forward * _movement.y) + (right * _movement.x) + Vector3.up * _playerRb.velocity.y;
            }
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _movement = value.ReadValue<Vector2>() * _movementSpeed;
            isMoving = value.performed;
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (CanPlayerJump(value))
            {
                _playerRb.velocity += Vector3.up * _jumpingSpeed;
            }
        }

        private bool CanPlayerJump(InputAction.CallbackContext value)
        {
            return value.started &&
            Physics.Raycast(transform.position, -transform.up, _maxGroundDistance, LayerMask.GetMask("Ground")) && _canMove;
        }
    }
}

