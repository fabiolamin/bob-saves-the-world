using BSTW.Utils;
using UnityEngine;

namespace BSTW.Enemy
{
    public class EnemyMeleeAttack : MonoBehaviour
    {
        [SerializeField] private Hit _hit;
        [SerializeField] private AudioSource _audioSource;

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

            if (targetHealth != null)
                targetHealth.Hit(_hit);
        }
    }
}

