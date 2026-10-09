using UnityEngine;

namespace SaikoMod.Utils {
    public static class GUIUtils {
        public static void DrawField(string label, ref float field, params GUILayoutOption[] options) {
            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label(label);
                field = float.Parse(GUILayout.TextField(field.ToString(format: "0.000"), options));
            }
        }

        public static void DrawField(string label, ref int field, params GUILayoutOption[] options) {
            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label(label);
                field = int.Parse(GUILayout.TextField(field.ToString(), options));
            }
        }

        public static void DrawField(string label, ref string field, params GUILayoutOption[] options) {
            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label(label);
                field = GUILayout.TextField(field.ToString(), options);
            }
        }
    }
}