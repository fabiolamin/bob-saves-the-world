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

            (EnemyController as TerrestrialEnemyAIController).StopNavMeshAgent(true);
            (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.speed = 0f;

            _enterMeleeAttackStateCoroutine = StartCoroutine(EnterMeleeAttackState());
        }

        public override void UpdateState()
        {
            base.UpdateState();

            (EnemyController as DefaultEnemyAIController).RotateEnemySmoothly(EnemyController.CurrentTarget.transform.position);

            if (_canAttack)
            {
                Vector3 destination = new Vector3(
                EnemyController.CurrentTarget.transform.position.x,
                EnemyController.transform.position.y,
                EnemyController.CurrentTarget.transform.position.z);

                if ((EnemyController as TerrestrialEnemyAIController).NavMeshAgent.isOnNavMesh)
                {
                    (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.destination = destination;
                    (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.speed = MovementSpeed;
                }

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
                (EnemyController as TerrestrialEnemyAIController).StopNavMeshAgent(false);

                return;
            }

            _enterMeleeAttackStateCoroutine = StartCoroutine(EnterMeleeAttackState());
        }

        private IEnumerator EnterMeleeAttackState()
        {
            yield return new WaitForSeconds(EnemyController.EnemyAnimator.GetCurrentAnimationDuration() + _enterMeleeAtackDelay);

            _canAttack = true;
            (EnemyController as TerrestrialEnemyAIController).StopNavMeshAgent(false);
        }

        private IEnumerator StartAttack()
        {
            _canAttack = false;

            (EnemyController as TerrestrialEnemyAIController).StopNavMeshAgent(true);

            _onAttack?.Invoke();

            yield return new WaitForSeconds(EnemyController.EnemyAnimator.GetCurrentAnimationDuration() + _attackDelay);

            (EnemyController as TerrestrialEnemyAIController).StopNavMeshAgent(false);
            (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.speed = MovementSpeed;
            _canAttack = true;
        }

        private bool IsNearTarget()
        {
            return Vector3.Distance(EnemyController.transform.position, EnemyController.CurrentTarget.transform.position) <= _minDistanceToAttack;
        }
    }
}

