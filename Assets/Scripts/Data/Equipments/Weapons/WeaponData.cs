using BSTW.Data.Player;
using UnityEngine;

namespace BSTW.Data.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "new Weapon Data", menuName = "Data/Equipments/new Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private RuntimeAnimatorController _animatorController;
        [SerializeField] private Sprite _icon;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private CameraShakeData _cameraShakeData;
        [SerializeField] private int _maxAmmo = 150;
        [SerializeField] private string[] _targets;
        [SerializeField] private float _bulletDamage = 20f;
        [SerializeField] private float _shootingInterval = 0.5f;
        [SerializeField] private float _shootingDistance = 500f;
        [SerializeField] private bool _isSelected = false;

        public RuntimeAnimatorController AnimatorController => _animatorController;
        public Sprite Icon => _icon;
        public AudioClip AudioClip => _audioClip;
        public CameraShakeData CameraShakeData => _cameraShakeData;
        public int CurrentAmmo { get; set; }
        public int MaxAmmo => _maxAmmo;
        public string[] Targets => _targets;
        public float BulletDamage => _bulletDamage;
        public float ShootingInterval => _shootingInterval;

        public float ShootingDistance => _shootingDistance;
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; } }
    }
}

