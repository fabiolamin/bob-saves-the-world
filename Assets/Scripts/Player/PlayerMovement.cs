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

        private bool _isJumping = false;
        private bool _canMove = false;
        private bool _isMovingForward = false;

        [SerializeField] private Rigidbody _playerRb;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Transform _thirdPersonCamera;

        [SerializeField] private float _waitingTimeToMove = 1f;
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _jumpingSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 5f;

        private void Awake()
        {
            StartCoroutine(WaitToMove(_waitingTimeToMove));
            PlayerFoot.OnPlayerFall += TurnOffJumping;
        }

        private void Update()
        {
            _playerAnimator.SetBool("IsTouchingGround", PlayerFoot.IsOnTheGround);
        }

        private void FixedUpdate()
        {
            RotatePlayer();
            MovePlayer();
        }

        private void TurnOffJumping()
        {
            _isJumping = false;
        }

        private void RotatePlayer()
        {
            if (_isMovingForward)
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

                _newMovement = (forward * _movement.y * _movementSpeed) + (right * _movement.x * _movementSpeed) + Vector3.up * _playerRb.velocity.y;

                _playerRb.velocity = _newMovement;

                if (_movement.magnitude != 0f)
                {
                    _defaultRotation = _newMovement;
                }

                _playerAnimator.SetFloat("Movement", _movement.magnitude);
            }
        }

        public void Jump()
        {
            _playerRb.velocity += Vector3.up * _jumpingSpeed;
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _movement = value.ReadValue<Vector2>();
            _isMovingForward = _movement.y > 0f && _movement.x == 0f;
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (CanPlayerJump(value))
            {
                _isJumping = true;
                _playerAnimator.SetTrigger("Jump");
            }
        }

        private bool CanPlayerJump(InputAction.CallbackContext value)
        {
            return value.started && PlayerFoot.IsOnTheGround && _canMove && !_isJumping;
        }
    }
}

