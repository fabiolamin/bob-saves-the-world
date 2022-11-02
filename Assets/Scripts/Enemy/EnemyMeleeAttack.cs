using BSTW.Enemy.AI;
using BSTW.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Enemy
{
    public class EnemyMeleeAttack : MonoBehaviour
    {
        [SerializeField] private Hit _hit;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private UnityEvent _onHit;

        private void OnTriggerEnter(Collider other)
        {
            HitTarget(other);
        }

        private void OnEnable()
        {
            _audioSource.Play();
        }

        private void HitTarget(Collider other)
        {
            var targetHealth = other.GetComponent<Health>();
            var isCollidingEnemy = other.GetComponent<EnemyAIController>() != null;

            var canHitTarget = targetHealth != null && !isCollidingEnemy;

            if (canHitTarget)
            {
                targetHealth.Hit(_hit);
                _onHit?.Invoke();
            }
        }
    }
}

