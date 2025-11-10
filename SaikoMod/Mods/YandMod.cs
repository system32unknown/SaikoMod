using HarmonyLib;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(YandereController))]
    internal class YandModController {
        public static bool notDetected = false;
        public static bool lookingatPlayer = false;
        
        [HarmonyPatch("isInNpcFOV"), HarmonyPrefix]
        static bool FOVPatch()
        {
            return !notDetected;    
        }

        [HarmonyPatch("Update"), HarmonyPostfix]
        static void PostUpdate(YandereController __instance) {
            if (lookingatPlayer)
            {
                __instance.lookAtIK.solver.target = __instance.playerHead;
            }
        }
    }

    [HarmonyPatch(typeof(YandereAI))]
    public class YandModAI
    {
        public static bool notDetected = false;
        public static bool notAttacted = false;
        public static bool noDistanceCheck = false;

        [HarmonyPatch("PlayerCanDetectAI"), HarmonyPrefix]
        static bool PlayerCanDetectPatch()
        {
            return !notDetected;
        }

        [HarmonyPatch("DetectPlayerLookingDown"), HarmonyPrefix]
        static bool DetectPlayerLookingDownPatch()
        {
            if (notDetected) return false;
            return true;
        }

        [HarmonyPatch("PlayerAttactAI"), HarmonyPrefix]
        static bool PlayerAttactAIPatch()
        {
            return !notAttacted;
        }

        [HarmonyPatch("DistanceReachedPlayerAndAI"), HarmonyPrefix]
        static bool DistanceReachedPlayerAndAIPatch()
        {
            return !noDistanceCheck;
        }
    }
}
