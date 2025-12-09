using System.Collections.Generic;
using SaikoMod.Core.Enums;
using SaikoMod.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaikoMod.Controller
{
    public class UIController : BaseController
    {
        public static UIController Instance;

        public Rect MainMenuRect = new Rect(20f, (Screen.height / 2) - (200 / 2), 130f, 240f);
        public Rect TabMenuRect = new Rect(200f, (Screen.height / 2) - 370.5f, 100f, 200f);
        public bool showMainMenu = false;

        public MenuTab MenuTab = MenuTab.Off;

        readonly PlayerUI playermods = new PlayerUI();
        readonly SaikoUI saikomods = new SaikoUI();
        readonly GameUI gamemods = new GameUI();
        readonly OtherUI othermods = new OtherUI();
        readonly LightingUI lighting = new LightingUI();
        readonly AssetBundleUI assetBundle = new AssetBundleUI();
        readonly SettingsUI settings = new SettingsUI();

        void Awake() => Instance = this;
        public override void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode) {
            if (loadSceneMode == LoadSceneMode.Single) {
                if (scene.name == "LevelNew") {
                    playermods.OnLoad();
                    gamemods.OnLoad();
                    saikomods.OnLoad();
                    assetBundle.OnLoad();

                    othermods.yand = saikomods.yand;
                }
                settings.OnLoad();
                lighting.OnLoad();
            }
        }
        public override void OnSceneUnload(Scene scene) {
            if (scene.name == "LevelNew")
            {
                saikomods.OnUnload();
                assetBundle.OnUnload();
                gamemods.OnUnload();
            }
        }

        void Update() {
            if (SceneManager.GetActiveScene().name == "LevelNew") {
                gamemods.OnUpdate();
                lighting.OnUpdate();
            }
            if (Input.GetKeyDown(KeyCode.Tab)) {
                showMainMenu = !showMainMenu;
                SetCursorState(showMainMenu);
            }
        }

        void SetCursorState(bool opened) {
            if (SceneManager.GetActiveScene().name == "MainMenu") return;
            Cursor.visible = opened;
            Cursor.lockState = opened ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public override void DoGUI() {
            if (!showMainMenu) return;

            MainMenuRect = GUILayout.Window(9000, MainMenuRect, MainMenu, "<b>Saiko Mod Menu</b>");

            if (MenuTab == MenuTab.Off) return;
            TabMenuRect = GUI.Window(9001, new Rect(TabMenuRect.position, GetTabSize(MenuTab)), GetTabWinFunc(MenuTab), "<b>" + GetTabTitle(MenuTab) + "</b>");
        }

        GUI.WindowFunction GetTabWinFunc(MenuTab tab) {
            switch (tab) {
                case MenuTab.Player: return playermods.WindowLayout;
                case MenuTab.Saiko: return saikomods.WindowLayout;
                case MenuTab.Game: return gamemods.WindowLayout;
                case MenuTab.Others: return othermods.WindowLayout;
                case MenuTab.Lighting: return lighting.WindowLayout;
                case MenuTab.AssetBundle: return assetBundle.WindowLayout;
                case MenuTab.Settings: return settings.WindowLayout;
            }
            return null;
        }
        string GetTabTitle(MenuTab tab)
        {
            switch (tab) {
                case MenuTab.Player: return "Player Mod";
                case MenuTab.Saiko: return "Saiko Mod";
                case MenuTab.Game: return "Game Mod";
                case MenuTab.Others: return "Other Mod";
                case MenuTab.Lighting: return "Lighting";
                case MenuTab.AssetBundle: return "Assetbundle";
                case MenuTab.Settings: return "Settings";
            }
            return "";
        }
        Vector3 GetTabSize(MenuTab tab)
        {
            switch (tab)
            {

            }
            return new Vector2(444f, 664f);
        }

        void MainMenu(int windowID) {
            GUI.backgroundColor = Color.black;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            // Condensed button list
            CreateTabButton(MenuTab.Player, "Player Mod");
            CreateTabButton(MenuTab.Saiko, "Saiko Mod");
            CreateTabButton(MenuTab.Game, "Game Mod");
            CreateTabButton(MenuTab.Others, "Other Mod");
            CreateTabButton(MenuTab.Lighting, "Lighting");
            CreateTabButton(MenuTab.AssetBundle, "AssetBundle");
            CreateTabButton(MenuTab.Settings, "Settings");
        }

        void CreateTabButton(MenuTab tab, string label) {
            if (GUILayout.Button("<b>" + label + "</b>", GUILayout.Height(21f))) {
                MenuTab = (MenuTab == tab) ? MenuTab.Off : tab;
            }
        }
    }
}