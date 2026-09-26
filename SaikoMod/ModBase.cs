using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SaikoMod.Controller;
using SaikoMod.Core.Components;
using SaikoMod.WinAPI;
using System;
using UnityEngine;

namespace SaikoMod {
    [BepInPlugin(modGUID, "Saiko Mod Menu", modVer)]
    [BepInProcess("Saiko no sutoka.exe")]
    public class ModBase : BaseUnityPlugin {
        public const string modGUID = "Altertoriel.SaikoMod";
        public const string modVer = "0.0.3";

        public static Version Version => new Version(modVer);

        internal static ModBase instance;
        internal ManualLogSource mls;

        public static GameObject manager;
        public static FPSDisplay fpsDisplay;

        internal ConfigEntry<bool> allowChangeWindowTitle;
        public ConfigEntry<bool> showFPSDisplay;
        public ConfigEntry<bool> skipLanguage;

        readonly Harmony harmony = new Harmony(modGUID);

        void Awake() {
            if (!IsGameValid() && WinMessageBox.Show("This version of the Modmenu is intended to be used with \"Saiko No Sutoka\".", WinMessageBox.MBIcon.Error)) {
                Application.Quit();
            }
            instance = this;

            manager = new GameObject("SaikoModMenu");
            manager.AddComponent<UIController>();
            manager.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(manager);

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Mod Loaded Successfully.");
            mls.LogInfo($"Mod Version: {modVer}");

            allowChangeWindowTitle = Config.Bind("General", "Allow Change Window Title", true);
            skipLanguage = Config.Bind("General", "Skip Language Menu", false);
            showFPSDisplay = Config.Bind("Misc", "Show FPS Display", false);

            harmony.PatchAllConditionals();
            if (allowChangeWindowTitle.Value) WindowTitle.SetText($"SaikoMod v{modVer}");

            if (showFPSDisplay.Value) {
                GameObject _fpsDis = new GameObject("FPS_Display");
                fpsDisplay = _fpsDis.AddComponent<FPSDisplay>();
                DontDestroyOnLoad(_fpsDis);
            }
        }

        static bool IsGameValid(string gameName = "Habupain/Saiko no sutoka") {
            return Application.temporaryCachePath.Contains(gameName);
        }
    }
}
