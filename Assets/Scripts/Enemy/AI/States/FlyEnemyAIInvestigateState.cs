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
            EnemyController.EnemyAnimator.SetMovementParameter(true);
        }

        public override void ExitState()
        {
            EnemyController.EnemyAnimator.SetMovementParameter(false);
        }

        public override void UpdateState()
        {
            EnemyController.RotateEnemy(_path.GetWaypoint(_currentWaypointIndex));

            transform.position = Vector3.MoveTowards(
            transform.position,
            _path.GetWaypoint(_currentWaypointIndex),
            MovementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _path.GetWaypoint(_currentWaypointIndex)) <= _minDistanceToAdvance)
            {
                _currentWaypointIndex = _path.GetNextWaypointIndex(_currentWaypointIndex);
            }
        }
    }
}

