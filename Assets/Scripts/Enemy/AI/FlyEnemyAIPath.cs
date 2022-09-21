using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSTW.Enemy.AI
{
    public class FlyEnemyAIPath : MonoBehaviour
    {
        [SerializeField] private float _waypointGizmosRadius = 0.5f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), _waypointGizmosRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextWaypointIndex(i)));
                Gizmos.color = Color.blue;
            }
        }

        public int GetNextWaypointIndex(int i)
        {
            return i == (transform.childCount - 1) ? 0 : (i + 1);
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}

