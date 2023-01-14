using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Utils
{
    public class Health : MonoBehaviour
    {
        private float _currentHealth;

        [SerializeField] private float _maxHealth = 100f;
        [Tooltip("Decimal value.")][SerializeField] private float _criticalHealthPercentage = 0.3f;
        [SerializeField] private float _knockDownPercentage = 0.5f;
        [SerializeField] private float _healthUpdateDelay = 0.5f;

        [SerializeField] private UnityEvent _onCriticalHealthStarted;
        [SerializeField] private UnityEvent _onCriticalHealthFinished;

        [SerializeField] private UnityEvent<float, float> _onHealthUpdated;

        [SerializeField] private UnityEvent _onDamageStarted;
        [SerializeField] private UnityEvent _onDamageFinished;

        [SerializeField] private UnityEvent _onHitStarted;
        [SerializeField] private UnityEvent _onHitFinished;

        [SerializeField] private UnityEvent _onKnockdownStarted;
        [SerializeField] private UnityEvent _onKnockdownFinished;

        [SerializeField] private UnityEvent _onHealthAdded;

        [SerializeField] private UnityEvent _onDeath;

        [SerializeField] private UnityEvent _onElectricalHit;

        public bool IsAlive { get; private set; } = true;
        public bool CanUpdateHealth { get; protected set; } = true;
        public bool IsHealthFull { get { return _currentHealth >= _maxHealth; } }
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        protected float KnockDownPercentage => _knockDownPercentage;
        protected bool GotHit = false;
        protected bool HasBeenOnCriticalHealth => _currentHealth <= (_criticalHealthPercentage * _maxHealth) && IsAlive;

        protected UnityEvent OnDamageStarted => _onDamageStarted;
        protected UnityEvent OnDamageFinished => _onDamageFinished;
        protected UnityEvent OnHitStarted => _onHitStarted;
        protected UnityEvent OnHitFinished => _onHitFinished;
        protected UnityEvent OnKnockdownStarted => _onKnockdownStarted;
        protected UnityEvent OnKnockdownFinished => _onKnockdownFinished;

        private void Awake()
        {
            UpdateHealth(_maxHealth);
        }

        private void UpdateHealth(float value)
        {
            CanUpdateHealth = false;

            _currentHealth = Mathf.Clamp(_currentHealth + value, 0f, _maxHealth);
            _onHealthUpdated?.Invoke(_currentHealth, _maxHealth);

            StartCoroutine(SetDelayToUpdateHealth());
        }

        public void Hit(Hit hit)
        {
            if (CanGotHit(hit))
            {
                UpdateHealth(-hit.Damage);
                CheckHealth();
                CheckHit(hit);

                if (HasBeenOnCriticalHealth)
                    _onCriticalHealthStarted?.Invoke();
            }
        }

        protected virtual void CheckHit(Hit hit)
        {
            GotHit = true;
            _onDamageStarted?.Invoke();
            _onHitStarted?.Invoke();
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
            if (!IsAlive || !CanUpdateHealth || IsHealthFull) return;

            if (!HasBeenOnCriticalHealth)
                _onCriticalHealthFinished?.Invoke();

            _onHealthAdded?.Invoke();
            UpdateHealth(value);
        }

        private IEnumerator SetDelayToUpdateHealth()
        {
            yield return new WaitForSeconds(_healthUpdateDelay);

            CanUpdateHealth = true;
        }

        public void OnHitEnd()
        {
            EnableHit();
            OnHitFinished?.Invoke();
        }

        public void OnKnockdownEnd()
        {
            EnableHit();
            _onKnockdownFinished?.Invoke();
        }

        private void EnableHit()
        {
            CanUpdateHealth = true;
            GotHit = false;

            _onDamageFinished?.Invoke();
        }

        protected virtual bool CanGotHit(Hit hit)
        {
            return IsAlive || CanUpdateHealth || hit.Damage != 0f;
        }

        public void RestoreHealth()
        {
            IsAlive = true;

            UpdateHealth(_maxHealth);
        }

        public void GetElectricalHit(Hit hit)
        {
            Hit(hit);
            _onElectricalHit.Invoke();
        }
    }
}