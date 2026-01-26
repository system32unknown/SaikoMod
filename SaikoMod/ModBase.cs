using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using UnityEngine;
using SaikoMod.Controller;
using SaikoMod.Core.Components;
using SaikoMod.WinAPI;

namespace SaikoMod {
    [BepInPlugin(modGUID, "Saiko Mod Menu", modVer)]
    [BepInProcess("Saiko no sutoka.exe")]
    public class ModBase : BaseUnityPlugin {
        public const string modGUID = "Altertoriel.SaikoMod";
        public const string modVer = "0.0.0.3";

        public static Version Version => new Version(modVer);

        internal static ModBase instance;
        internal ManualLogSource mls;

        public static GameObject manager;

        internal ConfigEntry<bool> allowChangeWindowTitle;
        public ConfigEntry<bool> showFPSDisplay;

        readonly Harmony harmony = new Harmony(modGUID);

        void Awake() {
            if (!IsGameValid() && WinMessageBox.Show("This version of the Modmenu is intended to be used with \"Saiko No Sutoka\".", WinMessageBox.MBIcon.Error)) {
                Application.Quit();
            }
            instance = this;

            manager = new GameObject("SaikoModMenu");
            manager.AddComponent<UIController>();
            DontDestroyOnLoad(manager);

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Mod Loaded Successfully.");
            mls.LogInfo($"Mod Version: {modVer}");

            allowChangeWindowTitle = Config.Bind("Misc", "Allow Change Window Title", true);
            showFPSDisplay = Config.Bind("Misc", "Show FPS Display", false);

            harmony.PatchAllConditionals();

            if (allowChangeWindowTitle.Value)
                WindowTitle.SetText(Application.productName + " (Modded)");

            if (showFPSDisplay.Value) {
                GameObject fpsDisplay = new GameObject("FPS_Display");
                fpsDisplay.AddComponent<FPSDisplay>();
                DontDestroyOnLoad(fpsDisplay);
            }
        }

        void OnDestroy() {
            harmony.UnpatchSelf();
        }

        static bool IsGameValid(string gameName = "Habupain/Saiko no sutoka") {
            return Application.temporaryCachePath.Contains(gameName);
        }
    }
}
