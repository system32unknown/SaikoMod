using UnityEngine;

namespace SaikoMod.Utils {
    class MathUtils {
        /// <summary>
        /// Round a decimal number to have reduced precision (less decimal numbers).
        ///
        /// Example:
        /// roundDecimal(1.2485f, 2) = 1.25f
        /// </summary>
        /// <param name="Value">Any number.</param>
        /// <param name="Precision">Number of decimals the result should have.</param>
        /// <returns>The rounded value of that number.</returns>
        public static float RoundDecimal(float value, int precision) {
            float mult = 1f;
            for (int _ = 0; _ < precision; _++) mult *= 10f;
            return Mathf.Round(value * mult) / mult;
        }

        /// <summary>
        /// Bound a number by a minimum and maximum. Ensures that this number is
        /// no smaller than the minimum, and no larger than the maximum.
        /// Leaving a bound null means that side is unbounded.
        /// </summary>
        /// <param name="Value">Any number.</param>
        /// <param name="Min">Any number or null.</param>
        /// <param name="Max">Any number or null.</param>
        /// <returns>The bounded value of the number.</returns>
        public static float Bound(float value, float? min = null, float? max = null) {
            float lowerBound = (min.HasValue && value < min.Value) ? min.Value : value;
            return (max.HasValue && lowerBound > max.Value) ? max.Value : lowerBound;
        }

        public static float Normalize(float x, float min, float max, bool isBound = true) {
            return isBound ? Bound((x - min) / (max - min), 0, 1) : (x - min) / (max - min);
        }
    }
}
