using SaikoMod.Core.Enums;
using SaikoMod.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaikoMod.Controller
{
    public class UIController : BaseController
    {
        public static UIController Instance;

        public Rect MainMenuRect = new Rect(20f, Screen.currentResolution.height / 2 - 370.5f, 130f, 200f);
        public Rect TabMenuRect = new Rect(200f, Screen.currentResolution.height / 2 - 370.5f, 100f, 200f);
        public bool showMainMenu = false;

        public MenuTab MenuTab = MenuTab.Off;
        
        void Awake() => Instance = this;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                if (!showMainMenu) {
                    Open();
                    return;
                }
                Close();
                return;
            }
        }

        void Open()
        {
            showMainMenu = true;
            if (SceneManager.GetActiveScene().name != "MainMenu") {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        void Close()
        {
            showMainMenu = false;
            if (SceneManager.GetActiveScene().name != "MainMenu") {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public override void DoGUI()
        {
            if (!showMainMenu) return;
            MainMenuRect = GUILayout.Window(9000, MainMenuRect, MainMenu, "<b>Saiko Mod Menu</b>");

            switch (MenuTab)
            {
                case MenuTab.Player:
                    TabMenuRect = GUI.Window(9001, new Rect(TabMenuRect.position, new Vector2(444f, 664f)), PlayerUI.Window, "<b>Player Mod</b>");
                    break;
                case MenuTab.Saiko:
                    TabMenuRect = GUI.Window(9001, new Rect(TabMenuRect.position, new Vector2(444f, 664f)), SaikoUI.Window, "<b>Saiko Mod</b>");
                    break;
                case MenuTab.Game:
                    TabMenuRect = GUI.Window(9001, new Rect(TabMenuRect.position, new Vector2(444f, 664f)), GameUI.Window, "<b>Game Mod</b>");
                    break;
                case MenuTab.Others:
                    TabMenuRect = GUI.Window(9001, new Rect(TabMenuRect.position, new Vector2(444f, 664f)), OtherUI.Window, "<b>Other Mod</b>");
                    break;
                case MenuTab.Settings:
                    TabMenuRect = GUI.Window(9001, new Rect(TabMenuRect.position, new Vector2(444f, 664f)), SettingsUI.Window, "<b>Settings</b>");
                    break;
            }
        }

        void MainMenu(int windowID) {
            GUI.backgroundColor = Color.black;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            if (GUILayout.Button("<b>Player Mod</b>", GUILayout.Height(21f)))
            {
                MenuTab = (MenuTab == MenuTab.Player) ? MenuTab.Off : MenuTab.Player;
            }
            if (GUILayout.Button("<b>Saiko Mod</b>", GUILayout.Height(21f)))
            {
                MenuTab = (MenuTab == MenuTab.Saiko) ? MenuTab.Off : MenuTab.Saiko;
            }
            if (GUILayout.Button("<b>Game Mod</b>", GUILayout.Height(21f)))
            {
                MenuTab = (MenuTab == MenuTab.Game) ? MenuTab.Off : MenuTab.Game;
            }
            if (GUILayout.Button("<b>Other Mod</b>", GUILayout.Height(21f)))
            {
                MenuTab = (MenuTab == MenuTab.Others) ? MenuTab.Off : MenuTab.Others;
            }
            if (GUILayout.Button("<b>Settings</b>", GUILayout.Height(21f)))
            {
                MenuTab = (MenuTab == MenuTab.Settings) ? MenuTab.Off : MenuTab.Settings;
            }
        }
    }
}