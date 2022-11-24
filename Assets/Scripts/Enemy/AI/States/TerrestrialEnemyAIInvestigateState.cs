using System.Collections;
using UnityEngine;

namespace BSTW.Enemy.AI.States
{
    public class TerrestrialEnemyAIInvestigateState : EnemyAIState
    {
        private bool _hasStopped = false;
        private Coroutine _waitToInvestigateCoroutine;

        [SerializeField] private float _investigateRadius = 10f;
        [SerializeField] private float _minTimeNewInvestigation = 3f;
        [SerializeField] private float _maxTimeNewInvestigation = 5f;

        public override void EnterState()
        {
            base.EnterState();

            (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.isStopped = false;
            (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.speed = MovementSpeed;
            _waitToInvestigateCoroutine = StartCoroutine(WaitToInvestigate());
        }

        public override void UpdateState()
        {
            base.UpdateState();

            CheckEnemyDestination();
        }

        public override void ExitState()
        {
            base.ExitState();

            if (_waitToInvestigateCoroutine != null)
                StopCoroutine(_waitToInvestigateCoroutine);
        }

        public override void RestartState()
        {
            base.RestartState();

            EnterState();
        }

        private IEnumerator WaitToInvestigate()
        {
            yield return new WaitForSeconds(Random.Range(_minTimeNewInvestigation, _maxTimeNewInvestigation));

            (EnemyController as TerrestrialEnemyAIController).MoveTowardsArea(_investigateRadius, transform.position);

            (EnemyController as TerrestrialEnemyAIController).NavMeshAgent.speed = MovementSpeed;
            _hasStopped = false;
        }

        private void CheckEnemyDestination()
        {
            if ((EnemyController as TerrestrialEnemyAIController).HasReachedDestination() && !_hasStopped)
            {
                _hasStopped = true;
                _waitToInvestigateCoroutine = StartCoroutine(WaitToInvestigate());
            }
        }
    }
}

