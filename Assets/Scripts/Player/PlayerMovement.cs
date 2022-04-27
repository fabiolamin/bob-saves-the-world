using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BSTW.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Vector2 _movement;
        private bool _canMove = false;

        [SerializeField] private Rigidbody _playerRb;
        [SerializeField] private float _waitingTimeToMove = 1f;
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _jumpingSpeed = 5f;
        [SerializeField] private float _maxGroundDistance = 0.5f;

        private void Awake()
        {
            StartCoroutine(WaitToMove(_waitingTimeToMove));
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private IEnumerator WaitToMove(float duration)
        {
            yield return new WaitForSeconds(duration);

            _canMove = true;
        }

        private void MovePlayer()
        {
            if (_canMove)
                _playerRb.velocity = new Vector3(_movement.x, _playerRb.velocity.y, _movement.y);
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _movement = value.ReadValue<Vector2>() * _movementSpeed;
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (CanPlayerJump(value))
                _playerRb.velocity += Vector3.up * _jumpingSpeed;
        }

        private bool CanPlayerJump(InputAction.CallbackContext value)
        {
            return value.started &&
            Physics.Raycast(transform.position, -transform.up, _maxGroundDistance, LayerMask.GetMask("Ground")) && _canMove;
        }
    }
}

