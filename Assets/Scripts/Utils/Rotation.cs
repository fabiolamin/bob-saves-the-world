using UnityEngine;

namespace BSTW.Utils
{
    public class Rotation : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 10f;

        private void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime, Space.World);
        }

        public void SetRotatationSpeed(float speed)
        {
            _rotationSpeed = speed;
        }
    }
}

