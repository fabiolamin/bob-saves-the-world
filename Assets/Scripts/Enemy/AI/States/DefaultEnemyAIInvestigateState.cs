using System.Collections;
using UnityEngine;

namespace BSTW.Enemy.AI.States
{
    public class DefaultEnemyAIInvestigateState : EnemyAIState
    {
        private bool _hasStopped = false;
        private Coroutine _waitToInvestigateCoroutine;

        [SerializeField] private float _investigateRadius = 10f;
        [SerializeField] private float _minTimeNewInvestigation = 3f;
        [SerializeField] private float _maxTimeNewInvestigation = 5f;

        public override void EnterState()
        {
            EnemyController.NavMeshAgent.speed = MovementSpeed;
            _waitToInvestigateCoroutine = StartCoroutine(WaitToInvestigate());
        }

        public override void UpdateState()
        {
            CheckEnemyDestination();
        }

        public override void ExitState()
        {
            if (_waitToInvestigateCoroutine != null)
                StopCoroutine(_waitToInvestigateCoroutine);
        }

        private IEnumerator WaitToInvestigate()
        {
            yield return new WaitForSeconds(Random.Range(_minTimeNewInvestigation, _maxTimeNewInvestigation));

            (EnemyController as DefaultEnemyAIController).MoveTowardsArea(_investigateRadius, transform.position);

            _hasStopped = false;
        }

        private void CheckEnemyDestination()
        {
            if (EnemyController.HasNavMeshAgentReachedDestination() && !_hasStopped)
            {
                _hasStopped = true;
                _waitToInvestigateCoroutine = StartCoroutine(WaitToInvestigate());
            }
        }
    }
}

