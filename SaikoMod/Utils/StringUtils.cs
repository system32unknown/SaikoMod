namespace SaikoMod.Utils {
    class StringUtils {
		public static string FormatBytes(float Bytes, int Precision = 2) {
			string[] units = {"Bytes", "kB", "MB", "GB", "TB", "PB"};
			int curUnit = 0;
			while (Bytes >= 1024 && curUnit < units.Length - 1) {
				Bytes /= 1024;
				curUnit++;
			}
			return MathUtils.RoundDecimal(Bytes, Precision) + units[curUnit];
		}
    }
}
