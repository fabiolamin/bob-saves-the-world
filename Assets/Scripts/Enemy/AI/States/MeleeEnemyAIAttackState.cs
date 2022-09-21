using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace BSTW.Enemy.AI.States
{
    public class MeleeEnemyAIAttackState : EnemyAIState
    {
        private Coroutine _attackCoroutine;
        private bool _hasAttacked;

        [SerializeField] private GameObject[] _meleeAttacks;
        [SerializeField] private UnityEvent _onAttack;

        public override void EnterState()
        {
            EnemyController.NavMeshAgent.speed = MovementSpeed;
            _hasAttacked = false;
            EnemyController.NavMeshAgent.SetDestination(EnemyController.CurrentTarget.transform.position);
        }

        public override void UpdateState()
        {
            if (!_hasAttacked)
            {
                EnemyController.RotateEnemy(EnemyController.CurrentTarget.transform.position);
            }

            if (EnemyController.HasNavMeshAgentReachedDestination() && !_hasAttacked)
            {
                _attackCoroutine = StartCoroutine(StartAttack());
            }
        }

        public override void ExitState()
        {
            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);
        }

        private IEnumerator StartAttack()
        {
            _hasAttacked = true;
            _onAttack?.Invoke();

            yield return new WaitForSeconds(EnemyController.EnemyAnimator.GetCurrentAnimationDuration() + 0.5f);

            _hasAttacked = false;
            EnemyController.NavMeshAgent.SetDestination(EnemyController.CurrentTarget.transform.position);
        }

        public void ActivateMeleeAttack(int isActive)
        {
            _meleeAttacks.ToList().ForEach(c => c.SetActive(isActive == 1));
        }
    }
}

