using HarmonyLib;
using SaikoMod.Core.Components;
using System.Linq;
using UnityEngine;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(CCTVCameraSystem))]
    class CCTVMod {
        [HarmonyPatch("Start"), HarmonyPostfix]
        static void AddMoreCam(ref CCTVCameraSystem __instance) {
            Camera SaikoCamera = CustomCam.GetHead("yandere").Find("POV").GetComponent<Camera>();
            Debug.Log("POV Cam? " + SaikoCamera.ToString());
            __instance.cctvCams.Append(SaikoCamera).ToArray();
            __instance.activeCams.Append(false).ToArray();
        }
    }
}
