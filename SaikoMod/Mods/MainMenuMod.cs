using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using SaikoMod.Core.Components.UI;
using SaikoMod.Core.Components;

namespace SaikoMod.Mods {
    [HarmonyPatch(typeof(MainMenuManager))]
    internal class MainMenuMod {
        static GameObject loadingScreen;
        static LevelLoader loader;

        [HarmonyPatch("Start"), HarmonyPrefix]
        static void Preload(MainMenuManager __instance) {
            loader = __instance.gameObject.AddComponent<LevelLoader>();
            Transform main_UI = __instance.transform.GetChild(0);
            Transform loading_UI = __instance.transform.GetChild(4);

            Font ArialFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

            #region MAIN MENU
            Transform t = main_UI.Find("Text (2)");
            Text text = t.gameObject.GetComponent<Text>();
            RectTransform rect = text.GetComponent<RectTransform>();
            text.font = ArialFont;
            text.fontSize = 18;
            text.alignment = TextAnchor.LowerRight;
            text.color = Color.white;
            UI.AnchorUtils.SetAnchor(rect, UI.AnchorUtils.AnchorPreset.BottomRight);
            rect.anchoredPosition = Vector2.zero;
            text.text += $"\n<size=16>Mod Version {ModBase.modVer}</size>";

            CustomButton quick = UI.UIHelpers.CreateButton("Quick Start", main_UI, UI.AnchorUtils.AnchorPreset.Left, new Vector2(40f, 50f));
            quick.button.name = "Quick Start";
            quick.button.onClick.AddListener(() => {
                GameData.instance.difficultyChosen = DifficultyChosen.Hard;
                __instance.PlayGame();
            });
            #endregion

            #region LOADING SCREEN
            Text loadTxt = loading_UI.Find("Text (3)").GetComponent<Text>();
            loadTxt.font = ArialFont;
            loadTxt.fontSize = 22;

            loadingScreen = __instance.loadingPage;
            loader.loadingPrefix = "Loading: ";
            loader.loadingText = loadTxt;
            #endregion
        }

        [HarmonyPatch("PlayGame"), HarmonyPrefix]
        static bool LoadPatch() {
            loadingScreen.SetActive(true);
            loader.LoadLevel(1);
            return false;
        }
    }
}
