using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Utils
{
    public class Health : MonoBehaviour
    {
        private float _currentHealth;

        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _knockDownPercentage = 0.5f;
        [SerializeField] private UnityEvent<float, float> _onHealthUpdated;
        [SerializeField] private UnityEvent _onHit;
        [SerializeField] private UnityEvent _onKnockDown;
        [SerializeField] private UnityEvent _onHealthAdded;
        [SerializeField] private UnityEvent _onDeath;

        public bool IsAlive { get; private set; } = true;

        private void Awake()
        {
            UpdateHealth(_maxHealth);
        }

        private void UpdateHealth(float value)
        {
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0f, _maxHealth);
            _onHealthUpdated?.Invoke(_currentHealth, _maxHealth);
        }

        public void Hit(float damage)
        {
            if (!IsAlive) return;

            var hasBeenKnockDown = damage >= (_maxHealth * _knockDownPercentage);

            if (hasBeenKnockDown)
                _onKnockDown?.Invoke();
            else
                _onHit?.Invoke();

            UpdateHealth(-damage);
            CheckHealth();
        }

        private void CheckHealth()
        {
            if (_currentHealth <= 0f)
            {
                IsAlive = false;
                _onDeath?.Invoke();
            }
        }

        public void AddHealth(float value)
        {
            if (!IsAlive) return;

            _onHealthAdded?.Invoke();
            UpdateHealth(value);
        }
    }
}