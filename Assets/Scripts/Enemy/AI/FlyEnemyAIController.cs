using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Enemy.AI
{
    public class FlyEnemyAIController : DefaultEnemyAIController
    {
        private Coroutine _respawnCoroutine;

        [SerializeField] private UnityEvent _onRespawn;

        [Tooltip("In seconds.")]
        [SerializeField] private float _respawnInterval;

        public override void OnDeath()
        {
            if (_respawnCoroutine != null)
            {
                StopCoroutine(_respawnCoroutine);
            }

            _respawnCoroutine = StartCoroutine(RespawnByItSelf());
        }

        private IEnumerator RespawnByItSelf()
        {
            yield return new WaitForSeconds(_respawnInterval);

            _onRespawn?.Invoke();

            targets.Clear();
            CurrentTarget = null;
        }
    }
}

