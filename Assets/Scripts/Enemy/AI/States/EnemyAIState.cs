namespace BSTW.Enemy.AI.States
{
    public abstract class EnemyAIState
    {
        public abstract void EnterState(EnemyAIController enemyAIController);
        public abstract void UpdateState(EnemyAIController enemyAIController);
    }
}

