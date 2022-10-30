using UnityEngine;

namespace BSTW.Enemy.AI.States
{
    public class FlyEnemyAIInvestigateState : EnemyAIState
    {
        private int _currentWaypointIndex = 0;

        [SerializeField] private FlyEnemyAIPath _path;
        [SerializeField] private float _minDistanceToAdvance = 0.5f;

        public override void EnterState()
        {
            base.EnterState();

            EnemyController.EnemyAnimator.SetMovementParameter(1);
        }

        public override void ExitState()
        {
            base.ExitState();

            EnemyController.EnemyAnimator.SetMovementParameter(0);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            (EnemyController as DefaultEnemyAIController).RotateEnemy(_path.GetWaypoint(_currentWaypointIndex));

            transform.position = Vector3.MoveTowards(
            transform.position,
            _path.GetWaypoint(_currentWaypointIndex),
            MovementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _path.GetWaypoint(_currentWaypointIndex)) <= _minDistanceToAdvance)
            {
                _currentWaypointIndex = _path.GetNextWaypointIndex(_currentWaypointIndex);
            }
        }

        public override void RestartState()
        {
            base.RestartState();

            EnemyController.EnemyAnimator.SetMovementParameter(1);
        }
    }
}

