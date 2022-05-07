using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BSTW.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Vector2 _movement;
        private Vector3 _newMovement;
        private Vector3 _defaultRotation = Vector3.zero;
        private bool _canMove = false;
        private bool isMovingForward = false;

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
            if (isMovingForward)
            {
                _defaultRotation = _thirdPersonCamera.transform.forward;
            }

            var newRotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(_defaultRotation),
                Time.deltaTime * _rotationSpeed);

            newRotation.x = 0f;
            newRotation.z = 0f;

            transform.rotation = newRotation;
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
                var forward = new Vector3(_thirdPersonCamera.transform.forward.x, 0f, _thirdPersonCamera.transform.forward.z).normalized;
                var right = new Vector3(_thirdPersonCamera.transform.right.x, 0f, _thirdPersonCamera.transform.right.z).normalized;
                _newMovement = (forward * _movement.y) + (right * _movement.x) + Vector3.up * _playerRb.velocity.y;
                _playerRb.velocity = _newMovement;

                if(_movement.magnitude != 0f)
                {
                    _defaultRotation = _newMovement;
                }
            }
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _movement = value.ReadValue<Vector2>() * _movementSpeed;
            isMovingForward = _movement.y > 0f && _movement.x == 0f;
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

