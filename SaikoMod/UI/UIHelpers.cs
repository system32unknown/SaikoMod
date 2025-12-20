using UnityEngine.UI;
using UnityEngine;
using System.Linq;

namespace SaikoMod.UI
{
    public static class AnchorUtils
    {
        public enum AnchorPreset
        {
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
        public static void SetAnchor(RectTransform rect, AnchorPreset preset)
        {
            if (rect == null) return;

            switch (preset)
            {
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
    public static class UIHelpers
    {
        public static Button CreateButton(string label, Transform parent, AnchorUtils.AnchorPreset anchor, Vector2 position)
        {
            Button _but = Object.FindObjectsOfType<Button>().Where(x => x.name == "Start").First();
            Button but = Object.Instantiate(_but, parent);
            RectTransform rectT = but.GetComponent<RectTransform>();
            but.transform.GetChild(0).GetComponent<Text>().text = label;
            AnchorUtils.SetAnchor(rectT, anchor);
            rectT.anchoredPosition = position;
            return but;
        }
    }
}
