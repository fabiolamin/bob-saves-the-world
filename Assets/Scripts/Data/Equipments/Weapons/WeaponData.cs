using BSTW.Data.Player;
using BSTW.Utils;
using UnityEngine;

namespace BSTW.Data.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "new Weapon Data", menuName = "Data/Equipments/new Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private RuntimeAnimatorController _animatorController;
        [SerializeField] private Sprite _icon;
        [SerializeField] private AudioClip _shootSFX;
        [SerializeField] private AudioClip _reloadSFX;
        [SerializeField] private CameraShakeData _cameraShakeData;
        [SerializeField] private int _maxAmmo = 150;
        [Tooltip("Decimal value.")][SerializeField] private float _criticalAmmoPercentage = 0.3f;
        [SerializeField] private string[] _targets;
        [SerializeField] private Hit _hitDamage;
        [SerializeField] private float _shootingInterval = 0.5f;
        [SerializeField] private float _shootingDistance = 500f;
        [SerializeField] private bool _isSelected = false;

        public RuntimeAnimatorController AnimatorController => _animatorController;
        public Sprite Icon => _icon;
        public AudioClip ShootSFX => _shootSFX;
        public AudioClip ReloadSFX => _reloadSFX;
        public CameraShakeData CameraShakeData => _cameraShakeData;
        public int CurrentAmmo { get; set; }
        public int MaxAmmo => _maxAmmo;
        public float CriticalAmmoPercentage => _criticalAmmoPercentage;
        public string[] Targets => _targets;
        public Hit HitDamage => _hitDamage;
        public float ShootingInterval => _shootingInterval;

        public float ShootingDistance => _shootingDistance;
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; } }
        public bool CanLoadWeapon { get { return CurrentAmmo < MaxAmmo; } }
    }
}

