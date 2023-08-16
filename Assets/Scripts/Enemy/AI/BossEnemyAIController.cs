using BSTW.Enemy.AI.States;
using BSTW.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace BSTW.Enemy.AI
{
    public class BossEnemyAIController : EnemyAIController
    {
        private List<EnemyMeleeAttack> _attacks = new List<EnemyMeleeAttack>();

        private bool _isMainAttackActive;
        private bool _canActivateMainAttack => EnemyHealth.CurrentHealth <= (_mainAttackHealthPercentage * EnemyHealth.MaxHealth);

        [Header("States")]
        [SerializeField] private EnemyAIState _defaultAttackState;
        [SerializeField] private EnemyAIState _deathState;

        [Header("Attack")]
        [SerializeField] private EnemyMeleeAttack _centerMeleeAttack;
        [SerializeField] private EnemyMeleeAttack _leftMeleeAttack;
        [SerializeField] private EnemyMeleeAttack _rightMeleeAttack;
        [SerializeField] private UnityEvent _onMainAttackActivated;

        [SerializeField] private float _mainAttackHealthPercentage = 0.5f;

        public NavMeshAgent NavMeshAgent;

        protected override void Start()
        {
            base.Start();

            CurrentTarget = player.GetComponent<Health>();

            SwitchState(_defaultAttackState);

            _attacks.Add(_leftMeleeAttack);
            _attacks.Add(_rightMeleeAttack);
        }

        protected override void Update()
        {
            base.Update();

            if (NavMeshAgent != null)
            {
                EnemyAnimator.SetMovementParameter(NavMeshAgent.velocity.magnitude);
            }
        }

        public override void OnHit()
        {
            CheckForMainAttack();
        }

        private void CheckForMainAttack()
        {
            if (_canActivateMainAttack && !_isMainAttackActive)
            {
                _isMainAttackActive = true;

                _onMainAttackActivated.Invoke();

                _attacks.Add(_centerMeleeAttack);
                EnemyAnimator.AddAttackAnimatorController(_centerMeleeAttack.AnimatorController);
            }
        }

        protected override bool IsTargetDead()
        {
            return CurrentTarget != null && !CurrentTarget.IsAlive && EnemyHealth.IsAlive;
        }

        public void ActivateCenterAttack(int isActive)
        {
            _centerMeleeAttack.gameObject.SetActive(isActive == 1);
        }

        public void ActivateRightHandAttack(int isActive)
        {
            _rightMeleeAttack.gameObject.SetActive(isActive == 1);
        }

        public void ActivateLeftHandAttack(int isActive)
        {
            _leftMeleeAttack.gameObject.SetActive(isActive == 1);
        }

        public bool IsNearTarget(Vector3 target, float minDistance)
        {
            var distanceFromTarget = Vector3.Distance(transform.position, target);
            return distanceFromTarget <= minDistance;
        }

        public void StopNavMeshAgent(bool isStopped)
        {
            if (!NavMeshAgent.isOnNavMesh) return;

            NavMeshAgent.isStopped = isStopped;
        }
    }
}

