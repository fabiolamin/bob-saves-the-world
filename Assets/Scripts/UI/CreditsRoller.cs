using UnityEngine;

namespace BSTW.UI
{
    public class CreditsRoller : MonoBehaviour
    {
        private Vector3 _startPosition;

        [SerializeField] private float _speed;
        [SerializeField] private float _limit;

        private void Awake()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            if (transform.position.y >= _limit) return;

            transform.position += Vector3.up * _speed * Time.deltaTime;
        }

        private void OnDisable()
        {
            transform.position = _startPosition;
        }
    }
}

