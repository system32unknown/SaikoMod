using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using SaikoMod.Mods;
using UnityEngine;
using SaikoMod.Controller;
using BepInEx.Configuration;

namespace SaikoMod
{
    [BepInPlugin(modGUID, "Saiko Mod Menu", modVer)]
    public class ModBase : BaseUnityPlugin
    {
        public const string modGUID = "Altertoriel.SaikoMod";
        public const string modVer = "0.0.0.2";

        public static Version Version => new Version(modVer);

        internal static ModBase instance;
        internal ManualLogSource mls;

        public static GameObject manager;

        internal ConfigEntry<bool> allowChangeWindowTitle;

        void Awake() {
            instance = this;
            Harmony harmony = new Harmony(modGUID);

            manager = new GameObject("SaikoModMenu");
            manager.AddComponent<UIController>();
            UnityEngine.Object.DontDestroyOnLoad(manager);

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Mod Loaded Successfully.");
            mls.LogInfo($"Mod Version: {modVer}");

            allowChangeWindowTitle = Config.Bind("Misc", "Allow Change Window Title", false);

            harmony.PatchAllConditionals();

            if (allowChangeWindowTitle.Value)
                WindowTitle.SetText(Application.productName + " (Modded)");
        }
    }
}
