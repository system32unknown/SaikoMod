using HarmonyLib;
using SaikoMod.Core.Components;
using SaikoMod.Core.Components.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SaikoMod.Mods {
    [HarmonyPatch(typeof(MainMenuManager))]
    internal class MainMenuMod {
        static GameObject loadingScreen;
        static LevelLoader loader;

        [HarmonyPatch("Start"), HarmonyPrefix]
        static void Preload(MainMenuManager __instance) {
            loader = __instance.gameObject.AddComponent<LevelLoader>();
            Transform main_UI = __instance.transform.GetChild(0);
            Transform choosedifficulty_UI = __instance.transform.GetChild(3);
            Transform loading_UI = __instance.transform.GetChild(4);

            Font ArialFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

            if (Main.instance.skipLanguage.Value) {
                GameObject langMenu = __instance.transform.GetChild(1).gameObject;
                langMenu.GetComponent<LanguageMenu>().menuCamAnim.enabled = true;
                langMenu.gameObject.SetActive(false);
            }

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
            text.text += $"\n<size=16>Mod Version {Main.modVer}</size>";

            CustomButton quick = UI.UIHelpers.CreateButton("Quick Start", main_UI, UI.AnchorUtils.AnchorPreset.Left, new Vector2(40f, 50f));
            quick.button.name = "Quick Start";
            quick.button.onClick.AddListener(() => {
                GameData.instance.difficultyChosen = DifficultyChosen.Hard;
                __instance.PlayGame();
            });

            CustomButton changelang = UI.UIHelpers.CreateButton("Change Language", main_UI, UI.AnchorUtils.AnchorPreset.Left, new Vector2(0f, GameObject.Find("Credits").GetComponent<RectTransform>().anchoredPosition.y + 30));
            changelang.button.name = "Change Language";
            changelang.Label.fontSize = 10;
            changelang.Rect.sizeDelta /= 1.5f;
            changelang.button.onClick.AddListener(() => __instance.transform.Find("ChooseLanguage").gameObject.SetActive(true));
            #endregion

            #region CHOOSE DIFFICULTY
            choosedifficulty_UI.Find("Text (3)").name = "SelectMode";
            choosedifficulty_UI.Find("Exit (1)").name = "Yangire";
            choosedifficulty_UI.Find("Exit (2)").name = "Hard";
            choosedifficulty_UI.Find("Practice").name = "Yandere";
            choosedifficulty_UI.Find("Practice (1)").name = "Play";
            choosedifficulty_UI.Find("Easy").name = "Normal";

            foreach (string obj in new string[] {"Play", "EASY", "Exit (3)", "Exit (4)", "Image (3)", "Image (4)"}) Object.Destroy(choosedifficulty_UI.Find(obj).gameObject);

            CustomButton closeBtn = UI.UIHelpers.CreateButton("Close", choosedifficulty_UI, UI.AnchorUtils.AnchorPreset.TopRight, new Vector2(0f, 0f));
            closeBtn.button.name = "Close";
            closeBtn.Label.fontSize = 14;
            closeBtn.Rect.sizeDelta /= 1.5f;
            closeBtn.button.onClick.AddListener(() => {
                __instance.cameras[0].SetActive(true);
                __instance.cameras[1].SetActive(false);
                __instance.diffPage.SetActive(false);
            });
            #endregion

            #region LOADING SCREEN
            Text loadTxt = loading_UI.Find("Text (3)").GetComponent<Text>();
            Object.Destroy(loadTxt.GetComponent<Outline>());
            loadTxt.font = ArialFont;
            loadTxt.fontSize = 22;

            loadingScreen = __instance.loadingPage;
            loader.loadingPrefix = "Loading: ";
            loader.loadingText = loadTxt;
            #endregion
        }

        [HarmonyPatch(nameof(MainMenuManager.PlayGame)), HarmonyPrefix]
        static bool LoadPatch() {
            loadingScreen.SetActive(true);
            loader.LoadLevel(1);
            return false;
        }
    }
}