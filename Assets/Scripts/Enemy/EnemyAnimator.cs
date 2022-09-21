using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSTW.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void SetMovementParameter(bool isMoving)
        {
            _animator.SetBool("IsMoving", isMoving);
        }

        public float GetCurrentAnimationDuration()
        {
            return _animator.GetCurrentAnimatorClipInfo(0).Length;
        }
    }
}

