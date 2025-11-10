using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(MainMenuManager)), HarmonyPatch("Start")]
    internal class MainMenuMod {
        static void Postfix(MainMenuManager __instance) {
            Transform t = __instance.transform.GetChild(0).Find("Text (2)");
            Text text = t.gameObject.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.fontSize = 18;
            text.text += $"\n<size=16>Mod Version {ModBase.modVer}</size>";
            text.color = Color.white;
        }
    }
}
