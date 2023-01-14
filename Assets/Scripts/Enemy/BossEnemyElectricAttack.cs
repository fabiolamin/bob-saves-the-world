using BSTW.Enemy.AI;
using BSTW.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Enemy
{
    public class BossEnemyElectricAttack : MonoBehaviour
    {
        private bool _isElectricAttackActive;

        [SerializeField] private BossEnemyAIController _enemyAIController;
        [SerializeField] private Hit _hit;
        [SerializeField] private ParticleSystem _electricityVFX;
        [SerializeField] private SphereCollider _sphereCollider;
        [SerializeField] private float _currentRadius = 2f;
        [SerializeField] private float _maxRadius = 5f;
        [SerializeField] private float _electricityRadiusConst = 3000f;

        [SerializeField] private UnityEvent _onActivated;

        private void Awake()
        {
            UpdateRadius();
        }

        private void OnTriggerEnter(Collider other)
        {
            var targetHealth = other.GetComponent<Health>();

            if (targetHealth != null)
            {
                targetHealth.GetElectricalHit(_hit);
            }
        }

        public void ActivateElectricAttack()
        {
            _isElectricAttackActive = true;
            _onActivated.Invoke();
        }

        public void IncreaseElectricityRadius()
        {
            if (_isElectricAttackActive)
            {
                var additionalRadius = _electricityRadiusConst * (1 / _enemyAIController.EnemyHealth.CurrentHealth);

                _currentRadius = Mathf.Clamp(_currentRadius + additionalRadius, _currentRadius, _maxRadius);

                UpdateRadius();
            }
        }

        public void ActivateElectricityCollider(bool isActive)
        {
            if (_isElectricAttackActive)
            {
                _sphereCollider.enabled = isActive;
            }
        }

        private void UpdateRadius()
        {
            _electricityVFX.transform.localScale = new Vector3(_currentRadius, _currentRadius, _currentRadius);
            _sphereCollider.radius = _currentRadius;
        }
    }
}

