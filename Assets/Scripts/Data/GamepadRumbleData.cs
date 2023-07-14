using UnityEngine;

namespace BSTW.Data
{
    [CreateAssetMenu(fileName = "Gamepad Rumble Data", menuName = "Data/new Gamepad Rumble Data")]
    public class GamepadRumbleData : ScriptableObject
    {
        [SerializeField] private float _lowFrequency;
        [SerializeField] private float _highFrequency;
        [SerializeField] private float _duration;

        public float LowFrequency => _lowFrequency;
        public float HighFrequency => _highFrequency;
        public float Duration => _duration;
    }
}

