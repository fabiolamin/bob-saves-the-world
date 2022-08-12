using UnityEngine;

namespace BSTW.Utils
{
    public static class Utilities
    {
        public static float GetValueBasedOnDistace(Vector3 origin, Vector3 target, float valueDistanceConstant, float maxValue, float maxDistance)
        {
            var distance = Vector3.Distance(origin, target);

            if (distance > maxDistance) return 0f;

            var value = valueDistanceConstant * (1 / distance);
            var newValue = Mathf.Clamp(value, 0f, maxValue);

            return newValue;
        }
    }
}

