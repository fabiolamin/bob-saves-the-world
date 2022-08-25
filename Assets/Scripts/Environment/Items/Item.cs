using BSTW.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Environment.Items
{
    public abstract class Item : MonoBehaviour
    {
        private Vector3 _startPosition;
        private Coroutine _activateItemCoroutine;
        private bool _hasInvokedFollowingEvent = false;
        private bool _respawnStarted = false;

        private bool _isOnAutoRespawn => AutoRespawn && _respawnStarted;

        [SerializeField] private Rigidbody _itemRb;
        [SerializeField] private Collider _itemCollider;
        [SerializeField] private GameObject _itemGO;
        [SerializeField] private GameObject _itemParticles;
        [SerializeField] private UnityEvent _onItemFollowing;

        [SerializeField] private float _bonusAmount;

        [Header("Movement")]
        [SerializeField] private float _followSpeed = 10f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _spawnForce = 10f;
        [SerializeField] private float _radius = 10f;

        [Header("Respawn")]
        [SerializeField] private float _minRespawnTimer = 5f;
        [SerializeField] private float _maxRespawnTimer = 7f;

        protected GameObject collector;
        protected float bonusAmount => _bonusAmount;

        public bool AutoRespawn = false;

        private void Awake()
        {
            collector = FindObjectOfType<PlayerMovement>().gameObject;
            _startPosition = transform.position;
        }
        private void OnEnable()
        {
            if (AutoRespawn)
            {
                ActivateItem(true);
                ActivateItemPhysics(true);
            }
            else
            {
                SpawnOnLoot();
            }
        }

        private void OnDisable()
        {
            if (!AutoRespawn)
                ActivateItemPhysics(false);
        }

        private void Update()
        {
            if (_isOnAutoRespawn) return;

            RotateItem();
            CheckRadius();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isOnAutoRespawn) return;

            CheckIfItemCanBeCollected(other);
        }

        private void RotateItem()
        {
            _itemGO.transform.Rotate(Vector3.up * _rotationSpeed, Space.World);
        }

        private void CheckRadius()
        {
            var distance = Vector3.Distance(transform.position, collector.transform.position);

            if (distance <= _radius && CanBeCollected())
            {
                transform.position = Vector3.MoveTowards(transform.position, collector.transform.position, _followSpeed * Time.deltaTime);

                if (!_hasInvokedFollowingEvent)
                {
                    _onItemFollowing?.Invoke();
                    _hasInvokedFollowingEvent = true;
                }

                if (!_itemRb.isKinematic && !_itemCollider.isTrigger)
                {
                    ActivateItemPhysics(true);

                    if (_activateItemCoroutine != null)
                        StopCoroutine(_activateItemCoroutine);
                }
            }
        }

        public void ActivateItemPhysics(bool isActive)
        {
            _itemRb.isKinematic = isActive;
            _itemCollider.isTrigger = isActive;
            _hasInvokedFollowingEvent = !isActive;
        }

        public void ActivateItem(bool isActive)
        {
            _itemGO.SetActive(isActive);
            _itemParticles.SetActive(isActive);
        }

        private void CheckIfItemCanBeCollected(Collider other)
        {
            if (CanBeCollected() && other.gameObject == collector)
            {
                OnCollected();

                if (AutoRespawn)
                    StartAutoRespawn();
                else
                    gameObject.SetActive(false);
            }

            _hasInvokedFollowingEvent = false;
        }

        public void SpawnOnLoot()
        {
            _hasInvokedFollowingEvent = false;

            var randomDirection = new Vector3(Mathf.Round(Random.Range(-1f, 1f)), 1f, Mathf.Round(Random.Range(-1f, 1f))) * _spawnForce;
            _itemRb.AddForce(randomDirection);

            if (_activateItemCoroutine != null)
                StopCoroutine(_activateItemCoroutine);

            _activateItemCoroutine = StartCoroutine(WaitUntilItemCanBeCollected());
        }

        private IEnumerator WaitUntilItemCanBeCollected()
        {
            yield return new WaitUntil(() => _itemRb.IsSleeping());

            ActivateItemPhysics(true);
        }

        private void StartAutoRespawn()
        {
            _respawnStarted = true;

            ActivateItem(false);
            StartCoroutine(WaitRespawn());
        }

        private IEnumerator WaitRespawn()
        {
            yield return new WaitForSeconds(Random.Range(_minRespawnTimer, _maxRespawnTimer));

            transform.position = _startPosition;
            _respawnStarted = false;

            ActivateItem(true);
            ActivateItemPhysics(true);
        }

        protected abstract bool CanBeCollected();
        protected abstract void OnCollected();
    }
}

