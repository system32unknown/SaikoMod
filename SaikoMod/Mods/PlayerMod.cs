using HarmonyLib;
using UnityEngine;
using SaikoMod.Helper;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(PlayerController))]
    class PlayerMod
    {
        [HarmonyPatch("GetsNeckBroken"), HarmonyPrefix]
        static bool KillPatch()
        {
            return !HealthMod.noKill;
        }
    }

    [HarmonyPatch(typeof(CameraMotionController))]
    class PlayerCamMod
    {
        [HarmonyPatch("PlayNeckStabAnimation"), HarmonyPrefix]
        static bool KillPatch()
        {
            return !HealthMod.noKill;
        }
    }
}
