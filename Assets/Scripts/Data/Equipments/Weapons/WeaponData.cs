using UnityEngine;

namespace BSTW.Data.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "new Weapon Data", menuName = "Data/Equipments/new Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private RuntimeAnimatorController _animatorController;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _maxAmmo = 150;
        [SerializeField] private string _targetName;
        [SerializeField] private float _bulletDamage = 20f;
        [SerializeField] private float _shootingInterval = 0.5f;
        [SerializeField] private float _shootingDistance = 500f;
        [SerializeField] private bool _isSelected = false;

        public RuntimeAnimatorController AnimatorController => _animatorController;
        public Sprite Icon => _icon;
        public int CurrentAmmo { get; set; }
        public int MaxAmmo => _maxAmmo;
        public string TargetName => _targetName;
        public float BulletDamage => _bulletDamage;
        public float ShootingInterval => _shootingInterval;

        public float ShootingDistance => _shootingDistance;
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; } }
    }
}

