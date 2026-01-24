using UnityEngine;
using System.Collections.Generic;
using SaikoMod.Helper;

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

        public static bool ArrayNavigatorButton<T>(ref int index, object collection, string label = null, params GUILayoutOption[] options) {
            T[] array;
            if (collection is T[] arr) {
                array = arr;
            } else if (collection is List<T> list) {
                array = list.ToArray();
            } else {
                GUILayout.Label("<b>Error: Not array or list</b>");
                return false;
            }

            if (array == null || array.Length == 0) {
                GUILayout.Label("<b>No Items</b>");
                return false;
            }

            GUI.backgroundColor = Color.black;

            bool clickedCenter = false;

            using (new GUILayout.VerticalScope(options))
            using (new GUILayout.HorizontalScope()) {
                if (!string.IsNullOrEmpty(label))
                    GUILayout.Label("<b>" + label + "</b>", GUILayout.Width(120f));

                // Left Button
                if (GUILayout.Button("<b><=</b>", RGUIStyle.button, GUILayout.Width(30f))) {
                    index--;
                    if (index < 0) index = array.Length - 1;
                }

                // Center button (shows selected item)
                string text = "Null";
                if (array[index] != null) text = ReflectionHelpers.GetNameIfExists(array[index]);

                if (GUILayout.Button("<b>" + text + "</b>", GUILayout.Width(200f))) {
                    clickedCenter = true;
                }

                // Right Button
                if (GUILayout.Button("<b>=></b>", GUILayout.Width(30f))) {
                    index++;
                    if (index >= array.Length) index = 0;
                }
            }

            return clickedCenter;
        }
    }
}