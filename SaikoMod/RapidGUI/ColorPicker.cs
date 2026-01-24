using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        public static Color ColorPicker(Color color, string label = null, bool hasHex = false) {
            GUI.backgroundColor = Color.white;
            GUILayout.BeginVertical("box");
            if (label != null) GUILayout.Label(label);

            // SLIDERS
            color.r = _Slider("R", color.r);
            color.g = _Slider("G", color.g);
            color.b = _Slider("B", color.b);
            color.a = _Slider("A", color.a);

            // HEX INPUT
            if (hasHex) {
                string hex = ColorUtility.ToHtmlStringRGBA(color);
                string newHex = GUILayout.TextField(hex, GUILayout.Width(120));

                if (newHex != hex && newHex.Length == 8) {
                    if (ColorUtility.TryParseHtmlString("#" + newHex, out Color c)) color = c;
                }
            }
            GUILayout.EndVertical();

            return color;
        }

        public static Color ColorPickerOne(Color color, string label = null) {
            GUI.backgroundColor = Color.white;
            GUILayout.BeginVertical("box");
            if (label != null) GUILayout.Label(label);

            float v = GUILayout.HorizontalSlider(color.r, 0f, 1f);
            color.r = v;
            color.g = v;
            color.b = v;
            color.a = v;

            GUILayout.EndVertical();

            return color;
        }

        static float _Slider(string name, float value) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name, GUILayout.Width(15));
            value = GUILayout.HorizontalSlider(value, 0f, 1f);
            GUILayout.Label(((int)(value * 255)).ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();
            return value;
        }
    }

}