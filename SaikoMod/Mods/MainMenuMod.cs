using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SaikoMod.Mods
{
    [HarmonyPatch(typeof(MainMenuManager), "Start")]
    internal class MainMenuMod {
        static void Postfix(MainMenuManager __instance) {
            Transform main_UI = __instance.transform.GetChild(0);

            Transform t = main_UI.Find("Text (2)");
            Text text = t.gameObject.GetComponent<Text>();
            RectTransform rect = text.GetComponent<RectTransform>();
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.fontSize = 18;
            text.alignment = TextAnchor.LowerRight;
            text.color = Color.white;
            UI.AnchorUtils.SetAnchor(rect, UI.AnchorUtils.AnchorPreset.BottomRight);
            rect.anchoredPosition = Vector2.zero;
            text.text += $"\n<size=16>Mod Version {ModBase.modVer}</size>";

            Button quick = UI.UIHelpers.CreateButton("QuickPlay", main_UI, UI.AnchorUtils.AnchorPreset.Left, new Vector2(40f, 50f));
            quick.onClick.RemoveAllListeners();
            quick.onClick.AddListener(() => {
                GameData.instance.difficultyChosen = DifficultyChosen.Hard;
                __instance.PlayGame();
            });
        }
    }
}
