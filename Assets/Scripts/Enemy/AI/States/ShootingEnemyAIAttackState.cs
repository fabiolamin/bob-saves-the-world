using System.Collections;
using UnityEngine;

namespace BSTW.Enemy.AI.States
{
    public class ShootingEnemyAIAttackState : EnemyAIState
    {
        private bool _startedShooting;
        private float _currentShootingTime = 0f;
        private Coroutine _shootingCoroutine;

        [SerializeField] private EnemyShooting _enemyShooting;

        [SerializeField] private float _attackPositionRadius = 10f;
        [SerializeField] private float _minAttackDuration = 3f;
        [SerializeField] private float _maxAttackDuration = 5f;

        public override void EnterState()
        {
            base.EnterState();

            _startedShooting = false;

            Move();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (_startedShooting)
            {
                (EnemyController as DefaultEnemyAIController).RotateEnemySmoothly(EnemyController.CurrentTarget.transform.position);

                _currentShootingTime += Time.deltaTime;
            }
            else
            {
                if ((EnemyController as TerrestrialEnemyAIController).HasReachedDestination() &&
                (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.isOnNavMesh)
                {
                    (EnemyController as TerrestrialEnemyAIController).StopNavMeshAgent(true);
                    (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.speed = 0f;

                    (EnemyController as TerrestrialEnemyAIController).LookAtTarget();

                    StopShootingCoroutine();

                    if (_enemyShooting.IsFrontOfObstacle())
                        Move();
                    else
                        _shootingCoroutine = StartCoroutine(StartShooting());
                }
            }
        }

        public override void ExitState()
        {
            base.ExitState();

            StopShootingCoroutine();
        }

        public override void RestartState()
        {
            base.RestartState();

            EnterState();
        }

        private void StopShootingCoroutine()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);
        }

        private void Move()
        {
            if ((EnemyController as TerrestrialEnemyAIController).NavMeshAgent.isOnNavMesh)
            {
                (EnemyController as TerrestrialEnemyAIController).StopNavMeshAgent(false);
                (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.speed = MovementSpeed;
                (EnemyController as TerrestrialEnemyAIController).MoveTowardsArea(_attackPositionRadius, EnemyController.CurrentTarget.transform.position);
            }
        }

        private IEnumerator StartShooting()
        {
            _startedShooting = true;

            yield return new WaitUntil(() =>
            Mathf.Approximately((EnemyController as TerrestrialEnemyAIController).NavMeshAgent.velocity.magnitude, 0f));

            var shootingDuration = Random.Range(_minAttackDuration, _maxAttackDuration);

            _currentShootingTime = 0f;

            while (_currentShootingTime < shootingDuration)
            {
                EnemyController.EnemyAnimator.TriggerAnimationAttack();

                yield return new WaitForSeconds(_enemyShooting.CurrentWeapon.WeaponData.ShootingInterval);
            }

            Move();

            _startedShooting = false;

            yield return null;
        }
    }
}