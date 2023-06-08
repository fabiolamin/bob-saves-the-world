using BSTW.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Environment.Items
{
    public abstract class Item : MonoBehaviour
    {
        private Vector3 _startPosition;
        private float _startMass;
        private Coroutine _activateItemCoroutine;
        private bool _hasInvokedFollowingEvent = false;
        private bool _respawnStarted = false;
        private const int CollectableLayer = 11;
        private const int NotCollectableLayer = 12;

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
            _startMass = _itemRb.mass;
        }

        private void OnEnable()
        {
            if (AutoRespawn)
            {
                ActivateItemVisualEffects(true);
            }
            else
            {
                SpawnOnLoot();
            }
        }

        private void Update()
        {
            if (_isOnAutoRespawn) return;

            if (collector != null)
                gameObject.layer = CanBeCollected() ? CollectableLayer : NotCollectableLayer;

            RotateItem();
            CheckRadius();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isOnAutoRespawn) return;

            CheckIfItemCanBeCollected(collision);
        }

        private void RotateItem()
        {
            _itemGO.transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime, Space.World);
        }

        private void CheckRadius()
        {
            var distance = Vector3.Distance(transform.position, collector.transform.position);

            if (distance <= _radius && CanBeCollected())
            {
                transform.position = Vector3.MoveTowards(transform.position, collector.transform.position, _followSpeed * Time.deltaTime);

                _itemRb.mass = 0f;

                if (!_hasInvokedFollowingEvent)
                {
                    _onItemFollowing?.Invoke();
                    _hasInvokedFollowingEvent = true;
                }
            }
            else if (distance <= _radius && _hasInvokedFollowingEvent)
            {
                _hasInvokedFollowingEvent = false;
            }
        }

        public void ActivateItemVisualEffects(bool isActive)
        {
            _itemGO.SetActive(isActive);
            _itemParticles.SetActive(isActive);
        }

        private void CheckIfItemCanBeCollected(Collision other)
        {
            if (other.gameObject != collector) return;

            _hasInvokedFollowingEvent = false;

            if (CanBeCollected())
            {
                OnCollected();

                if (AutoRespawn)
                    StartAutoRespawn();
                else
                    gameObject.SetActive(false);
            }
        }

        public void SpawnOnLoot()
        {
            _itemRb.mass = _startMass;
            _hasInvokedFollowingEvent = false;

            var directions = new int[] { -1, 1 };

            var randomDirection = new Vector3(
            directions[Random.Range(0, directions.Length)],
            1f,
            directions[Random.Range(0, directions.Length)]) * _spawnForce;

            _itemRb.AddForce(randomDirection);
        }

        private void StartAutoRespawn()
        {
            _respawnStarted = true;

            ActivateItemVisualEffects(false);
            StartCoroutine(WaitRespawn());
        }

        private IEnumerator WaitRespawn()
        {
            yield return new WaitForSeconds(Random.Range(_minRespawnTimer, _maxRespawnTimer));

            _itemRb.mass = _startMass;
            transform.position = _startPosition;
            _respawnStarted = false;

            ActivateItemVisualEffects(true);
        }

        protected abstract bool CanBeCollected();
        protected abstract void OnCollected();
    }
}