using BSTW.Enemy.AI;
using BSTW.Equipments.Weapons.Shooting;
using UnityEngine;

namespace BSTW.Enemy
{
    public class EnemyShooting : CharacterShooting
    {
        [SerializeField] private EnemyAIController _enemyController;

        protected override Vector3 GetShootingDirection()
        {
            return transform.position - _enemyController.CurrentTarget.transform.position;
        }

        protected override Vector3 GetShootingOrigin()
        {
            return transform.position;
        }

        public void StartEnemyShooting()
        {
            isHoldingShootingTrigger = true;
            CheckShooting();
        }

        public void StopEnemyShooting()
        {
            isHoldingShootingTrigger = false;
            StopShooting();
        }
    }
}

