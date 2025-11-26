using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        public static void Divider(Color color, float thickness = 1f, float padding = 4f) {
            GUILayout.Space(padding);
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(thickness));
            GUI.DrawTexture(rect, RGUIStyle.white, ScaleMode.StretchToFill, false, 0, color, 0, 0);
            GUILayout.Space(padding);
        }

        public static void Divider(Color color) {
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(1f));
            GUI.DrawTexture(rect, RGUIStyle.white, ScaleMode.StretchToFill, false, 0, color, 0, 0);
        }

        public static int Page(int page, int maxPage, bool warped) {
            GUILayout.FlexibleSpace(); // Push JUST this block to bottom

            GUILayout.BeginHorizontal();

            // LEFT BUTTON (“<”)
            if (GUILayout.Button("<", GUILayout.Width(40))) {
                if (page > 0) page--;
                else if (warped) page = maxPage; // wrap to end
            }

            // CENTER LABEL
            GUILayout.FlexibleSpace();
            GUILayout.Label($"Page {page} / {maxPage}");
            GUILayout.FlexibleSpace();

            // RIGHT BUTTON (“>”)
            if (GUILayout.Button(">", GUILayout.Width(40))) {
                if (page < maxPage) page++;
                else if (warped) page = 0; // wrap to beginning
            }

            GUILayout.EndHorizontal();

            return page;
        }
    }
}
