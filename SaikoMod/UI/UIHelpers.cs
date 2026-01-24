using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using SaikoMod.Core.Components.UI;

namespace SaikoMod.UI {
    public static class AnchorUtils {
        public enum AnchorPreset {
            TopLeft,
            Top,
            TopRight,
            Left,
            MiddleCenter,
            Right,
            BottomLeft,
            Bottom,
            BottomRight
        }

        /// <summary>
        /// Sets anchor, pivot, and anchoredPosition using an enum.
        /// </summary>
        public static void SetAnchor(RectTransform rect, AnchorPreset preset) {
            if (rect == null) return;

            switch (preset) {
                case AnchorPreset.TopLeft:
                    rect.anchorMin = new Vector2(0f, 1f);
                    rect.anchorMax = new Vector2(0f, 1f);
                    rect.pivot = new Vector2(0f, 1f);
                    break;

                case AnchorPreset.Top:
                    rect.anchorMin = new Vector2(0.5f, 1f);
                    rect.anchorMax = new Vector2(0.5f, 1f);
                    rect.pivot = new Vector2(0.5f, 1f);
                    break;

                case AnchorPreset.TopRight:
                    rect.anchorMin = new Vector2(1f, 1f);
                    rect.anchorMax = new Vector2(1f, 1f);
                    rect.pivot = new Vector2(1f, 1f);
                    break;

                case AnchorPreset.Left:
                    rect.anchorMin = new Vector2(0f, 0.5f);
                    rect.anchorMax = new Vector2(0f, 0.5f);
                    rect.pivot = new Vector2(0f, 0.5f);
                    break;

                case AnchorPreset.MiddleCenter:
                    rect.anchorMin = new Vector2(0.5f, 0.5f);
                    rect.anchorMax = new Vector2(0.5f, 0.5f);
                    rect.pivot = new Vector2(0.5f, 0.5f);
                    break;

                case AnchorPreset.Right:
                    rect.anchorMin = new Vector2(1f, 0.5f);
                    rect.anchorMax = new Vector2(1f, 0.5f);
                    rect.pivot = new Vector2(1f, 0.5f);
                    break;

                case AnchorPreset.BottomLeft:
                    rect.anchorMin = new Vector2(0f, 0f);
                    rect.anchorMax = new Vector2(0f, 0f);
                    rect.pivot = new Vector2(0f, 0f);
                    break;

                case AnchorPreset.Bottom:
                    rect.anchorMin = new Vector2(0.5f, 0f);
                    rect.anchorMax = new Vector2(0.5f, 0f);
                    rect.pivot = new Vector2(0.5f, 0f);
                    break;

                case AnchorPreset.BottomRight:
                    rect.anchorMin = new Vector2(1f, 0f);
                    rect.anchorMax = new Vector2(1f, 0f);
                    rect.pivot = new Vector2(1f, 0f);
                    break;
            }
        }
    }

    public static class UIHelpers {
        public static CustomButton CreateButton(string label, Transform parent, AnchorUtils.AnchorPreset anchor, Vector2 position) {
            Button but = Object.Instantiate(Object.FindObjectsOfType<Button>().First(x => x.name == "Start"), parent);
            CustomButton button = new CustomButton(but);
            button.Label.text = label;
            AnchorUtils.SetAnchor(button.Rect, anchor);
            button.Label.fontSize = 22;
            button.position = position;
            return button;
        }
    }
}
