using HarmonyLib;
using SaikoMod.Core.Enums;
using UnityEngine;

namespace SaikoMod.Mods {
    [HarmonyPatch(typeof(YandereController))]
    internal class YandModController {
        public static bool notDetected = false;
        public static bool notAlerted = false;
        public static bool notChoking = false;
        public static SaikoLookMode lookMode = SaikoLookMode.None;

        static Transform[] transforms;

        [HarmonyPatch("Start"), HarmonyPostfix]
        static void InitYandController()
        {
            transforms = Resources.FindObjectsOfTypeAll(typeof(Transform)) as Transform[];
        }

        [HarmonyPatch("isInNpcFOV"), HarmonyPrefix]
        static bool FOVPatch()
        {
            return !notDetected;    
        }

        [HarmonyPatch("PlayerFoundDetection"), HarmonyPrefix]
        static bool PlayerFoundDetectionPatch()
        {
            return !notDetected;
        }

        [HarmonyPatch("AlertToPlayerPosition"), HarmonyPrefix]
        static bool AlertToPlayerPositionPatch()
        {
            return !notAlerted;
        }

        [HarmonyPatch("AtemptKidnapPlayer"), HarmonyPrefix]
        static bool AtemptKidnapPlayerPatch()
        {
            return !notChoking;
        }

        [HarmonyPatch("ChokePlayer"), HarmonyPrefix]
        static bool ChokePlayerPatch()
        {
            return !notChoking;
        }

        [HarmonyPatch("KillPlayerFromFront"), HarmonyPrefix]
        static bool KillPlayerFromFrontPatch()
        {
            return !HealthMod.noKill;
        }

        [HarmonyPatch("Update"), HarmonyPostfix]
        static void PostUpdate(YandereController __instance)
        {
            switch (lookMode)
            {
                case SaikoLookMode.Player:
                    __instance.lookAtIK.solver.target = __instance.playerHead;
                    break;
                case SaikoLookMode.Random:
                    __instance.lookAtIK.solver.target = transforms[UnityEngine.Random.Range(0, transforms.Length)];
                    break;
            }
        }
    }

    [HarmonyPatch(typeof(YandereAI))]
    public class YandModAI
    {
        public static bool notAttacted = false;
        public static bool noDistanceCheck = false;

        [HarmonyPatch("PlayerCanDetectAI"), HarmonyPrefix]
        static bool PlayerCanDetectPatch()
        {
            return !YandModController.notDetected;
        }

        [HarmonyPatch("DetectPlayerLookingDown"), HarmonyPrefix]
        static bool DetectPlayerLookingDownPatch()
        {
            return !YandModController.notDetected;
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
