using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Enemy.AI.States
{
    public class EnemyAIState : MonoBehaviour
    {
        public EnemyAIController EnemyController;

        public float MovementSpeed = 3.5f;

        public UnityEvent OnEnter;
        public UnityEvent OnUpdate;
        public UnityEvent OnExit;

        public bool IsActive { get; private set; }

        public virtual void EnterState()
        {
            IsActive = true;
            OnEnter?.Invoke();
        }

        public virtual void UpdateState()
        {
            if (!IsActive) return;

            OnUpdate?.Invoke();
        }
        public virtual void ExitState()
        {
            IsActive = false;
            OnExit?.Invoke();
        }

        public virtual void RestartState()
        {
            ExitState();
            IsActive = true;
        }
    }
}

