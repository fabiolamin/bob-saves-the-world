using UnityEngine;

namespace BSTW.Data.Player
{
    [CreateAssetMenu(fileName = "Camera Shake Data", menuName = "Data/Player/new Camera Shake Data")]
    public class CameraShakeData : ScriptableObject
    {
        [SerializeField] private float _intensity;
        [SerializeField] private float _timer;

        public float Intensity => _intensity;
        public float Timer => _timer;
    }
}

