using UnityEngine;

namespace BSTW.Player
{
    public class PlayerFoot : MonoBehaviour
    {
        public static bool IsOnTheGround { get; private set; } = true;

        private void OnTriggerEnter(Collider other)
        {
            IsOnTheGround = true;
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

