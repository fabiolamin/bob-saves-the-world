using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace BSTW.Enemy.AI
{
    public class TerrestrialEnemyAIController : DefaultEnemyAIController
    {
        public NavMeshAgent NavMeshAgent;

        protected override void Update()
        {
            base.Update();

            CheckMovement();
        }

        private void CheckMovement()
        {
            if (NavMeshAgent != null)
            {
                EnemyAnimator.SetMovementParameter(NavMeshAgent.velocity.magnitude);
            }
        }

        public bool HasReachedDestination()
        {
            var hasReachedDestination = !NavMeshAgent.pathPending && NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance;

            if (hasReachedDestination)
                OnMovementFinished?.Invoke();

            return hasReachedDestination;
        }

        public void MoveTowardsArea(float radius, Vector3 center)
        {
            NavMeshHit hit;

            center.y = transform.position.y;

            var newPosition = GetArea(radius, center);

            while (!NavMesh.SamplePosition(newPosition, out hit, radius, 1))
            {
                newPosition = GetArea(radius, center);
            }

            OnMovementStarted?.Invoke();
            NavMeshAgent.SetDestination(hit.position);
        }

        private Vector3 GetArea(float radius, Vector3 center)
        {
            var randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            var x = Mathf.Cos(randomAngle) * radius;
            var z = Mathf.Sin(randomAngle) * radius;

            var newPosition = center + new Vector3(x, 0f, z);

            return newPosition;
        }

        public void LookAtTarget()
        {
            if (CurrentTarget == null) return;

            var targetPosition = new Vector3(CurrentTarget.transform.position.x,
            transform.position.y,
            CurrentTarget.transform.position.z);

            transform.LookAt(targetPosition);
        }
    }
}