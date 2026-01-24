using UnityEngine;
using System.Text;

namespace SaikoMod.Utils {
    class StringUtils {
        public static string FormatBytes(float Bytes, int Precision = 2) {
            string[] units = { "Bytes", "kB", "MB", "GB", "TB", "PB" };
            int curUnit = 0;
            while (Bytes >= 1024 && curUnit < units.Length - 1) {
                Bytes /= 1024;
                curUnit++;
            }
            return MathUtils.RoundDecimal(Bytes, Precision) + units[curUnit];
        }

        /// <summary>
        /// Pads a numeric value on the left with a specific character until it reaches a fixed digit count.
        /// Example: FillNumber(7, 3, '0') -> "007"
        /// </summary>
        public static string FillNumber(float value, int digits, char padChar) {
            string str = value.ToString();
            int len = str.Length;

            if (len >= digits) return str;

            StringBuilder sb = new StringBuilder(digits);
            for (int i = 0; i < digits - len; i++) sb.Append(padChar);
            sb.Append(str);
            return sb.ToString();
        }

        /// <summary>
        /// Formats time (seconds) into a readable string:
        /// "mm:ss", "h:mm:ss", "Xd Xh Xm Xs", or "Xw Xd Xh Xm Xs"
        /// Matches your original Haxe logic.
        /// </summary>
        public static string FormatTime(float time, int precision = 0, int timePre = 0) {
            int totalSeconds = Mathf.FloorToInt(time);

            string secs = (totalSeconds % 60).ToString();
            string mins = (totalSeconds / 60 % 60).ToString();
            string hour = (totalSeconds / 3600 % 24).ToString();
            string days = (totalSeconds / 86400 % 7).ToString();
            string weeks = (totalSeconds / (86400 * 7)).ToString();

            if (secs.Length < 2) secs = "0" + secs;

            string formatted = $"{mins}:{secs}";

            // When there are hours but no days
            if (hour != "0" && days == "0") {
                if (mins.Length < 2) mins = "0" + mins;
                formatted = $"{hour}:{mins}:{secs}";
            }

            if (days != "0" && weeks == "0") formatted = $"{days}d {hour}h {mins}m {secs}s"; // Days but no weeks
            if (weeks != "0") formatted = $"{weeks}w {days}d {hour}h {mins}m {secs}s"; // Full format including weeks

            // Decimal precision (sub-second)
            if (precision > 0) {
                float secondsForMS = time % 60f;
                formatted += ".";

                int fraction = (int)((secondsForMS - Mathf.Floor(secondsForMS)) * precision);
                formatted += FillNumber(fraction, timePre, '0');
            }

            return formatted;
        }
    }
}
