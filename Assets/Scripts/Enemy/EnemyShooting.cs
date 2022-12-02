using BSTW.Enemy.AI;
using BSTW.Equipments.Weapons.Shooting;
using UnityEngine;

namespace BSTW.Enemy
{
    public class EnemyShooting : CharacterShooting
    {
        [SerializeField] private EnemyAIController _enemyController;
        [SerializeField] private Transform _center;
        [SerializeField] private Vector3 _shootingDirectionOffset;

        [Header("Obstacle avoidance")]
        [SerializeField] private float _obstacleAvoidanceDistance = 5f;
        [SerializeField] private string[] _obstacleLayers;

        protected override Vector3 GetShootingDirection()
        {
            if (_enemyController.CurrentTarget == null) return Vector3.zero;

            var direction = (_enemyController.CurrentTarget.transform.position - transform.position) + _shootingDirectionOffset;

            return direction;
        }

        protected override Vector3 GetShootingOrigin()
        {
            return transform.position;
        }

        public void StartEnemyShooting()
        {
            Shoot();

            if (CurrentWeapon.CurrentProjectile == null)
                CurrentWeapon.SetProjectile();
        }

        public bool IsFrontOfObstacle()
        {
            return Physics.Raycast(_center.transform.position,
            _center.transform.forward,
            _obstacleAvoidanceDistance,
            LayerMask.GetMask(_obstacleLayers));
        }
    }
}

