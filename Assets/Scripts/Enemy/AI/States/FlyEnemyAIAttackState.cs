using UnityEngine;

namespace BSTW.Enemy.AI.States
{
    public class FlyEnemyAIAttackState : EnemyAIState
    {
        [SerializeField] private EnemyShooting _enemyShooting;

        public override void EnterState()
        {
            base.EnterState();

            _enemyShooting.StartEnemyShooting();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            (EnemyController as DefaultEnemyAIController).RotateEnemy(EnemyController.CurrentTarget.transform.position);
        }

        public override void ExitState()
        {
            base.ExitState();

            _enemyShooting.StopShooting();
        }

        public override void RestartState()
        {
            base.RestartState();

            _enemyShooting.StartEnemyShooting();
        }
    }
}

