using BSTW.Enemy.AI.States;
using UnityEngine;

namespace BSTW.Enemy.AI
{
    public class EnemyAIController : MonoBehaviour
    {
        //STATES

        //SEARCH FOR TARGETS IN ORDER OF PRIORITY OF THE LEVEL (PLAYER, PROP OR NPC).
        //CHASE WHEN A TARGET IS SIGHTED.
        //ATTACK THE TARGET UNTIL IT IS DEAD. IF THE TARGET GOES VERY FAR AWAY, RETURN TO SEARCH.

        private EnemyAIState currentState;

        private void Update()
        {
            currentState.UpdateState(this);
        }

        public void SwitchState(EnemyAIState enemyAIState)
        {
            currentState = enemyAIState;
            currentState.EnterState(this);
        }
    }
}

