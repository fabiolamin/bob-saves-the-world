using System.Collections;
using UnityEngine;

namespace BSTW.Enemy.AI.States
{
    public class FlyEnemyAIAttackState : EnemyAIState
    {
        private bool _startedShooting;

        private Coroutine _shootingCoroutine;

        [SerializeField] private EnemyShooting _enemyShooting;

        public override void EnterState()
        {
            base.EnterState();

            _startedShooting = true;

            StopShootingCoroutine();

            _shootingCoroutine = StartCoroutine(StartShooting());
        }

        public override void UpdateState()
        {
            base.UpdateState();

            (EnemyController as DefaultEnemyAIController).RotateEnemy(EnemyController.CurrentTarget.transform.position, false);
        }

        public override void ExitState()
        {
            base.ExitState();

            _startedShooting = false;

            StopShootingCoroutine();
        }

        public override void RestartState()
        {
            base.RestartState();

            EnterState();
        }

        private void StopShootingCoroutine()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);
        }

        private IEnumerator StartShooting()
        {
            while (_startedShooting)
            {
                EnemyController.EnemyAnimator.TriggerAnimationAttack();

                yield return new WaitForSeconds(_enemyShooting.CurrentWeapon.WeaponData.ShootingInterval);
            }
        }
    }
}

