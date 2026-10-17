using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SaikoMod.Mods {
    [HarmonyPatch(typeof(InteractManager))]
    static class InteractManagerMod {
        public static bool ForceUnlock = false;

        [HarmonyPatch(nameof(InteractManager.Interact)), HarmonyPrefix]
        static void Interact_Prefix(GameObject __0) {
            if (!ForceUnlock || !__0) return;

            DynamicObject door = __0.GetComponent<DynamicObject>();
            if (!door) door = __0.GetComponent<DynamicNode>()?.door;
            UnlockDoor(door, new HashSet<DynamicObject>());
        }

        static void UnlockDoor(DynamicObject door, HashSet<DynamicObject> visited) {
            if (!door || !visited.Add(door)) return;

            if (door.isLocked) {
                door.isLocked = false;
                door.FoundLocked = false;
            }
            UnlockDoor(door.attachedDoor, visited);
        }

        [HarmonyPatch(typeof(DynamicObject), nameof(DynamicObject.CheckHasKey)), HarmonyPostfix]
        static void CheckHasKeyPatch(DynamicObject __instance, ref bool __result) {
            if (ForceUnlock && __instance && __instance.isLocked) __result = true;
        }
    }
}