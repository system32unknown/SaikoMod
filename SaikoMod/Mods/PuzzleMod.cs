using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(ElectricPuzzle))]
    class PuzzleMod {
        static ElectricPuzzle i;
        [HarmonyPatch("Start")]
        static void Postfix(ElectricPuzzle __instance) {
            if (__instance != null) i = __instance;
        }

        public static void SetPuzzle(string digits)
        {
            i.puzzlePair = digits.Split(',').Select(new System.Func<string, int>(int.Parse)).ToList<int>().ToArray();
        }
    }
}
