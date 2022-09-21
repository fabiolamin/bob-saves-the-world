using BSTW.Enemy.AI.States;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

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

        public EnemyTargetPriority[] TargetPriorities;

        public EnemyAIState InvestigateState;
        public EnemyAIState AttackState;

        protected override void Start()
        {
            base.Start();

            SwitchState(InvestigateState);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (TargetPriorities.Any(t => t.TargetTag == other.gameObject.tag))
            {
                _targets.Add(other.gameObject);

                SetNewTarget();
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            if (TargetPriorities.Any(t => t.TargetTag == other.gameObject.tag))
            {
                _targets.Remove(other.gameObject);

                if (_targets.Count == 0)
                {
                    SwitchState(InvestigateState);

                    return;
                }

                SetNewTarget();
            }
        }

        private void SetNewTarget()
        {
            var newCurrentTarget = _targets.Count == 1 ? _targets[0] : _targets.
            Where(t => IsAPriorityTarget(t.gameObject.tag)).
                OrderBy(t => Vector3.Distance(t.transform.position, transform.position)).ElementAt(0);

            if (currentState != AttackState)
            {
                CurrentTarget = newCurrentTarget;
                SwitchState(AttackState);
            }
        }

        private bool IsAPriorityTarget(string tag)
        {
            return tag == TargetPriorities.FirstOrDefault(tp => tp.Priority == 1).TargetTag;
        }

        public override void OnHit()
        {
            base.OnHit();

            if (IsAPriorityTarget(player.tag) && currentState != AttackState)
            {
                CurrentTarget = player;
                SwitchState(AttackState);
            }
        }

        public void MoveTowardsArea(float radius, Vector3 center)
        {
            NavMeshHit hit;

            var newPosition = GetArea(radius, center);

            while (!NavMesh.SamplePosition(newPosition, out hit, radius, 1))
            {
                newPosition = GetArea(radius, center);
            }

            NavMeshAgent.SetDestination(hit.position);
        }

        private Vector3 GetArea(float radius, Vector3 center)
        {
            var randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            var x = Mathf.Cos(randomAngle) * radius;
            var z = Mathf.Sin(randomAngle) * radius;

            var newPosition = center + new Vector3(x, 0f, z);

            return newPosition;
        }
    }
}

