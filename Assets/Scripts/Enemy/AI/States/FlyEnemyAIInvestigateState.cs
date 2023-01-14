using UnityEngine;

namespace BSTW.Enemy.AI.States
{
    public class FlyEnemyAIInvestigateState : EnemyAIState
    {
        private int _currentWaypointIndex = 0;
        private bool _canMove;

        [SerializeField] private FlyEnemyAIPath _path;
        [SerializeField] private float _minDistanceToAdvance = 0.5f;

        public override void EnterState()
        {
            base.EnterState();

            _canMove = true;

            (EnemyController as DefaultEnemyAIController).OnMovementStarted?.Invoke();
            EnemyController.EnemyAnimator.SetMovementParameter(1f);
        }

        public override void ExitState()
        {
            base.ExitState();

            _canMove = false;

            (EnemyController as DefaultEnemyAIController).OnMovementFinished?.Invoke();
            EnemyController.EnemyAnimator.SetMovementParameter(0);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Move();
        }

        public override void RestartState()
        {
            base.RestartState();

            EnterState();
        }

        private void Move()
        {
            if (!_canMove) return;

            (EnemyController as DefaultEnemyAIController).RotateEnemySmoothly(_path.GetWaypoint(_currentWaypointIndex), false);

            EnemyController.transform.position = Vector3.MoveTowards(
            EnemyController.transform.position,
            _path.GetWaypoint(_currentWaypointIndex),
            MovementSpeed * Time.deltaTime);

            if (Vector3.Distance(EnemyController.transform.position, _path.GetWaypoint(_currentWaypointIndex)) <= _minDistanceToAdvance)
            {
                _currentWaypointIndex = _path.GetNextWaypointIndex(_currentWaypointIndex);
            }
        }

        public void StopEnemy()
        {
            if (!IsActive) return;

            _canMove = false;
        }
    }
}

