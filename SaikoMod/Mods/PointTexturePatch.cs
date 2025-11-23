using HarmonyLib;
using UnityEngine;
using System.Linq;

namespace SaikoMod.Mods {
    [HarmonyPatch(typeof(LanguageMenu))]
    class PointTexturePatch
    {
        [HarmonyPatch("Start")]
        static void Postfix() {
            foreach (Texture2D tex in Resources.FindObjectsOfTypeAll<Texture2D>().Where(x => x.filterMode != FilterMode.Point).ToArray()) tex.filterMode = FilterMode.Point;
        }
    }
}