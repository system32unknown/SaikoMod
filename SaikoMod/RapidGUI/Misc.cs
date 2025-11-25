using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        public static void Divider() {
            GUILayout.Box(RGUIStyle.white, RGUIStyle.line, new GUILayoutOption[0]);
        }
    }
}
