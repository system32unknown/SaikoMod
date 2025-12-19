using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(MainMenuManager), "Start")]
    internal class MainMenuMod {
        static void Postfix(MainMenuManager __instance) {
            Transform t = __instance.transform.GetChild(0).Find("Text (2)");
            Text text = t.gameObject.GetComponent<Text>();
            RectTransform rect = text.GetComponent<RectTransform>();
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.fontSize = 18;
            text.alignment = TextAnchor.LowerRight;
            text.color = Color.white;
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(1f, 0f);
            text.transform.position = new Vector2(Screen.width, 0f);
            text.text += $"\n<size=16>Mod Version {ModBase.modVer}</size>";
        }
    }
}
