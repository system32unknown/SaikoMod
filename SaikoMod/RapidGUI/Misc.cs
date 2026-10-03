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
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            if ((page > 0 || warped) && GUILayout.Button("<", GUILayout.Width(40))) {
                if (page > 0) page--;
                else if (warped) page = maxPage;
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label($"Page {page} / {maxPage}", RGUIStyle.centerLabel);
            GUILayout.FlexibleSpace();

            if ((page < maxPage || warped) && GUILayout.Button(">", GUILayout.Width(40))) {
                if (page < maxPage) page++;
                else if (warped) page = 0;
            }

            GUILayout.EndHorizontal();

            return page;
        }

        public static bool ArrayNavigator<T>(ref int index, object collection, string label = null) {
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

            GUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label)) GUILayout.Label("<b>" + label + "</b>", GUILayout.Width(120f));

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("<", RGUIStyle.button, GUILayout.Width(30f))) {
                index--;
                if (index < 0) index = array.Length - 1;
            }

            string text = "Null";
            if (array[index] != null) text = SaikoMod.Helper.ReflectionHelpers.GetNameIfExists(array[index]);

            if (GUILayout.Button("<b>" + text + "</b>", GUILayout.Width(200f))) {
                clickedCenter = true;
            }

            if (GUILayout.Button(">", GUILayout.Width(30f))) {
                index++;
                if (index >= array.Length) index = 0;
            }
            GUILayout.EndHorizontal();

            return clickedCenter;
        }
        public static T ArrayNavigator<T>(object items, ref int index, bool warped = true, Func<T, string> labelSelector = null) {
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
            if (items == null || array.Length == 0) {
                index = 0;
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Empty", GUILayout.ExpandWidth(false));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                return default;
            }

            index = Mathf.Clamp(index, 0, array.Length - 1);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("<", GUILayout.Width(30f))) {
                if (index > 0) index--;
                else if (warped) index = array.Length - 1;
            }
            GUILayout.FlexibleSpace();

            string labelText;
            T current = array[index];
            if (labelSelector != null) labelText = labelSelector(current);
            else labelText = (current != null) ? current.ToString() : "null";
            GUILayout.Label(string.Format("{0}/{1} {2}", index + 1, array.Length, labelText), RGUIStyle.centerLabel, GUILayout.ExpandWidth(false));

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(">", GUILayout.Width(30f))) {
                if (index < array.Length - 1) index++;
                else if (warped) index = 0;
            }
            GUILayout.EndHorizontal();

            return array[index];
        }
    }
}
