using UnityEngine;
using FPSCounter = SaikoMod.Core.Components.FPSDisplay;
using FPSUtils = SaikoMod.Utils.FPSUtils;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using RapidGUI;

namespace SaikoMod.UI {
    public class SettingsUI : BaseWindowUI {
        public override string Title => "Settings";

        bool allPoint = false;
        int selMenu = 0;

        FPSUtils fpsUtils;

        GameObject[] windowLights;
        bool windowLightEnabled = true;

        public void OnLoad() {
            if (allPoint) {
                ForcePointFilter(Resources.FindObjectsOfTypeAll<Texture2D>());
                ForcePointFilter(Resources.FindObjectsOfTypeAll<RenderTexture>());
            }

            if (SceneManager.GetActiveScene().name == "LevelNew") {
                windowLights = Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name.Contains("SHW_Add_effect_r") && x.activeSelf).ToArray();
                windowLightEnabled = true;
            }
        }

        public override void Draw() {
            if (fpsUtils == null) fpsUtils = ModBase.fpsDisplay.fps;

            selMenu = GUILayout.SelectionGrid(selMenu, new string[] { "General", "Stats" }, 2);
            switch (selMenu) {
                case 0:
                    if (RGUI.Button(allPoint, "All Points")) allPoint = !allPoint;
                    if (windowLights != null && RGUI.Button(windowLightEnabled, "Window Light Enabled")) {
                        windowLightEnabled = !windowLightEnabled;
                        foreach (GameObject window in windowLights) window.SetActive(windowLightEnabled);
                    }
                    if (ModBase.instance.showFPSDisplay.Value) FPSCounter.lagMode = RGUI.Field(FPSCounter.lagMode, "Lag Mode");
                    break;
                case 1:
                    if (fpsUtils == null) return;
                    GUILayout.Label($"curFPS:{fpsUtils.CurFPS} / Total: {fpsUtils.TotalFPS}\nclamped:{fpsUtils.ClampFPS}\nTarget:{fpsUtils.TargetFPS}");
                    break;
            }
        }

        void ForcePointFilter<T>(IEnumerable<T> textures) where T : Texture {
            foreach (Texture tex in textures) if (tex.filterMode != FilterMode.Point) tex.filterMode = FilterMode.Point;
        }
    }
}