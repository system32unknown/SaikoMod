using UnityEngine;
using System.Linq;
using RapidGUI;

namespace SaikoMod.UI {
    public class SettingsUI : BaseWindowUI {
        public override string Title => "Settings";

        bool allPoint = false;
        public void OnLoad()
        {
            if (allPoint) foreach (Texture2D tex in Resources.FindObjectsOfTypeAll<Texture2D>().Where(x => x.filterMode != FilterMode.Point).ToArray()) tex.filterMode = FilterMode.Point;
        }

        public override void Draw()
        {
            allPoint = RGUI.Button(allPoint, "All Points");
        }
    }
}