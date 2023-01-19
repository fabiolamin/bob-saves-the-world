using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BSTW.Utils;
using BSTW.Enemy.AI.States;
using UnityEngine.Events;

namespace BSTW.Enemy.AI
{
    [System.Serializable]
    public struct EnemyTargetPriority
    {
        public int Priority;
        public string TargetTag;
    }

    public class DefaultEnemyAIController : EnemyAIController
    {
        private List<GameObject> _targets = new List<GameObject>();

        [SerializeField] private EnemySight _enemySight;

        public UnityEvent OnMovementStarted;
        public UnityEvent OnMovementFinished;

        public EnemyAIState InvestigateState;
        public EnemyAIState AttackState;
        public EnemyAIState DeathState;

        public EnemyTargetPriority[] TargetPriorities;

        protected override void Start()
        {
            base.Start();

            SwitchState(InvestigateState);
        }

        private bool IsAPriorityTarget(string tag)
        {
            return tag == TargetPriorities.FirstOrDefault(tp => tp.Priority == 1).TargetTag;
        }

        public override void OnHit()
        {
            if (CanAttackPlayerOnHit())
            {
                AddTarget(player);
            }
        }

        private bool CanAttackPlayerOnHit()
        {
            return !AttackState.IsActive && !_enemySight.IsTargetFarAway(player.transform);
        }

        public void RemoveTarget(GameObject target)
        {
            if (TargetPriorities.Any(t => t.TargetTag == target.tag) && _targets.Contains(target))
            {
                CurrentTarget = null;
                _targets.Remove(target);

                if (_targets.Count == 0)
                {
                    SwitchState(InvestigateState);

                    return;
                }

                if (AttackState.IsActive)
                    CurrentTarget = GetNewTarget();
                else
                    AttackTarget();
            }
        }

        public void AddTarget(GameObject newTarget)
        {
            if (TargetPriorities.Any(t => t.TargetTag == newTarget.tag) && !_targets.Contains(newTarget))
            {
                _targets.Add(newTarget);

                if (!AttackState.IsActive)
                    AttackTarget();
            }
        }

        private Health GetNewTarget()
        {
            return _targets.Count == 1 ? _targets[0].GetComponent<Health>() : _targets.
            Where(t => IsAPriorityTarget(t.gameObject.tag)).
                OrderBy(t => Vector3.Distance(t.transform.position, transform.position)).ElementAt(0).GetComponent<Health>();
        }

        private void AttackTarget()
        {
            CurrentTarget = GetNewTarget();

            SwitchState(AttackState);
        }

        public void RestoreEnemy()
        {
            SwitchState(InvestigateState);

            EnemyHealth.RestoreHealth();
        }

        protected override bool IsTargetDead()
        {
            return CurrentTarget != null && !CurrentTarget.IsAlive && AttackState.IsActive && EnemyHealth.IsAlive;
        }
    }
}