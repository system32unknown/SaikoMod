using HarmonyLib;
using SaikoMod.Core.Components;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(HFPS_GameManager))]
    internal class GameManagerMod {
        [HarmonyPatch("Start")]
        static void Postfix(HFPS_GameManager __instance)
        {
            SaikoTracker tracker = new GameObject("SaikoTracker").AddComponent<SaikoTracker>();

            pc = __instance.playerController;
            tracker.from = pc.yandereController.transform;
            tracker.to = pc.transform;

            Image img = Resources.FindObjectsOfTypeAll<Image>().First(x => x.name == "PausePanel");
            img.enabled = false;

            DynamicObject powerbox = Resources.FindObjectsOfTypeAll<DynamicObject>().First(x => x.name == "locker" && x.transform.parent.name == "Dynamic_ElectricBox");
            powerbox.backUseAnim2 = "PowerBox_Close";

            __instance.healthManager.Health = 200f;
            eyeObject = pc.cameraMotionController.eyeBlinkAnim.gameObject;
        }

        public static bool EyeEnabled
        {
            get {
                if (pc != null)
                    return eyeObject.transform.GetChild(0).gameObject.activeSelf && eyeObject.transform.GetChild(1).gameObject.activeSelf;
                else return false;
            }
            set {
                if (pc != null) {
                    eyeObject.transform.GetChild(0).gameObject.SetActive(value);
                    eyeObject.transform.GetChild(1).gameObject.SetActive(value);
                }
            }
        }

        static PlayerController pc;
        static GameObject eyeObject;
    }

    [HarmonyPatch(typeof(Tutorial), "Start")]
    class TutorialMod {
        static void Postfix(Tutorial __instance) {
            __instance.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(StairDrop), nameof(StairDrop.OnTriggerEnter))]
    class StairPatch {
        static bool Prefix() {
            return !YandModController.noPushing;
        }
    }
}
