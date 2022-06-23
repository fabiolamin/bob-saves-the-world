using UnityEngine;

namespace BSTW.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void SetMovementParameters(float posX, float posY)
        {
            _animator.SetFloat("Pos X", posX);
            _animator.SetFloat("Pos Y", posY);
        }

        public void SetFlyingParameter(bool isFlying)
        {
            _animator.SetBool("IsFlying", isFlying);
        }

        public void SetGroundParameter(bool isOnTheGround)
        {
            _animator.SetBool("IsOnTheGround", isOnTheGround);
        }

        public void SetHitParameter(bool gotHit)
        {
            var hitParamName = "GotHit";

            if (_animator.GetBool(hitParamName) == gotHit) return;

            _animator.SetBool(hitParamName, gotHit);
        }

        public void SetKnockdownParameter(bool isKnocked)
        {
            _animator.SetBool("IsKnocked", isKnocked);
        }

        public void SetAliveParameter(bool isAlive)
        {
            _animator.SetBool("IsAlive", isAlive);
        }
    }
}

