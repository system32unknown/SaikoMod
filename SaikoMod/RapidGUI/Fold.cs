using Action = System.Action;
using UnityEngine;

namespace RapidGUI {
    public class Fold {
        public bool active = false;
        public Action foldAction;
        public string text;

        public Fold(string text) => this.text = text;

        public bool DoFold() {
            using (new GUILayout.HorizontalScope()) {
                active = DoGUIHeader(active, text);
                foldAction?.Invoke();
            }
            return active;
        }

        public static bool DoGUIHeader(bool isOpen, string name, params GUILayoutOption[] options) {
            isOpen ^= GUILayout.Button((isOpen ? "▼" : "▶") + name, Style.Fold, options);
            return isOpen;
        }

        public static class Style {
            public static readonly GUIStyle Fold;

            static Style() {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                GUIStyle toggle = GUI.skin.toggle;
                style.normal.textColor = toggle.normal.textColor;
                style.hover.textColor = toggle.hover.textColor;
                style.margin.left = 0;

                Texture2D tex = new Texture2D(1, 1);
                tex.SetPixels(new[] { new Color(0.5f, 0.5f, 0.5f, 0.5f) });
                tex.Apply();
                style.hover.background = tex;

                Fold = style;
            }
        }
    }
}