using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Environment.Items
{
    public abstract class Item<T> : MonoBehaviour
    {
        private GameObject _collector;
        private Coroutine _activateItemCoroutine;
        private bool _hasInvokedFollowingEvent = false;

        [SerializeField] private Rigidbody _itemRb;
        [SerializeField] private Collider _itemCollider;
        [SerializeField] private string _collectorTag;
        [SerializeField] private GameObject _itemGO;
        [SerializeField] private UnityEvent _onItemCollected;
        [SerializeField] private UnityEvent _onItemFollowing;

        [SerializeField] private float _bonusAmount;

        [SerializeField] private float _followSpeed = 10f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _spawnForce = 10f;

        [SerializeField] private float _radius = 10f;

        protected T collectorPerk;
        protected float bonusAmount => _bonusAmount;

        private void Awake()
        {
            _collector = GameObject.FindGameObjectWithTag(_collectorTag);
            collectorPerk = _collector.GetComponent<T>();
        }

        private void Update()
        {
            RotateItem();
            CheckRadius();
        }

        private void OnEnable()
        {
            Spawn();
        }

        private void OnDisable()
        {
            ActivateItem(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckIfItemCanBeCollected(other);
        }

        private void RotateItem()
        {
            _itemGO.transform.Rotate(Vector3.up * _rotationSpeed, Space.World);
        }

        private void CheckRadius()
        {
            var distance = Vector3.Distance(transform.position, _collector.transform.position);

            if (distance <= _radius && CanBeCollected())
            {
                transform.position = Vector3.MoveTowards(transform.position, _collector.transform.position, _followSpeed * Time.deltaTime);

                if (!_hasInvokedFollowingEvent)
                {
                    _onItemFollowing?.Invoke();
                    _hasInvokedFollowingEvent = true;
                }

                if (!_itemRb.isKinematic && !_itemCollider.isTrigger)
                {
                    ActivateItem(true);

                    if (_activateItemCoroutine != null)
                        StopCoroutine(_activateItemCoroutine);
                }
            }
        }

        public void ActivateItem(bool isActive)
        {
            _itemRb.isKinematic = isActive;
            _itemCollider.isTrigger = isActive;
            _hasInvokedFollowingEvent = !isActive;
        }

        private void CheckIfItemCanBeCollected(Collider other)
        {
            if (CanBeCollected() && other.gameObject == _collector)
            {
                _onItemCollected?.Invoke();
                OnCollected();
                gameObject.SetActive(false);
            }
        }

        public void Spawn()
        {
            var randomDirection = new Vector3(Mathf.Round(Random.Range(-1f, 1f)), 1f, Mathf.Round(Random.Range(-1f, 1f))) * _spawnForce;
            _itemRb.AddForce(randomDirection);

            if (_activateItemCoroutine != null)
                StopCoroutine(_activateItemCoroutine);

            _activateItemCoroutine = StartCoroutine(WaitUntilItemCanBeCollected());
        }

        private IEnumerator WaitUntilItemCanBeCollected()
        {
            yield return new WaitUntil(() => _itemRb.IsSleeping());

            ActivateItem(true);
        }

        protected abstract bool CanBeCollected();
        protected abstract void OnCollected();
    }
}

