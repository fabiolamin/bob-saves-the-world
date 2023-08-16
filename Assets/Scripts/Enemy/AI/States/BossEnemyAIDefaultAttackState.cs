using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Enemy.AI.States
{
    public class BossEnemyAIDefaultAttackState : EnemyAIState
    {
        private Coroutine _enterMeleeAttackStateCoroutine;
        private Coroutine _attackCoroutine;
        private bool _canAttack;

        [SerializeField] private UnityEvent _onAttack;
        [SerializeField] private UnityEvent _onAttackEnd;

        [SerializeField] private float _minDistanceToAttack = 13;
        [SerializeField] private float _attackDelay = 1f;

        public override void EnterState()
        {
            base.EnterState();

            StartBossEnemyMovement();

            _canAttack = true;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (_canAttack)
            {
                if ((EnemyController as BossEnemyAIController).NavMeshAgent.isOnNavMesh)
                {
                    (EnemyController as BossEnemyAIController).NavMeshAgent.destination = EnemyController.CurrentTarget.transform.position;
                    (EnemyController as BossEnemyAIController).NavMeshAgent.speed = MovementSpeed;
                }

                if ((EnemyController as BossEnemyAIController).IsNearTarget(
                EnemyController.CurrentTarget.transform.position,
                _minDistanceToAttack))
                {
                    if (_attackCoroutine != null)
                        StopCoroutine(_attackCoroutine);

                    _attackCoroutine = StartCoroutine(StartAttack());
                }
            }
        }

        public override void ExitState()
        {
            base.ExitState();

            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            if (_enterMeleeAttackStateCoroutine != null)
                StopCoroutine(_enterMeleeAttackStateCoroutine);

            (EnemyController as BossEnemyAIController).ActivateRightHandAttack(0);
            (EnemyController as BossEnemyAIController).ActivateLeftHandAttack(0);
            (EnemyController as BossEnemyAIController).ActivateCenterAttack(0);

            _canAttack = false;
        }

        public override void RestartState()
        {

        }

        private IEnumerator StartAttack()
        {
            EnemyController.StopEnemy();

            _canAttack = false;

            (EnemyController as BossEnemyAIController).StopNavMeshAgent(true);
            (EnemyController as BossEnemyAIController).NavMeshAgent.speed = 0f;

            EnemyController.RotateEnemyQuickly(EnemyController.CurrentTarget.transform.position);

            _onAttack?.Invoke();

            yield return new WaitForSeconds(EnemyController.EnemyAnimator.GetCurrentAnimationDuration());

            _onAttackEnd?.Invoke();

            yield return new WaitForSeconds(_attackDelay);

            StartBossEnemyMovement();

            _canAttack = true;
        }

        private void StartBossEnemyMovement()
        {
            if (!(EnemyController as BossEnemyAIController).NavMeshAgent.isOnNavMesh) return;

            (EnemyController as BossEnemyAIController).StopNavMeshAgent(false);
            (EnemyController as BossEnemyAIController).NavMeshAgent.speed = MovementSpeed;
        }
    }
}

