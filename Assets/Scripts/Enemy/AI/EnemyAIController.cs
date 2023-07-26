using BSTW.Enemy.AI.States;
using BSTW.Player;
using BSTW.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Enemy.AI
{
    public abstract class EnemyAIController : MonoBehaviour
    {
        private Coroutine _deactivationEnemyCoroutine;

        protected EnemyAIState currentState;
        protected GameObject player;

        [SerializeField] private UnityEvent _onTargetDeath;
        [SerializeField] private UnityEvent _onEnemyStop;
        [SerializeField] private UnityEvent _onDeathActivated;

        [SerializeField] private float _rotationSpeed = 10f;

        [SerializeField] private float _deactivationInterval = 5f;

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
            if (IsTargetDead())
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
            ExitCurrentState();

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

        public void RotateEnemySmoothly(Vector3 target, bool lockYAxis = true)
        {
            var newRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position), Time.deltaTime * _rotationSpeed);

            if (lockYAxis)
            {
                newRotation.x = 0f;
                newRotation.z = 0f;
            }

            transform.rotation = newRotation;
        }

        public void RotateEnemyQuickly(Vector3 target, bool lockYAxis = true)
        {
            if (lockYAxis)
            {
                target.y = transform.position.y;
            }

            transform.LookAt(target);
        }

        public virtual void OnDeath()
        {
            if (_deactivationEnemyCoroutine != null)
            {
                StopCoroutine(_deactivationEnemyCoroutine);
            }

            _deactivationEnemyCoroutine = StartCoroutine(DeactivateEnemy());
        }

        private IEnumerator DeactivateEnemy()
        {
            yield return new WaitForSeconds(_deactivationInterval);

            _onDeathActivated?.Invoke();

            gameObject.SetActive(false);
        }

        public void ExitCurrentState()
        {
            if (currentState != null)
            {
                currentState.ExitState();
            }
        }

        public abstract void OnHit();
        protected abstract bool IsTargetDead();
    }
}

