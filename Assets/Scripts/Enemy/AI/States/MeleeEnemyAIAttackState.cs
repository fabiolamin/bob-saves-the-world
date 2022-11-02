using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Enemy.AI.States
{
    public class MeleeEnemyAIAttackState : EnemyAIState
    {
        private Coroutine _enterMeleeAttackStateCoroutine;
        private Coroutine _attackCoroutine;
        private bool _canAttack;

        [SerializeField] private UnityEvent _onAttack;

        [SerializeField] private float _enterMeleeAtackDelay = 0.4f;
        [SerializeField] private float _attackDelay = 1f;
        [SerializeField] private float _minDistanceToAttack = 0.5f;

        public override void EnterState()
        {
            _canAttack = false;

            base.EnterState();

            (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.isStopped = true;
            (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.speed = 0f;

            _enterMeleeAttackStateCoroutine = StartCoroutine(EnterMeleeAttackState());
        }

        public override void UpdateState()
        {
            base.UpdateState();

            (EnemyController as DefaultEnemyAIController).RotateEnemy(EnemyController.CurrentTarget.transform.position);

            if (_canAttack)
            {
                (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.destination = EnemyController.CurrentTarget.transform.position;
                (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.speed = MovementSpeed;

                if (IsNearTarget())
                {
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

            (EnemyController as MeleeEnemyAIController).ActivateRightHandAttack(0);
            (EnemyController as MeleeEnemyAIController).ActivateLeftHandAttack(0);
        }

        public override void RestartState()
        {
            base.RestartState();

            if (_canAttack)
            {
                (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.isStopped = false;

                return;
            }

            _enterMeleeAttackStateCoroutine = StartCoroutine(EnterMeleeAttackState());
        }

        private IEnumerator EnterMeleeAttackState()
        {
            yield return new WaitForSeconds(EnemyController.EnemyAnimator.GetCurrentAnimationDuration() + _enterMeleeAtackDelay);

            _canAttack = true;
            (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.isStopped = false;
        }

        private IEnumerator StartAttack()
        {
            _canAttack = false;

            (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.isStopped = true;

            _onAttack?.Invoke();

            yield return new WaitForSeconds(EnemyController.EnemyAnimator.GetCurrentAnimationDuration() + _attackDelay);

            (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.isStopped = false;
            (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.speed = MovementSpeed;
            _canAttack = true;
        }

        private bool IsNearTarget()
        {
            return Vector3.Distance(EnemyController.transform.position, EnemyController.CurrentTarget.transform.position) <= _minDistanceToAttack;
        }
    }
}

