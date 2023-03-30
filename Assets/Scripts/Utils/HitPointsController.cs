using UnityEngine;

namespace BSTW.Utils
{
    public class HitPointsController : MonoBehaviour
    {
        [SerializeField] private ObjectPooling _hitPointsPooling;

        public void OnHit(Hit hit)
        {
            if (hit.Damage == 0f) return;    

            var hitPoints = _hitPointsPooling.GetObject().GetComponent<HitPoints>();
            hitPoints.transform.rotation = Quaternion.identity;
            hitPoints.transform.position = transform.position;
            hitPoints.Activate(hit);
        }

    }
}
