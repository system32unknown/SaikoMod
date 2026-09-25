using HarmonyLib;
using SaikoMod.Core.Enums;
using System;
using UnityEngine;

namespace SaikoMod.Mods {
    [HarmonyPatch(typeof(YandereController))]
    internal class YandModController {
        public static bool noDetect = false;
        public static bool noAlert = false;
        public static bool noChoke = false;
        public static bool noBadEnding = false;
        public static bool noPushing = false;
        public static SaikoLookMode lookMode = SaikoLookMode.None;

        static Transform[] transforms;

        [HarmonyPatch("Start"), HarmonyPostfix]
        static void InitYandController() {
            transforms = Resources.FindObjectsOfTypeAll<Transform>();
        }

        [HarmonyPatch("isInNpcFOV"), HarmonyPrefix]
        static bool FOVPatch() => !noDetect;

        [HarmonyPatch(nameof(YandereController.PlayerFoundDetection)), HarmonyPrefix]
        static bool PlayerFoundDetectionPatch() => !noDetect;

        [HarmonyPatch(nameof(YandereController.AlertToPlayerPosition), new Type[] { typeof(bool), typeof(bool) }), HarmonyPrefix]
        static bool AlertToPlayerPositionPatch() => !noAlert;

        [HarmonyPatch(nameof(YandereController.AtemptKidnapPlayer)), HarmonyPrefix]
        static bool AtemptKidnapPlayerPatch() => !noChoke;

        [HarmonyPatch(nameof(YandereController.ChokePlayer)), HarmonyPrefix]
        static bool ChokePlayerPatch() => !noChoke;

        [HarmonyPatch(nameof(YandereController.SpawnAtGameIntroPos)), HarmonyPrefix]
        static bool SpawnAtGameIntroPosPatch() => !noBadEnding;

        [HarmonyPatch(nameof(YandereController.KillPlayerFromFront)), HarmonyPrefix]
        static bool KillPlayerFromFrontPatch() {
            return !(HealthMod.godModeType == GodModeType.Kill || HealthMod.godModeType == GodModeType.All || HealthMod.godModeType == GodModeType.AllNoQuick);
        }

        [HarmonyPatch(nameof(YandereController.PushPlayerDown), new Type[] { typeof(bool) }), HarmonyPrefix]
        static bool PushPlayerDownPatch() => !noPushing;

        [HarmonyPatch(nameof(YandereController.LookThroughWindow), new Type[] { typeof(AIRoom) }), HarmonyPrefix]
        static bool LookThroughWindowPatch() => !noDetect;

        [HarmonyPatch("stabbing", MethodType.Enumerator), HarmonyPrefix]
        static bool StabPatch() {
            return !(HealthMod.godModeType == GodModeType.DamageNoQuick || HealthMod.godModeType == GodModeType.AllNoQuick);
        }

        [HarmonyPatch("Update"), HarmonyPostfix]
        static void PostUpdate(YandereController __instance) {
            switch (lookMode) {
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
    public class YandModAI {
        public static bool notAttacted = false;
        public static bool noDistanceCheck = false;
        public static bool customEye = false;

        [HarmonyPatch("FixedUpdate"), HarmonyPrefix]
        static bool FixedUpdatePatch() => !customEye;

        [HarmonyPatch(nameof(YandereAI.PlayerCanDetectAI)), HarmonyPrefix]
        static bool PlayerCanDetectPatch() => !YandModController.noDetect;

        [HarmonyPatch(nameof(YandereAI.DetectPlayerLookingDown)), HarmonyPrefix]
        static bool DetectPlayerLookingDownPatch() => !YandModController.noDetect;

        [HarmonyPatch(nameof(YandereAI.PlayerAttactAI)), HarmonyPrefix]
        static bool PlayerAttactAIPatch() => !notAttacted;

        [HarmonyPatch(nameof(YandereAI.DistanceReachedPlayerAndAI), new Type[] { typeof(float) }), HarmonyPrefix]
        static bool DistanceReachedPlayerAndAIPatch() => !noDistanceCheck;
    }

    [HarmonyPatch(typeof(YandereMoodController))]
    internal class YandModMood {
        [HarmonyPatch(nameof(YandereMoodController.CanExitAtemptKidnap)), HarmonyPrefix]
        static bool CanExitAtemptKidnapPatch()=> !YandModController.noBadEnding;
    }
}