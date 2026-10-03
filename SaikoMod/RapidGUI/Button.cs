using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        public static bool Button(bool v, string label, params GUILayoutOption[] options) {
            GUI.backgroundColor = Color.black;

            using (new GUILayout.VerticalScope(options))
            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label("<b>" + label + "</b>");
                v = GUILayout.Button("<b>" + (v ? "On" : "Off") + "</b>", RGUIStyle.button, GUILayout.Width(260f));
            }

            return v;
        }
    }
}