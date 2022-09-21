using UnityEngine;

namespace BSTW.Enemy.AI.States
{
    public abstract class EnemyAIState : MonoBehaviour
    {
        public EnemyAIController EnemyController;

        public float MovementSpeed = 3.5f;

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
    }
}

