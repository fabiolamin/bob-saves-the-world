using TMPro;
using UnityEngine;

namespace BSTW.Utils
{
    public class HitPoints : MonoBehaviour
    {
        private Vector3 _defaultScale;
        private Color _defaultColor;
        private Vector3 _starPosition;
        private float _durationAux = 0f;
        private bool _isActive;
        private Vector3 _randomDirection;

        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private Vector2[] _directions;
        [SerializeField] private float _duration;
        [SerializeField] private float _speed;
        [SerializeField] private float _maxDistance = 5f;

        [Header("High Damage")]
        [SerializeField] private float _highDamageValue = 40f;
        [SerializeField] private float _scaleIncrementOnHighDamage;
        [SerializeField] private Color _colorOnHighDamage;

        private void Awake()
        {
            _defaultColor = _textMeshPro.color;
            _defaultScale = _textMeshPro.transform.localScale; 
        }

        private void Update()
        {
            MoveHitPoints();
        }

        private void MoveHitPoints()
        {
            if (_isActive)
            {
                _durationAux += Time.deltaTime;

                transform.LookAt(Camera.main.transform);

                if (Vector3.Distance(_starPosition, transform.position) < _maxDistance)
                {
                    transform.Translate(_randomDirection * _speed * Time.deltaTime);
                }

                if (_durationAux >= _duration)
                {
                    _durationAux = 0f;
                    _isActive = false;
                    gameObject.SetActive(false);
                }
            }
        }

        public void Activate(Hit hit)
        {
            if (!_isActive)
            {
                int damage = (int)hit.Damage;
                _textMeshPro.text = damage.ToString();
                _randomDirection = _directions[Random.Range(0, _directions.Length)];
                _starPosition = transform.position;

                _textMeshPro.transform.localScale = _defaultScale;
                _textMeshPro.color = _defaultColor;

                if (hit.Damage >= _highDamageValue)
                {
                    _textMeshPro.transform.localScale += Vector3.one * _scaleIncrementOnHighDamage;
                    _textMeshPro.color = _colorOnHighDamage;
                }
                _isActive = true;
            }
        }
    }

}
