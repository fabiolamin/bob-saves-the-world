using BSTW.Enemy.AI;
using BSTW.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Enemy
{
    public class EnemyMeleeAttack : MonoBehaviour
    {
        private List<Collider> _targets = new List<Collider>();

        [SerializeField] private Hit _hit;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private UnityEvent _onHit;

        [SerializeField] private RuntimeAnimatorController _animatorController;

        public RuntimeAnimatorController AnimatorController => _animatorController;

        private void OnTriggerEnter(Collider other)
        {
            HitTarget(other);
        }

        private void OnEnable()
        {
            _audioSource.Play();

            _targets.Clear();
        }

        private void HitTarget(Collider other)
        {
            if (_targets.Contains(other)) return;

            var targetHealth = other.GetComponent<Health>();
            var isCollidingEnemy = other.GetComponent<EnemyAIController>() != null;

            var canHitTarget = targetHealth != null && !isCollidingEnemy;

            if (canHitTarget)
            {
                targetHealth.Hit(_hit);
                _onHit?.Invoke();

                _targets.Add(other);
            }
        }
    }
}

