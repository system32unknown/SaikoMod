using UnityEngine;
using System.Linq;
using HarmonyLib;
using SaikoMod.Windows;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(ElectricPuzzle))]
    class PuzzleMod {
        static ElectricPuzzle i;
        [HarmonyPatch("Start")]
        static void Postfix(ElectricPuzzle __instance) {
            if (__instance != null) i = __instance;
            OtherUI.patternCode = GetPatterns;
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
        public static string GetPatterns
        {
            get
            {
                if (i == null)
                    return string.Empty;
                else
                    return string.Join(",", i.puzzlePair);
            }
        }
    }
}
