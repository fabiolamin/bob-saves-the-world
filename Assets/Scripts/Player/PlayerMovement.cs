using BSTW.Data.Player;
using BSTW.Equipments;
using BSTW.Game;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace BSTW.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Vector2 _movement = Vector3.zero;
        private Vector3 _newMovement = Vector3.zero;
        private Vector3 _defaultVelocity = Vector3.zero;
        private Vector3 _defaultRotation = Vector3.zero;

        private float _currentSpeed = 0f;
        private float _delayJumpingToFlyAux = 0f;
        private float _currentFallForce = 0f;
        private float _jumpDelay = 0.5f;
        private float _jumpDelayAux;

        private bool _isJumping = false;
        private bool _isHoldingJumpKey = false;
        private bool _canMove = false;
        private bool _canJump = false;
        private bool _isMovingForward = false;

        private GameManager _gameManager;

        [SerializeField] private Rigidbody _playerRb;
        [SerializeField] private Transform _thirdPersonCamera;
        [SerializeField] private JetBackpackUser _jetBackpackUser;
        [SerializeField] private PlayerMovementData _movementData;
        [SerializeField] private PlayerShooting _playerShooting;

        [Header("Events")]
        [SerializeField] private UnityEvent<float, float> _onPlayerMove;
        [SerializeField] private UnityEvent _onPlayerJump;
        [SerializeField] private UnityEvent _onPlayerRoll;
        [SerializeField] private UnityEvent _onPlayerRollFinished;
        [SerializeField] private UnityEvent _onPlayerFall;
        [SerializeField] private UnityEvent<bool> _onPlayerFly;
        [SerializeField] private UnityEvent<bool> _onPlayerIsGround;

        [Header("Camera Shake")]
        [SerializeField] private PlayerCameraShake _playerCameraShake;
        [SerializeField] private CameraShakeData _shakeOnFlying;
        [SerializeField] private CameraShakeData _shakeOnFalling;
        [SerializeField] private float _minFallForceToShakeCamera = 2000f;

        public static bool IsRolling { get; private set; } = false;

        private void Awake()
        {
            if (_movementData != null && _playerRb != null)
            {
                _currentSpeed = _movementData.MovementSpeed;
                _defaultVelocity = _playerRb.velocity;

                StartCoroutine(WaitToMove(_movementData.DelayToMove));
            }

            _gameManager = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            _onPlayerIsGround?.Invoke(PlayerFoot.IsOnTheGround);

            if (!_canMove || _gameManager.IsGamePaused || _gameManager.IsGameFinished) return;

            CheckIfPlayerIsFlying();
            CheckPlayerHeight();
            CheckJumpDelay();
        }

        private void FixedUpdate()
        {
            if (!_canMove || _gameManager.IsGamePaused || _gameManager.IsGameFinished) return;

            RotatePlayer();
            MovePlayer();
        }

        private void CheckIfPlayerIsFlying()
        {
            if (_isHoldingJumpKey && _jetBackpackUser.HasFuel)
            {
                if (_jetBackpackUser.IsFlying) return;

                _delayJumpingToFlyAux += Time.deltaTime;

                if (_delayJumpingToFlyAux > _movementData.DelayJumpingToFly)
                {
                    _onPlayerFly?.Invoke(true);
                    _jetBackpackUser.IsFlying = true;
                    _currentSpeed = _movementData.FlySpeed;
                }
            }
            else
            {
                _onPlayerFly?.Invoke(false);
                _delayJumpingToFlyAux = 0f;
                _jetBackpackUser.IsFlying = false;
            }
        }

        private void CheckPlayerHeight()
        {
            var currentPosition = transform.position;
            currentPosition.y = Mathf.Clamp(currentPosition.y, _movementData.MinPlayerHeight, _movementData.MaxPlayerHeight);
            transform.position = currentPosition;

            if (_playerRb.velocity.y < 0f)
            {
                _currentFallForce += Mathf.Abs(_playerRb.velocity.y);

                if (_currentFallForce >= _minFallForceToShakeCamera)
                    _playerCameraShake.StartShakeCamera(_shakeOnFalling);
            }
            else if (_playerRb.velocity.y > 0f)
            {
                _currentFallForce = 0f;

                if (_jetBackpackUser.IsFlying)
                    _playerCameraShake.StartShakeCamera(_shakeOnFlying);
            }
        }

        private void CheckJumpDelay()
        {
            if (!_canJump)
            {
                _jumpDelayAux -= Time.deltaTime;

                if (_jumpDelayAux <= 0)
                {
                    _canJump = true;
                    _jumpDelayAux = _jumpDelay;
                }
            }
        }

        public void CheckPlayerFall()
        {
            _isJumping = false;
            _isHoldingJumpKey = false;

            if (CanPlayerRollOnFall())
            {
                _onPlayerFall?.Invoke();
                Roll();
            }

            _currentSpeed = _playerShooting.IsAiming ? _movementData.AimingSpeed : _movementData.MovementSpeed;

            _currentFallForce = 0f;
        }

        private bool CanPlayerRollOnFall()
        {
            return _currentFallForce > _movementData.MinFallForceToRoll;
        }

        private void RotatePlayer()
        {
            if (_isMovingForward || ((_playerShooting.IsAiming || _playerShooting.IsShooting) && !IsRolling))
            {
                _defaultRotation = _thirdPersonCamera.transform.forward;
            }

            var newRotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(_defaultRotation),
                Time.deltaTime * _movementData.RotationSpeed);

            newRotation.x = 0f;
            newRotation.z = 0f;

            transform.rotation = newRotation;
        }

        private IEnumerator WaitToMove(float duration)
        {
            yield return new WaitForSeconds(duration);

            _canMove = true;
            _canJump = true;

            _jumpDelayAux = _jumpDelay;
        }

        private void MovePlayer()
        {
            UpdateMovement();
            CheckMovementAnimations();
        }

        private void UpdateMovement()
        {
            var forward = new Vector3(_thirdPersonCamera.transform.forward.x, 0f, _thirdPersonCamera.transform.forward.z).normalized;
            var right = new Vector3(_thirdPersonCamera.transform.right.x, 0f, _thirdPersonCamera.transform.right.z).normalized;

            _newMovement = (forward * _movement.y * _currentSpeed) + (right * _movement.x * _currentSpeed) + Vector3.up * _playerRb.velocity.y;

            if (_jetBackpackUser.IsFlying)
            {
                _newMovement.y = _movementData.FlySpeed;
            }

            _playerRb.velocity = _newMovement;

            if (_movement.magnitude != 0f)
            {
                _defaultRotation = _newMovement;
            }
        }

        private void CheckMovementAnimations()
        {
            if (_jetBackpackUser.IsFlying)
            {
                SetMovementAnimations(0f, 0f);

                return;
            }

            if (_playerShooting.IsAiming || _playerShooting.IsShooting)
                SetMovementAnimations(_movement.x, _movement.y);
            else
                SetMovementAnimations(0f, _movement.magnitude);
        }

        private void SetMovementAnimations(float posX, float posY)
        {
            _onPlayerMove?.Invoke(posX, posY);
        }

        public void Jump()
        {
            _playerRb.velocity += Vector3.up * _movementData.JumpingSpeed;
        }

        public void Bounce()
        {
            var calculatedBounceForce = _movementData.BounceSpeed + (_currentFallForce / _movementData.BounceForceModifier);
            _playerRb.velocity += Vector3.up * calculatedBounceForce;

            Roll();
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
                _canJump = false;
                _onPlayerJump?.Invoke();
            }

            _isHoldingJumpKey = value.action.IsPressed();
        }

        public void OnRoll(InputAction.CallbackContext value)
        {
            if (CanPlayerRollOnGround(value))
            {
                Roll();
            }
        }

        public void OnRollStarted()
        {
            _currentSpeed = _movementData.RollSpeed;
        }

        public void OnRollFinished()
        {
            if (!IsRolling) return;

            IsRolling = false;
            _currentSpeed = _movementData.MovementSpeed;
            _onPlayerRollFinished?.Invoke();
        }

        private bool CanPlayerJump(InputAction.CallbackContext value)
        {
            return value.started && PlayerFoot.IsOnTheGround &&
            _canMove && _canJump && !_isJumping && !_gameManager.IsGamePaused && !_gameManager.IsGameFinished;
        }

        private bool CanPlayerRollOnGround(InputAction.CallbackContext value)
        {
            return value.started && PlayerFoot.IsOnTheGround && _canMove &&
            !_isJumping && !IsRolling && !_jetBackpackUser.IsFlying &&
            _movement.magnitude != 0f && !_gameManager.IsGamePaused && !_gameManager.IsGameFinished;
        }

        private void Roll()
        {
            IsRolling = true;
            _onPlayerRoll?.Invoke();
        }

        public void UpdateMovementSpeedOnAiming(bool isPlayerAiming)
        {
            _currentSpeed = isPlayerAiming ? _movementData.AimingSpeed : _movementData.MovementSpeed;
        }

        public void EnablePlayerMovement(bool isEnabled)
        {
            _canMove = isEnabled;

            if (!isEnabled)
                _playerRb.velocity = Vector3.zero;
        }
    }
}

