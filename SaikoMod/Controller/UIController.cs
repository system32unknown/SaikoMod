using SaikoMod.Core.Enums;
using SaikoMod.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaikoMod.Controller
{
    public class UIController : BaseController
    {
        public static UIController Instance;

        public Rect MainMenuRect = new Rect(20f, (Screen.height / 2) - (200 / 2), 130f, 200f);
        public Rect TabMenuRect = new Rect(200f, Screen.currentResolution.height / 2 - 370.5f, 100f, 200f);
        public bool showMainMenu = false;

        public MenuTab MenuTab = MenuTab.Off;
        readonly Vector2 tabWindowSize = new Vector2(444f, 664f);

        readonly SettingsUI settings = new SettingsUI();
        readonly PlayerUI playermods = new PlayerUI();
        readonly SaikoUI saikomods = new SaikoUI();
        readonly GameUI gamemods = new GameUI();
        readonly OtherUI othermods = new OtherUI();
        readonly LightingUI lighting = new LightingUI();

        void Awake() => Instance = this;
        public override void onSceneLoad(Scene scene, LoadSceneMode loadSceneMode) {
            if (loadSceneMode == LoadSceneMode.Single) {
                if (scene.name == "LevelNew") {
                    playermods.Reload();
                    gamemods.Reload();
                    saikomods.Reload();
                }
                lighting.Reload();
            }
        }

        void Update() {
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
            TabMenuRect = GUI.Window(9001, new Rect(TabMenuRect.position, tabWindowSize), GetTabWindowFunction(MenuTab), "<b>" + GetTabTitle(MenuTab) + "</b>");
        }

        GUI.WindowFunction GetTabWindowFunction(MenuTab tab) {
            switch (tab) {
                case MenuTab.Player: return playermods.WindowLayout;
                case MenuTab.Saiko: return saikomods.WindowLayout;
                case MenuTab.Game: return gamemods.WindowLayout;
                case MenuTab.Others: return othermods.WindowLayout;
                case MenuTab.Settings: return settings.WindowLayout;
                case MenuTab.Lighting: return lighting.WindowLayout;
            }
            return null;
        }

        /// <summary> Returns window title for selected tab. </summary>
        string GetTabTitle(MenuTab tab)
        {
            switch (tab) {
                case MenuTab.Player: return "Player Mod";
                case MenuTab.Saiko: return "Saiko Mod";
                case MenuTab.Game: return "Game Mod";
                case MenuTab.Others: return "Other Mod";
                case MenuTab.Settings: return "Settings";
                case MenuTab.Lighting: return "Lighting";
            }
            return "";
        }

        void MainMenu(int windowID) {
            GUI.backgroundColor = Color.black;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            // Condensed button list
            CreateTabButton(MenuTab.Player, "Player Mod");
            CreateTabButton(MenuTab.Saiko, "Saiko Mod");
            CreateTabButton(MenuTab.Game, "Game Mod");
            CreateTabButton(MenuTab.Others, "Other Mod");
            CreateTabButton(MenuTab.Settings, "Settings");
            CreateTabButton(MenuTab.Lighting, "Lighting");
        }

        void CreateTabButton(MenuTab tab, string label) {
            if (GUILayout.Button("<b>" + label + "</b>", GUILayout.Height(21f))) {
                MenuTab = (MenuTab == tab) ? MenuTab.Off : tab;
            }
        }
    }
}