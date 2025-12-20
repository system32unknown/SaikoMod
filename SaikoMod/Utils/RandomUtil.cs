using UnityEngine;
using System.Text;

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
        public static T RandomEnum<T>() where T : System.Enum
        {
            System.Array values = System.Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Range(0, values.Length));
        }
        public static string GetString(int max, bool includeSpace = false, int chance = 50)
        {
            StringBuilder tempStr = new StringBuilder();

            for (int i = 0; i < max; i++) {
                char randomChar = (char)Random.Range(65, 123);
                tempStr.Append(randomChar);

                if (includeSpace && Random.Range(0, 100) < chance) tempStr.Append('\n');
            }

            return tempStr.ToString();
        }
        public static void ShuffleCurve(AnimationCurve[] curves, float vRange = 2f, float outT = 10f, float inT = 10f) {
            foreach (AnimationCurve curve in curves)
            {
                for (int i = 0; i < curve.keys.Length; i++)
                {
                    Keyframe k = curve.keys[i];
                    k.value = Random.Range(-vRange, vRange);
                    k.outTangent = Random.Range(0f, outT);
                    k.inTangent = Random.Range(0f, inT);
                    curve.MoveKey(i, k);
                }
            }
        }
    }
}
