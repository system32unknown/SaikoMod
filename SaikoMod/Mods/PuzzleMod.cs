using UnityEngine;
using System.Linq;
using HarmonyLib;
using SaikoMod.UI;

namespace SaikoMod.Mods {
    [HarmonyPatch(typeof(ElectricPuzzle))]
    class PuzzleMod {
        static ElectricPuzzle i;
        public static bool PuzzleHack = false;

        [HarmonyPatch("Start"), HarmonyPostfix]
        static void PuzzleModInit(ElectricPuzzle __instance) {
            if (__instance != null) i = __instance;
            GameUI.patternCode = GetPatterns;
        }

        [HarmonyPatch("CheckCanContinue"), HarmonyPrefix]
        static bool CheckCanContinuePatch(ref bool __result) {
            if (PuzzleHack) {
                __result = true;
                return false;
            }
            return true;
        }

        public static void SetPuzzle(string digits) {
            if (i == null) return;
            i.puzzlePair = digits.Split(',').Select(new System.Func<string, int>(int.Parse)).ToList().ToArray();

            for (int idx = 0; idx < i.puzzlePair.Length; idx++) {
                int ledIndex = idx / 2;
                ElectricPuzzleSwitch sw = i.switches[i.puzzlePair[idx]];
                sw.switchLed.GetComponent<MeshRenderer>().material = i.puzzleLed[ledIndex];
            }
        }
        public static string GetPatterns {
            get {
                if (i == null) return string.Empty;
                else return string.Join(",", i.puzzlePair);
            }
        }
    }
}
