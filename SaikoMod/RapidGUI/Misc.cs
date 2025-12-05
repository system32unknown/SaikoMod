using System;
using System.Collections.Generic;
using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        public static void Divider(Color color, float thickness = 1f, float padding = 4f) {
            GUILayout.Space(padding);
            GUI.DrawTexture(GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(thickness)), RGUIStyle.white, ScaleMode.StretchToFill, false, 0, color, 0, 0);
            GUILayout.Space(padding);
        }
        public static void Divider(Color color) {
            GUI.DrawTexture(GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(1f)), RGUIStyle.white, ScaleMode.StretchToFill, false, 0, color, 0, 0);
        }

        public static int Page(int page, int maxPage, bool warped) {
            GUI.backgroundColor = Color.black;
            GUILayout.FlexibleSpace(); // Push JUST this block to bottom

            GUILayout.BeginHorizontal();

            if ((page > 0 || warped) && GUILayout.Button("<", GUILayout.Width(40))) {
                if (page > 0) page--;
                else if (warped) page = maxPage; // wrap to end
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label($"Page {page} / {maxPage}", RGUIStyle.centerLabel);
            GUILayout.FlexibleSpace();

            if ((page < maxPage || warped) && GUILayout.Button(">", GUILayout.Width(40))) {
                if (page < maxPage) page++;
                else if (warped) page = 0; // wrap to beginning
            }

            GUILayout.EndHorizontal();

            return page;
        }

        public static T ArrayNavigator<T>(object items, ref int index, bool warped = true, Func<T, string> labelSelector = null, float buttonWidth = 40f)
        {
            T[] array;
            if (items is T[] arr) {
                array = arr;
            } else if (items is List<T> list) {
                array = list.ToArray();
            } else {
                GUILayout.Label("<b>Error: Not array or list</b>");
                return default;
            }

            GUI.backgroundColor = Color.black;
            // Safety / normalize
            if (items == null || array.Length == 0) {
                index = 0;
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Empty", GUILayout.ExpandWidth(false));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                return default;
            }

            // Clamp index to valid range in case array changed externally
            if (index < 0) index = 0;
            if (index >= array.Length) index = array.Length - 1;

            // Buttons + center label layout
            GUILayout.BeginHorizontal();

            // Left button
            if (GUILayout.Button("<", GUILayout.Width(buttonWidth))) {
                if (index > 0) index--;
                else if (warped) index = array.Length - 1;
            }

            // Center label (center by flexible spaces)
            GUILayout.FlexibleSpace();

            string labelText;
            T current = array[index];
            if (labelSelector != null) labelText = labelSelector(current);
            else labelText = (current != null) ? current.ToString() : "null";

            // show index/count and item label
            GUILayout.Label(string.Format("{0}/{1}  {2}", index + 1, array.Length, labelText), RGUIStyle.centerLabel, GUILayout.ExpandWidth(false));

            GUILayout.FlexibleSpace();

            // Right button
            if (GUILayout.Button(">", GUILayout.Width(buttonWidth))) {
                if (index < array.Length - 1) index++;
                else if (warped) index = 0;
            }

            GUILayout.EndHorizontal();

            return array[index];
        }
    }
}
