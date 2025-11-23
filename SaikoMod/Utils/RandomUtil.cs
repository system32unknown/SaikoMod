using UnityEngine;

namespace SaikoMod.Utils {
    class RandomUtil {
        public static Vector3 GetVector(float min = 0f, float max = .3f)
        {
            return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        }
        public static Vector3 GetVectorRange(float value)
        {
            return new Vector3(Random.Range(-value, value), Random.Range(-value, value), Random.Range(-value, value));
        }
        public static Color GetColor(bool includeAlpha = false)
        {
            return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), includeAlpha ? Random.Range(0f, 1f) : 1f);
        }
        public static bool GetBool(float chance = 50f)
        {
            return Random.Range(0, 100) < chance;
        }
    }
}
