using UnityEngine;
using SaikoMod.Controller;
using SaikoMod.Core.Interfaces;

namespace SaikoMod.UI
{
    public abstract class BaseWindowUI : IWindowUI
    {
        public virtual Color BgColor => Color.black;
        public abstract string Title { get; }
        public abstract void Draw();

        public virtual void OnClose() {
            // Default behavior: close the menu panel
            UIController.Instance.MenuTab = Core.Enums.MenuTab.Off;
        }

        public void WindowLayout(int _)
        {
            GUI.backgroundColor = BgColor;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            DrawTitleBar();
            Draw();
        }

        void DrawTitleBar() {
            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>" + Title + "</b>", GUILayout.Height(21f));
            if (GUILayout.Button("<b>X</b>", GUILayout.Height(19f), GUILayout.Width(32f))) OnClose();
            GUILayout.EndHorizontal();
        }
    }
}
