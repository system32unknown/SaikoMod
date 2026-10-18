using HarmonyLib;
using System;

namespace SaikoMod.Mods {
    [HarmonyPatch(typeof(HealthManager))]
    internal class HealthMod {
        public static bool godMode = false;

        [HarmonyPatch(nameof(HealthManager.ApplyBleedDamage), new Type[] { typeof(float) }), HarmonyPrefix]
        static bool ApplyBleedDamagePatch() => !godMode;

        [HarmonyPatch(nameof(HealthManager.ApplyDamage), new Type[] { typeof(float) }), HarmonyPrefix]
        static bool ApplyDamagePatch() => !godMode;

        [HarmonyPatch(nameof(HealthManager.Kill)), HarmonyPrefix]
        static bool KillPatch() => !godMode;
    }
}