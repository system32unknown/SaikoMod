using HarmonyLib;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(HealthManager))]
    internal class HealthMod
    {
        public static bool noDamage = false;
        public static bool noKill = false;

        [HarmonyPatch("ApplyBleedDamage"), HarmonyPrefix]
        static bool ApplyBleedDamagePatch()
        {
            return !noDamage;
        }

        [HarmonyPatch("ApplyDamage"), HarmonyPrefix]
        static bool ApplyDamagePatch()
        {
            return !noDamage;
        }

        [HarmonyPatch("Kill"), HarmonyPrefix]
        static bool KillPatch()
        {
            return !noKill;
        }
    }
}
