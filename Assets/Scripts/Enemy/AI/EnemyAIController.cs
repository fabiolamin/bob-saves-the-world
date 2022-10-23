using BSTW.Enemy.AI.States;
using BSTW.Player;
using BSTW.Utils;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace BSTW.Enemy.AI
{
    public abstract class EnemyAIController : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onTargetDeath;

        protected EnemyAIState currentState;
        protected GameObject player;

        public EnemyHealth EnemyHealth;
        public NavMeshAgent NavMeshAgent;
        public EnemyAnimator EnemyAnimator;

        public EnemyAIState InvestigateState;
        public EnemyAIState AttackState;
        public EnemyAIState DeathState;

        [Header("Speed")]
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _decreaseSpeed = 5f;
        [SerializeField] private float _minSpeedOnDecrease = 0.5f;
        [SerializeField] private float _minDecreaseDistance = 1f;

        [HideInInspector] public Health CurrentTarget;

        protected virtual void Start()
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
        }

        protected virtual void Update()
        {
            CheckTargetHealth();
            UpdateCurrentState();
            CheckNavMeshAgentMovement();
        }

        private void CheckTargetHealth()
        {
            if (CurrentTarget != null && !CurrentTarget.IsAlive && AttackState.IsActive && EnemyHealth.IsAlive)
            {
                _onTargetDeath?.Invoke();
            }
        }

        private void UpdateCurrentState()
        {
            if (currentState != null)
                currentState.UpdateState();
        }

        private void CheckNavMeshAgentMovement()
        {
            if (NavMeshAgent != null)
            {
                DecreaseNavMeshAgentSpeedAsHeReachesDestination();

                EnemyAnimator.SetMovementParameter(NavMeshAgent.velocity.magnitude);
            }
        }

        private void DecreaseNavMeshAgentSpeedAsHeReachesDestination()
        {
            if (NavMeshAgent.hasPath && !NavMeshAgent.isStopped)
            {
                var distanceFromDestination = Vector3.Distance(transform.position, NavMeshAgent.destination);

                if (distanceFromDestination <= _minDecreaseDistance)
                    NavMeshAgent.speed = Mathf.Lerp(NavMeshAgent.speed, _minSpeedOnDecrease, _decreaseSpeed * Time.deltaTime * (1 / distanceFromDestination));
            }
        }

        public void SwitchState(EnemyAIState enemyAIState)
        {
            if (currentState != null)
            {
                currentState.ExitState();
            }

            currentState = enemyAIState;
            currentState.EnterState();
        }

        public bool HasNavMeshAgentReachedDestination()
        {
            return !NavMeshAgent.pathPending && NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance;
        }

        public void RotateEnemy(Vector3 target)
        {
            var newRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position), Time.deltaTime * _rotationSpeed);

            newRotation.x = 0f;
            newRotation.z = 0f;

            transform.rotation = newRotation;
        }

        public void RestoreEnemy()
        {
            EnemyHealth.RestoreHealth();

            SwitchState(InvestigateState);
        }

        public virtual void StopEnemy()
        {
            NavMeshAgent.speed = 0f;
            NavMeshAgent.isStopped = true;
        }

        public void RestartCurrentState()
        {
            currentState.RestartState();
        }

        public abstract void OnHit();
    }
}

