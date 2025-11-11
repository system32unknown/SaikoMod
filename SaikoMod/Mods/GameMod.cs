using HarmonyLib;
using SaikoMod.Components;
using System;
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
        }

        public static bool EyeEnabled
        {
            get { return HFPS_GameManager.instance.playerController.cameraMotionController.eyeBlinkAnim.gameObject.activeSelf; }
            set
            {
                HFPS_GameManager.instance.playerController.cameraMotionController.eyeBlinkAnim.gameObject.SetActive(value);
            }
        }
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
