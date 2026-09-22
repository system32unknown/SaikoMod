using HarmonyLib;
using SaikoMod.Core.Enums;
using UnityEngine;

namespace SaikoMod.Mods {
    [HarmonyPatch(typeof(PlayerController))]
    class PlayerMod {
        [HarmonyPatch(nameof(PlayerController.GetsNeckBroken)), HarmonyPrefix]
        static bool KillPatch() {
            return !(HealthMod.godModeType == GodModeType.Kill || HealthMod.godModeType == GodModeType.All || HealthMod.godModeType == GodModeType.AllNoQuick);
        }

        public static bool allowJump = false;
        public static bool infJump = false;

        [HarmonyPatch("Update"), HarmonyPrefix]
        public static void Prefix(ref Vector3 ___moveDirection, ref bool ___grounded, KeyCode ___JumpKey, float ___jumpSpeed) {
            if (allowJump && (___grounded || infJump) && Input.GetKeyDown(___JumpKey)) {
                ___moveDirection.y = ___jumpSpeed;
                ___grounded = false;
            }
        }
    }

    [HarmonyPatch(typeof(CameraMotionController))]
    class PlayerCamMod {
        [HarmonyPatch(nameof(CameraMotionController.PlayNeckBreakAnimation)), HarmonyPrefix]
        static bool KillPatch() {
            return !(HealthMod.godModeType == GodModeType.Kill || HealthMod.godModeType == GodModeType.All || HealthMod.godModeType == GodModeType.AllNoQuick);
        }
    }
}