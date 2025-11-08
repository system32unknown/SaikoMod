using HarmonyLib;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(HealthManager))]
    public class HealthMod
    {
        public static bool noDamage = false;
        public static bool noKill = false;

        [HarmonyPatch("ApplyBleedDamage"), HarmonyPrefix]
        static bool ApplyBleedDamagePatch()
        {
            if (noDamage) return false;
            return true;
        }

        [HarmonyPatch("ApplyDamage"), HarmonyPrefix]
        static bool ApplyDamagePatch()
        {
            if (noDamage) return false;
            return true;
        }

        [HarmonyPatch("Kill"), HarmonyPrefix]
        static bool KillPatch()
        {
            if (noKill) return false;
            return true;
        }
    }
}
