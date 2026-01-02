using HarmonyLib;
using System;
using RogoDigital.Lipsync;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(LipSync))]
    public class LipPatches
    {
        public static Action<LipSyncData> onPlay;

        [HarmonyPatch("Play", new Type[] {typeof(LipSyncData)}), HarmonyPostfix]
        static void PlayPatch(LipSyncData dataFile)
        {
            onPlay?.Invoke(dataFile);
        }
    }
}
