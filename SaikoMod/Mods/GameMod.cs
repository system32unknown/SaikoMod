using HarmonyLib;
using SaikoMod.Components;
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
            tracker.from = __instance.playerController.yandereController.transform;
            tracker.to = __instance.playerController.transform;

            Image img = Resources.FindObjectsOfTypeAll<Image>().Where(x => x.name == "PausePanel").First();
            img.enabled = false;

            __instance.healthManager.Health = 200f;
            pc = __instance.playerController;
        }

        public static bool EyeEnabled
        {
            get {
                if (pc != null)
                    return pc.cameraMotionController.eyeBlinkAnim.gameObject.activeSelf;
                else return false;
            }
            set {
                if (pc != null) pc.cameraMotionController.eyeBlinkAnim.gameObject.SetActive(value);
            }
        }

        static PlayerController pc;
    }

    [HarmonyPatch(typeof(Tutorial))]
    internal class TutorialMod
    {
        [HarmonyPatch("Start")]
        static void Postfix(Tutorial __instance) {
            __instance.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
