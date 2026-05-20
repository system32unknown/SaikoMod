using UnityEngine;

namespace SaikoMod.Utils {
    public static class GUIUtils {
        public static void DrawField(string label, ref float field, params GUILayoutOption[] options) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            field = float.Parse(GUILayout.TextField(field.ToString(format: "0.000"), options));
            GUILayout.EndHorizontal();
        }

        public static void DrawField(string label, ref int field, params GUILayoutOption[] options) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            field = int.Parse(GUILayout.TextField(field.ToString(), options));
            GUILayout.EndHorizontal();
        }

        public static void DrawField(string label, ref string field, params GUILayoutOption[] options) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            GUILayout.TextField(field.ToString(), options);
            GUILayout.EndHorizontal();
        }
    }
}
