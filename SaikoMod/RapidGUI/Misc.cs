using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        public static void Divider(Color color, float thickness = 1f, float padding = 4f) {
            GUILayout.Space(padding);

            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none,
                GUILayout.Height(thickness));

            GUI.DrawTexture(rect, RGUIStyle.white, ScaleMode.StretchToFill, false, 0, color, 0, 0);

            GUILayout.Space(padding);
        }
    }
}
