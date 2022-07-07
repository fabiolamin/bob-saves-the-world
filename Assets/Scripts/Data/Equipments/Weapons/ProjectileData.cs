using UnityEngine;

namespace BSTW.Data.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "new Projectile Data", menuName = "Data/Equipments/new Projectile Data")]
    public class ProjectileData : ScriptableObject
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _hitRadius = 25f;
        [SerializeField] private float _hitForce = 500f;
        [SerializeField] private float _hitUpwards = 100f;

        public float Speed => _speed;
        public float HitRadius => _hitRadius;
        public float HitForce => _hitForce;
        public float HitUpwards => _hitUpwards;
    }

}
