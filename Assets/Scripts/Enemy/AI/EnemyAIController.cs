using BSTW.Enemy.AI.States;
using BSTW.Player;
using UnityEngine;
using UnityEngine.AI;

namespace BSTW.Enemy.AI
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 10f;

        protected GameObject player;
        protected EnemyAIState currentState;

        public NavMeshAgent NavMeshAgent;
        public EnemyAnimator EnemyAnimator;

        [HideInInspector] public GameObject CurrentTarget;

        protected virtual void Start()
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
        }

        private void Update()
        {
            if (currentState != null)
                currentState.UpdateState();

            if (NavMeshAgent != null)
                EnemyAnimator.SetMovementParameter(!HasNavMeshAgentReachedDestination());
        }

        protected virtual void OnTriggerEnter(Collider other)
        {

        }

        protected virtual void OnTriggerExit(Collider other)
        {

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

        public virtual void OnHit()
        {

        }
    }
}

