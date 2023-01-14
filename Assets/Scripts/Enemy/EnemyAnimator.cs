using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSTW.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private List<RuntimeAnimatorController> _attacks;

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
            if (_attacks.Count > 0)
            {
                SetAnimatorController(_attacks[Random.Range(0, _attacks.Count)]);
            }

            _animator.SetTrigger(attackTrigger);
        }

        public void SetAnimatorController(RuntimeAnimatorController animatorController)
        {
            _animator.runtimeAnimatorController = animatorController;
        }

        public void AddAttackAnimatorController(RuntimeAnimatorController attack)
        {
            if (_attacks.Contains(attack)) return;

            _attacks.Add(attack);
        }
    }
}

