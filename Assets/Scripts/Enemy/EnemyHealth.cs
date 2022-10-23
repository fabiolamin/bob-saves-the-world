using BSTW.Enemy.AI;
using BSTW.Utils;
using System.Collections;
using UnityEngine;

namespace BSTW.Enemy
{
    public class EnemyHealth : Health
    {
        private Coroutine _checkHitAnimationCoroutine;

        [SerializeField] private EnemyAIController _enemyController;
        [SerializeField] private EnemyAnimator _enemyAnimator;
        [SerializeField] private GameObject _healthBar;

        private void Update()
        {
            UpdateHealthBarTransform();
        }

        private void UpdateHealthBarTransform()
        {
            _healthBar.transform.LookAt(Camera.main.transform);
        }

        protected override void CheckHit(Hit hit)
        {
            _enemyController.StopEnemy();

            base.CheckHit(hit);

            if (_checkHitAnimationCoroutine != null)
            {
                StopCoroutine(_checkHitAnimationCoroutine);
            }

            _checkHitAnimationCoroutine = StartCoroutine(CheckHitAnimation());
        }

        private IEnumerator CheckHitAnimation()
        {
            yield return new WaitForSeconds(_enemyAnimator.GetCurrentAnimationDuration());

            if (IsAlive)
                _enemyController.RestartCurrentState();
        }
    }
}

