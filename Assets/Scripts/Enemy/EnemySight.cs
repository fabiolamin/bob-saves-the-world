using BSTW.Enemy.AI;
using UnityEngine;

namespace BSTW.Enemy
{
    public class EnemySight : MonoBehaviour
    {
        [SerializeField] private DefaultEnemyAIController _defaultEnemyAIController;

        [SerializeField] private float _chasingDistace = 70f;

        [SerializeField] private float _maxAttackHeightRange = 5f;
        [SerializeField] private bool _checkAttackHeight = true;

        private void Update()
        {
            RemoveTargetWhenFarAway();
        }

        private void OnTriggerEnter(Collider other)
        {
            AddTarget(other);
        }

        private void OnTriggerExit(Collider other)
        {
            RemoveTarget(other);
        }

        private void RemoveTargetWhenFarAway()
        {
            if (_defaultEnemyAIController.AttackState.IsActive)
            {
                if (IsTargetFarAway(_defaultEnemyAIController.CurrentTarget.transform))
                {
                    _defaultEnemyAIController.RemoveTarget(_defaultEnemyAIController.CurrentTarget.gameObject);
                }
            }
        }

        private void AddTarget(Collider other)
        {
            if (_defaultEnemyAIController.CurrentTarget != null && other.gameObject == _defaultEnemyAIController.CurrentTarget.gameObject) return;

            _defaultEnemyAIController.AddTarget(other.gameObject);
        }

        private void RemoveTarget(Collider other)
        {
            if (_defaultEnemyAIController.CurrentTarget != null && other.gameObject == _defaultEnemyAIController.CurrentTarget.gameObject) return;

            _defaultEnemyAIController.RemoveTarget(other.gameObject);
        }

        public bool IsTargetFarAway(Transform target)
        {
            return Vector3.Distance(_defaultEnemyAIController.transform.position, target.transform.position) > _chasingDistace ||
            (_checkAttackHeight && target.position.y - transform.position.y > _maxAttackHeightRange);
        }
    }
}

