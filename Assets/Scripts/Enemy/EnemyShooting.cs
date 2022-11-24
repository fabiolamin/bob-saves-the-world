using BSTW.Enemy.AI;
using BSTW.Equipments.Weapons.Shooting;
using UnityEngine;

namespace BSTW.Enemy
{
    public class EnemyShooting : CharacterShooting
    {
        [SerializeField] private EnemyAIController _enemyController;
        [SerializeField] private Transform _center;

        protected override Vector3 GetShootingDirection()
        {
            if (_enemyController.CurrentTarget == null) return Vector3.zero;

            var direction = _enemyController.CurrentTarget.transform.position - transform.position;

            direction.y = _enemyController.CurrentTarget.transform.position.y > transform.position.y ?
            ((_enemyController.CurrentTarget.transform.position.y - transform.position.y) + 0.1f) :
            _center.position.y;

            return direction;
        }

        protected override Vector3 GetShootingOrigin()
        {
            return transform.position;
        }

        public void StartEnemyShooting()
        {
            Shoot();

            if (CurrentWeapon.CurrentProjectile == null)
                CurrentWeapon.SetProjectile();
        }
    }
}

