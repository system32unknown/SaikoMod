using HarmonyLib;
using SaikoMod.Core.Enums;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(PlayerController))]
    class PlayerMod
    {
        [HarmonyPatch("GetsNeckBroken"), HarmonyPrefix]
        static bool KillPatch()
        {
            return !(HealthMod.godModeType == GodModeType.Kill || HealthMod.godModeType == GodModeType.All || HealthMod.godModeType == GodModeType.AllNoQuick);
        }
    }

    [HarmonyPatch(typeof(CameraMotionController))]
    class PlayerCamMod
    {
        [HarmonyPatch("PlayNeckStabAnimation"), HarmonyPrefix]
        static bool KillPatch()
        {
            return !(HealthMod.godModeType == GodModeType.Kill || HealthMod.godModeType == GodModeType.All || HealthMod.godModeType == GodModeType.AllNoQuick);
        }
    }
}
