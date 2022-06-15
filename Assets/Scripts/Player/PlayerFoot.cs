using BSTW.Environment;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Player
{
    public class PlayerFoot : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onPlayerFall;
        [SerializeField] private UnityEvent _onPlayerBounce;
        public static bool IsOnTheGround { get; private set; } = true;

        private void OnTriggerEnter(Collider other)
        {
            IsOnTheGround = true;

            if (other.GetComponent<BouncySurface>() != null)
            {
                _onPlayerBounce?.Invoke();
                return;
            }

            _onPlayerFall?.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            IsOnTheGround = true;
        }

        private void OnTriggerExit(Collider other)
        {
            IsOnTheGround = false;
        }
    }
}

