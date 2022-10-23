using UnityEngine;

namespace BSTW.Enemy.AI
{
    public class MeleeEnemyAIController : DefaultEnemyAIController
    {
        [Header("SFX")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _screamSFX;

        [Header("Attack")]
        [SerializeField] private EnemyMeleeAttack _leftHand;
        [SerializeField] private EnemyMeleeAttack _rightHand;

        public void PlayScreamSFX()
        {
            _audioSource.clip = _screamSFX;
            _audioSource.Play();
        }

        public void ActivateRightHandAttack(int isActive)
        {
            _rightHand.gameObject.SetActive(isActive == 1);
        }

        public void ActivateLeftHandAttack(int isActive)
        {
            _leftHand.gameObject.SetActive(isActive == 1);
        }
    }
}

