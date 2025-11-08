using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using SaikoMod.Mods;
using UnityEngine;
using SaikoMod.Controller;

namespace SaikoMod
{
    [BepInPlugin(modGUID, modName, modVer)]
    public class ModBase : BaseUnityPlugin
    {
        const string modGUID = "Altertoriel.SaikoMod";
        const string modName = "Saiko Mod Menu";
        const string modVer = "0.0.0.1";

        public static Version Version => new Version(modVer);

        internal static ModBase i;
        internal ManualLogSource mls;

        public static GameObject manager;

        void Awake() {
            i = this;
            Harmony harmony = new Harmony(modGUID);

            manager = new GameObject("SaikoModMenu");
            manager.AddComponent<UIController>();
            UnityEngine.Object.DontDestroyOnLoad(manager);

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Mod Loaded Successfully.");
            mls.LogInfo($"Mod Version: {modVer}");

            harmony.PatchAllConditionals();
        }
    }
}
