using UnityEngine;
using FPSCounter = SaikoMod.Core.Components.FPSDisplay;
using System.Collections.Generic;
using RapidGUI;

namespace SaikoMod.UI {
    public class SettingsUI : BaseWindowUI {
        public override string Title => "Settings";

        bool allPoint = false;

        public void OnLoad() {
            if (allPoint) {
                ForcePointFilter(Resources.FindObjectsOfTypeAll<Texture2D>());
                ForcePointFilter(Resources.FindObjectsOfTypeAll<RenderTexture>());
            }
        }

        public override void Draw() {
            if (RGUI.Button(allPoint, "All Points")) allPoint = !allPoint;
            if (ModBase.instance.showFPSDisplay.Value) FPSCounter.lagMode = RGUI.Field(FPSCounter.lagMode, "Lag Mode");
        }

        void ForcePointFilter<T>(IEnumerable<T> textures) where T : Texture {
            foreach (Texture tex in textures) if (tex.filterMode != FilterMode.Point) tex.filterMode = FilterMode.Point;
        }
    }
}