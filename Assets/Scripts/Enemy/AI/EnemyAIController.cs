using BSTW.Enemy.AI.States;
using BSTW.Player;
using BSTW.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Enemy.AI
{
    public abstract class EnemyAIController : MonoBehaviour
    {
        protected EnemyAIState currentState;
        protected GameObject player;

        [SerializeField] private UnityEvent _onTargetDeath;
        [SerializeField] private UnityEvent _onEnemyStop;

        public EnemyHealth EnemyHealth;
        public EnemyAnimator EnemyAnimator;

        [HideInInspector] public Health CurrentTarget;

        protected virtual void Start()
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
        }

        protected virtual void Update()
        {
            CheckTargetHealth();
            UpdateCurrentState();
        }

        private void CheckTargetHealth()
        {
            if (IsTargetAlive())
            {
                _onTargetDeath?.Invoke();
            }
        }

        private void UpdateCurrentState()
        {
            if (currentState != null)
                currentState.UpdateState();
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

        public void RestartCurrentState()
        {
            currentState.RestartState();
        }

        public void StopEnemy()
        {
            _onEnemyStop?.Invoke();
        }

        public abstract void OnHit();
        protected abstract bool IsTargetAlive();
    }
}

