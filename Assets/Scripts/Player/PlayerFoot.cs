using System;
using UnityEngine;

namespace BSTW.Player
{
    public class PlayerFoot : MonoBehaviour
    {
        public static bool IsOnTheGround { get; private set; } = true;

        public static event Action OnPlayerFall;

        private void OnTriggerEnter(Collider other)
        {
            OnPlayerFall?.Invoke();
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

