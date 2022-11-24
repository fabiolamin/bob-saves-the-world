using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSTW.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private RuntimeAnimatorController[] _attacks;

        public void SetMovementParameter(float speed)
        {
            _animator.SetFloat("Speed Float", speed);
        }

        public float GetCurrentAnimationDuration()
        {
            return _animator.GetCurrentAnimatorClipInfo(0).Length;
        }

        public void TriggerAnimationAttack(string attackTrigger = "Attack")
        {
            if(_attacks.Length > 0)
            {
                _animator.runtimeAnimatorController = _attacks[Random.Range(0, _attacks.Length)];
            }

            _animator.SetTrigger(attackTrigger);
        }
    }
}

