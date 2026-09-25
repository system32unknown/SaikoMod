using RapidGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using FPSCounter = SaikoMod.Core.Components.FPSDisplay;
using FPSUtils = SaikoMod.Utils.FPSUtils;

namespace SaikoMod.UI {
    public class SettingsUI : BaseWindowUI {
        public override string Title => "Settings";
        int selMenu = 0;

        FPSUtils fpsUtils;

        GameObject[] windowLights;
        GameObject eyeObj;
        CameraBloodEffect bloodEffect;
        bool windowLightEnabled = true;

        OperatingSystem os;

        public void OnLoad() {
            if (SceneManager.GetActiveScene().name == "LevelNew") {
                windowLights = Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name.Contains("SHW_Add_effect_r") && x.activeSelf).ToArray();
                windowLightEnabled = true;
            }

            eyeObj = GameObject.Find("GAMEMANAGER/Canvas/UI/Eye");
            bloodEffect = UnityEngine.Object.FindObjectOfType<CameraBloodEffect>();
            os = Environment.OSVersion;
        }

        public void OnUnload() {
            if (SceneManager.GetActiveScene().name == "LevelNew") windowLights = null;
        }

        public override void Draw() {
            if (fpsUtils == null) fpsUtils = ModBase.fpsDisplay.fps;

            selMenu = GUILayout.SelectionGrid(selMenu, new string[] { "General", "Stats", "Optimize" }, 3);
            switch (selMenu) {
                case 0: // General
                    if (ModBase.instance.showFPSDisplay.Value) FPSCounter.lagMode = RGUI.Field(FPSCounter.lagMode, "Lag Display Mode");
                    break;
                case 1: // Stats
                    if (fpsUtils != null) {
                        GUILayout.BeginVertical("Box");
                        GUILayout.Label("Framerate");
                        GUILayout.Label($"curFPS:{fpsUtils.CurFPS} / Total:{fpsUtils.TotalFPS}\nTarget:{fpsUtils.TargetFPS + (fpsUtils.ClampFPS ? " (Clamped)": "")}");
                        GUILayout.EndVertical();
                    }

                    GUILayout.BeginVertical("Box");
                    GUILayout.Label("System");
                    GUILayout.Label($"Platform: {os.VersionString}\nVersion: {Application.version} / Unity Ver: {Application.unityVersion}");
                    GUILayout.EndVertical();
                    break;
                case 2: // Optimize
                    if (eyeObj && RGUI.Button(eyeObj.activeSelf, "Vignette")) eyeObj.SetActive(!eyeObj.activeSelf);
                    if (GUILayout.Button("Optimize")) {
                        ForcePointFilter(Resources.FindObjectsOfTypeAll<Texture2D>());
                        ForcePointFilter(Resources.FindObjectsOfTypeAll<Texture>());
                        ForcePointFilter(Resources.FindObjectsOfTypeAll<RenderTexture>());

                        foreach (Light light in Resources.FindObjectsOfTypeAll<Light>()) light.shadows = LightShadows.Hard;
                        QualitySettings.shadows = ShadowQuality.HardOnly;
                        QualitySettings.shadowResolution = ShadowResolution.Low;
                        QualitySettings.antiAliasing = 0;
                    }
                    if (windowLights != null && RGUI.Button(windowLightEnabled, "Window Lights")) {
                        windowLightEnabled = !windowLightEnabled;
                        foreach (GameObject window in windowLights) window.SetActive(windowLightEnabled);
                    }
                    if (bloodEffect != null && RGUI.Button(bloodEffect.enabled, "Blood FX")) bloodEffect.enabled = !bloodEffect.enabled;
                    break;
            }
        }

        void ForcePointFilter<T>(IEnumerable<T> textures) where T : Texture {
            foreach (Texture tex in textures) if (tex.filterMode != FilterMode.Point) tex.filterMode = FilterMode.Point;
        }
    }
}