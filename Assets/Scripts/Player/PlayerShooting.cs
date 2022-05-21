using UnityEngine;
using UnityEngine.InputSystem;

namespace BSTW.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        public static bool IsAiming { get; private set; } = false;

        public void OnAim(InputAction.CallbackContext value)
        {
            IsAiming = value.action.IsPressed() && PlayerFoot.IsOnTheGround;
        }
    }
}

