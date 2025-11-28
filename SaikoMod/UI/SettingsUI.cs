using UnityEngine;

namespace SaikoMod.UI {
    public class SettingsUI : BaseWindowUI {
        public override string Title => "Settings";
        public override Color BgColor => Color.blue;
        public override void Draw()
        {
            GUILayout.Label("Settings go here...");
        }
    }
}