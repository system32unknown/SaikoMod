using System;
using System.Linq;
using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        static int popupControlId;
        static readonly PopupWindow popupWindow = new PopupWindow();

        public static string SelectionPopup(string current, string[] displayOptions) {
            int idx = Array.IndexOf(displayOptions, current);
            GUILayout.Box(current, RGUIStyle.alignLeftBox);
            int newIdx = PopupOnLastRect(idx, displayOptions);
            if (newIdx != idx) current = displayOptions[newIdx];
            return current;
        }

        public static int SelectionPopup(int selectionIndex, string[] displayOptions) {
            string label = (selectionIndex < 0 || displayOptions.Length <= selectionIndex) ? "" : displayOptions[selectionIndex];
            GUILayout.Box("<b>" + label + "</b>", RGUIStyle.alignLeftBox);
            return PopupOnLastRect(selectionIndex, displayOptions);
        }

        public static int PopupOnLastRect(string[] displayOptions, string label = "") => PopupOnLastRect(-1, displayOptions, -1, label);
        public static int PopupOnLastRect(string[] displayOptions, int button, string label = "") => PopupOnLastRect(-1, displayOptions, button, label);

        public static int PopupOnLastRect(int selectionIndex, string[] displayOptions, int mouseButton = -1, string label = "") => Popup(GUILayoutUtility.GetLastRect(), mouseButton, selectionIndex, displayOptions, label);

        public static int Popup(Rect launchRect, int mouseButton, int selectionIndex, string[] displayOptions, string label = "") {
            int ret = selectionIndex;
            int controlId = GUIUtility.GetControlID(FocusType.Passive);

            // not Popup Owner
            if (popupControlId != controlId) {
                Event ev = Event.current;
                Vector2 pos = ev.mousePosition;

                if ((ev.type == EventType.MouseUp) && ((mouseButton < 0) || (ev.button == mouseButton)) && launchRect.Contains(pos) && displayOptions != null && displayOptions.Any()) {
                    popupWindow.pos = RGUIUtility.GetMouseScreenPos(Vector2.one * 150f);
                    popupControlId = controlId;
                    ev.Use();
                }
            } else { // Active
                EventType type = Event.current.type;

                int? result = popupWindow.result;
                if (result.HasValue && type == EventType.Layout) {
                    if (result.Value >= 0) ret = result.Value; // -1 when the popup is closed by clicking outside the window
                    popupWindow.result = null;
                    popupControlId = 0;
                } else {
                    if ((type == EventType.Layout) || (type == EventType.Repaint)) {
                        GUIStyle buttonStyle = RGUIStyle.popupFlatButton;
                        Vector2 contentSize = Vector2.zero;
                        for (int i = 0; i < displayOptions.Length; ++i) {
                            Vector2 textSize = buttonStyle.CalcSize(RGUIUtility.TempContent(displayOptions[i]));
                            contentSize.x = Mathf.Max(contentSize.x, textSize.x);
                            contentSize.y += textSize.y;
                        }

                        RectOffset margin = buttonStyle.margin;
                        contentSize.y += Mathf.Max(0, displayOptions.Length - 1) * Mathf.Max(margin.top, margin.bottom); // is this right?

                        GUIStyle vbarSkin = GUI.skin.verticalScrollbar;
                        Vector2 vbarSize = vbarSkin.CalcScreenSize(Vector2.zero);
                        RectOffset vbarMargin = vbarSkin.margin;

                        GUIStyle hbarSkin = GUI.skin.horizontalScrollbar;
                        Vector2 hbarSize = hbarSkin.CalcScreenSize(Vector2.zero);
                        RectOffset hbarMargin = hbarSkin.margin;

                        const float offset = 5f;
                        contentSize += new Vector2(vbarSize.x + vbarMargin.horizontal, hbarSize.y + hbarMargin.vertical) + Vector2.one * offset;
                        Vector2 size = RGUIStyle.popup.CalcScreenSize(contentSize);
                        Vector2 maxSize = new Vector2(Screen.width, Screen.height) - popupWindow.pos;

                        popupWindow.size = Vector2.Min(size, maxSize);
                    }

                    popupWindow.label = label;
                    popupWindow.displayOptions = displayOptions;
                    WindowInvoker.Add(popupWindow);
                }
            }

            return ret;
        }

        class PopupWindow : IDoGUIWindow {
            public string label;
            public Vector2 pos;
            public Vector2 size;
            public int? result;
            public string[] displayOptions;
            public Vector2 scrollPosition;

            static readonly int PopupWindowId = "Popup".GetHashCode();

            public Rect GetWindowRect() => new Rect(pos, size);

            public void DoGUIWindow() {
                GUI.ModalWindow(PopupWindowId, GetWindowRect(), (id) => {
                    using (GUILayout.ScrollViewScope sc = new GUILayout.ScrollViewScope(scrollPosition)) {
                        scrollPosition = sc.scrollPosition;

                        for (int j = 0; j < displayOptions.Length; ++j) {
                            if (GUILayout.Button(displayOptions[j], RGUIStyle.popupFlatButton)) {
                                result = j;
                            }
                        }
                    }

                    Event ev = Event.current;
                    if ((ev.rawType == EventType.MouseDown) && !(new Rect(Vector2.zero, size).Contains(ev.mousePosition))) {
                        result = -1;
                    }
                }, label, RGUIStyle.popup);
            }

            public void CloseWindow() => result = -1;
        }
    }
}