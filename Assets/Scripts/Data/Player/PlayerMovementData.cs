using UnityEngine;

namespace BSTW.Data.Player
{
    [CreateAssetMenu(fileName = "Player Movement Data", menuName = "Data/Player/new Player Movement Data")]
    public class PlayerMovementData : ScriptableObject
    {
        [Header("Speed")]
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _rollSpeed = 10f;
        [SerializeField] private float _jumpingSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _flySpeed = 5f;
        [SerializeField] private float _aimingSpeed = 5f;

        [Header("Time")]
        [SerializeField] private float _delayJumpingToFly = 1.5f;
        [SerializeField] private float _delayToMove = 1f;

        [Header("Height")]
        [SerializeField] private float _maxPlayerHeight = 90f;
        [SerializeField] private float _minPlayerHeight = -1f;
        [SerializeField] private float _minFallForceToRoll = 15f;

        public float MovementSpeed => _movementSpeed;
        public float RollSpeed => _rollSpeed;
        public float JumpingSpeed => _jumpingSpeed;
        public float RotationSpeed => _rotationSpeed;
        public float FlySpeed => _flySpeed;
        public float AimingSpeed => _aimingSpeed;

        public float DelayJumpingToFly => _delayJumpingToFly;
        public float DelayToMove => _delayToMove;

        public float MaxPlayerHeight => _maxPlayerHeight;
        public float MinPlayerHeight => _minPlayerHeight;
        public float MinFallForceToRoll => _minFallForceToRoll;
    }
}

