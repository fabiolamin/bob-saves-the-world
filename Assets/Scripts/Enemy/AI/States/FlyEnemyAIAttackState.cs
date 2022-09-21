using UnityEngine;

namespace BSTW.Enemy.AI.States
{
    public class FlyEnemyAIAttackState : EnemyAIState
    {
        [SerializeField] private EnemyShooting _enemyShooting;

        public override void EnterState()
        {
            _enemyShooting.StartEnemyShooting();
        }

        public override void UpdateState()
        {
            EnemyController.RotateEnemy(EnemyController.CurrentTarget.transform.position);
        }

        public override void ExitState()
        {
            _enemyShooting.StopShooting();
        }
    }
}

