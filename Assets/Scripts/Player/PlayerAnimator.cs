using UnityEngine;

namespace BSTW.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void TriggerJumpAnimation()
        {
            _animator.SetTrigger("Jump");    
        }

        public void TriggerRollAnimation()
        {
            _animator.SetTrigger("Roll");
        }

        public void SetMovementAnimation(float posX, float posY)
        {
            _animator.SetFloat("Pos X", posX);
            _animator.SetFloat("Pos Y", posY);
        }

        public void UpdateFlyBool(bool isFlying)
        {
            _animator.SetBool("IsFlying", isFlying);
        }

        public void UpdateGroundBool(bool isOnTheGround)
        {
            _animator.SetBool("IsOnTheGround", isOnTheGround);
        }
    }
}

