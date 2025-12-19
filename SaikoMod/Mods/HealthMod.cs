using HarmonyLib;
using SaikoMod.Core.Enums;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(HealthManager))]
    internal class HealthMod
    {
        public static GodModeType godModeType = GodModeType.None;

        [HarmonyPatch("ApplyBleedDamage"), HarmonyPrefix]
        static bool ApplyBleedDamagePatch()
        {
            return !(godModeType == GodModeType.Damage || godModeType == GodModeType.DamageNoQuick || godModeType == GodModeType.All || godModeType == GodModeType.AllNoQuick);
        }

        [HarmonyPatch("ApplyDamage"), HarmonyPrefix]
        static bool ApplyDamagePatch()
        {
            return !(godModeType == GodModeType.Damage || godModeType == GodModeType.DamageNoQuick || godModeType == GodModeType.All || godModeType == GodModeType.AllNoQuick);
        }

        [HarmonyPatch("Kill"), HarmonyPrefix]
        static bool KillPatch()
        {
            return !(godModeType == GodModeType.Kill || godModeType == GodModeType.All || godModeType == GodModeType.AllNoQuick);
        }
    }
}
