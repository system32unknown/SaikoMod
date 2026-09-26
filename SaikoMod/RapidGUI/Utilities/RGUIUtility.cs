using UnityEngine;

namespace RapidGUI {
    public static partial class RGUIUtility {
        static readonly GUIContent tempContent = new GUIContent();

        public static GUIContent TempContent(string text) {
            tempContent.text = text;
            tempContent.tooltip = null;
            tempContent.image = null;
            return tempContent;
        }

        public static Vector2 GetMouseScreenPos(Vector2? screenInsideOffset = null) {
            Vector3 mousePos = Input.mousePosition;
            Vector2 ret = new Vector2(mousePos.x, Screen.height - mousePos.y);

            if (screenInsideOffset.HasValue) {
                Vector2 maxPos = new Vector2(Screen.width, Screen.height) - screenInsideOffset.Value;
                ret = Vector2.Min(ret, maxPos);
            }

            return ret;
        }
    }
}