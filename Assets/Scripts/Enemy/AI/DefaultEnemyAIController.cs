using BSTW.Enemy.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BSTW.Enemy.AI
{
    public class DefaultEnemyAIController : EnemyAIController
    {
        public DefaultEnemyAISearchState SearchState = new DefaultEnemyAISearchState();
        public DefaultEnemyAIChaseState ChaseState = new DefaultEnemyAIChaseState();
        public DefaultEnemyAIAttackState AttackState = new DefaultEnemyAIAttackState();

        public NavMeshAgent NavMeshAgent;

        private void Start()
        {
            SwitchState(SearchState);
        }
    }
}

