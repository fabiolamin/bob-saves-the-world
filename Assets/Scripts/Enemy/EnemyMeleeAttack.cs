using BSTW.Utils;
using UnityEngine;

namespace BSTW.Enemy
{
    public class EnemyMeleeAttack : MonoBehaviour
    {
        [SerializeField] private Hit _hit;

        private void OnTriggerEnter(Collider other)
        {
            var targetHealth = other.GetComponent<Health>();

            if (targetHealth != null)
                targetHealth.Hit(_hit);
        }
    }
}

