using HarmonyLib;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(YandereController))]
    public class YandModController {
        public static bool notDetected = false;
        
        [HarmonyPatch("isInNpcFOV"), HarmonyPrefix]
        static bool FOVPatch()
        {
            if (notDetected) return false;
            return true;    
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
            if (notDetected) return false;
            return true;
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
            if (notAttacted) return false;
            return true;
        }

        [HarmonyPatch("DistanceReachedPlayerAndAI"), HarmonyPrefix]
        static bool DistanceReachedPlayerAndAIPatch()
        {
            if (noDistanceCheck) return false;
            return true;
        }
    }
}
