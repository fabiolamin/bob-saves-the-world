using BSTW.Enemy.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSTW.Enemy.AI
{
    public class FlyEnemyAIController : EnemyAIController
    {
        public FlyEnemyAISearchState SearchState = new FlyEnemyAISearchState();
        public FlyEnemyAIChaseState ChaseState = new FlyEnemyAIChaseState();
        public FlyEnemyAIAttackState AttackState = new FlyEnemyAIAttackState();

        private void Start()
        {
            SwitchState(SearchState);
        }
    }
}

