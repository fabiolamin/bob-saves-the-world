using BSTW.Environment;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Player
{
    public class PlayerFoot : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onPlayerFall;
        [SerializeField] private UnityEvent _onPlayerBounce;
        [SerializeField] private ParticleSystem _footStepsVFX;
        public static bool IsOnTheGround { get; private set; } = true;

        private void Update()
        {
            if (PlayerMovement.IsMoving && IsOnTheGround)
            {
                if (_footStepsVFX.isPlaying) return;

                _footStepsVFX.Play();
            }
            else
            {
                if (_footStepsVFX.isStopped) return;

                _footStepsVFX.Stop();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IsOnTheGround = true;

            var bouncySurface = other.GetComponent<BouncySurface>();

            if (bouncySurface != null)
            {
                bouncySurface.OnBounce?.Invoke();
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

