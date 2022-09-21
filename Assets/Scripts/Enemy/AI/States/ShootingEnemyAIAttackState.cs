using System.Collections;
using UnityEngine;

namespace BSTW.Enemy.AI.States
{
    public class ShootingEnemyAIAttackState : EnemyAIState
    {
        private bool _startedShooting;
        private Coroutine _shootingCoroutine;

        [SerializeField] private EnemyShooting _enemyShooting;
        [SerializeField] private float _attackPositionRadius = 10f;
        [SerializeField] private float _minAttackDelay = 3f;
        [SerializeField] private float _maxAttackDelay = 5f;

        public override void EnterState()
        {
            EnemyController.NavMeshAgent.speed = MovementSpeed;

            (EnemyController as DefaultEnemyAIController).MoveTowardsArea(_attackPositionRadius, EnemyController.CurrentTarget.transform.position);
        }

        public override void UpdateState()
        {
            if (_startedShooting)
            {
                EnemyController.RotateEnemy(EnemyController.CurrentTarget.transform.position);
            }

            if (EnemyController.HasNavMeshAgentReachedDestination() && !_startedShooting)
            {
                _shootingCoroutine = StartCoroutine(StartShooting());
            }
        }

        public override void ExitState()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);

            _enemyShooting.StopEnemyShooting();
        }

        protected virtual IEnumerator StartShooting()
        {
            _startedShooting = true;

            _enemyShooting.StartEnemyShooting();

            yield return new WaitForSeconds(Random.Range(_minAttackDelay, _maxAttackDelay));

            _enemyShooting.StopShooting();

            (EnemyController as DefaultEnemyAIController).MoveTowardsArea(_attackPositionRadius, EnemyController.CurrentTarget.transform.position);

            _startedShooting = false;
        }
    }
}

