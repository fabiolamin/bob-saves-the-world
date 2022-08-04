using System.Collections;
using UnityEngine;

namespace BSTW.Equipments.Weapons.Shooting
{
    public class BulletShell : MonoBehaviour
    {
        private bool _canRotate;

        [SerializeField] private AudioClip[] _audioClips;
        [SerializeField] private Rigidbody _rb;

        [SerializeField] private float _force = 10f;
        [SerializeField] private float _minRotationDegree = 45f;
        [SerializeField] private float _maxRotationDegree = 180f;
        [SerializeField] private float _disableDelay = 3f;

        public Transform Weapon { get; set; }

        private void FixedUpdate()
        {
            RotateBulletShell();
        }

        private void OnCollisionEnter(Collision collision)
        {
            _canRotate = false;
            PlayBulletShellSFX();
            StartCoroutine(DisableBulletShell());
        }

        private void OnEnable()
        {
            _canRotate = true;
        }

        private void RotateBulletShell()
        {
            if (!_canRotate) return;

            var rotation = Quaternion.Euler(
            Random.Range(_minRotationDegree, _maxRotationDegree),
            0f,
            Random.Range(_minRotationDegree, _maxRotationDegree));

            _rb.MoveRotation(rotation * _rb.rotation);
        }

        private void PlayBulletShellSFX()
        {
            if (_audioClips.Length > 0)
            {
                var randomAudioClip = _audioClips[Random.Range(0, _audioClips.Length)];
                AudioSource.PlayClipAtPoint(randomAudioClip, transform.position);
            }
        }

        private IEnumerator DisableBulletShell()
        {
            yield return new WaitForSeconds(_disableDelay);

            gameObject.SetActive(false);
        }

        public void MoveBulletShell()
        {
            _rb.AddForce(Weapon.root.right * _force);
        }
    }
}

