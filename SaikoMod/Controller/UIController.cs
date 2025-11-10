using SaikoMod.Core.Enums;
using UnityEngine;
using SaikoMod.Windows;

namespace SaikoMod.Controller
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance { get; private set; }
        public Rect MainMenuRect;
        public Rect TabMenuRect;
        public bool showMainMenu;

        public MenuTab MenuTab = MenuTab.Off;

        private void Awake() => Instance = this;

        private void Start()
        {
            MainMenuRect = new Rect(20f, Screen.currentResolution.height / 2 - 370.5f, 100f, 200f);
            TabMenuRect = new Rect(200f, Screen.currentResolution.height / 2 - 370.5f, 100f, 200f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!showMainMenu)
                {
                    Open();
                    return;
                }
                Close();
                return;
            }
        }

        private void Open()
        {
            showMainMenu = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void Close()
        {
            showMainMenu = false;
            Cursor.visible = false;
        }

        private void OnGUI()
        {
            if (showMainMenu)
            {
                MainMenuRect = GUILayout.Window(9000, MainMenuRect, MainMenu, "<b>SaikoMenu</b>");

                switch (MenuTab)
                {
                    case MenuTab.Player:
                        TabMenuRect = GUI.Window(9001, new Rect(TabMenuRect.position, new Vector2(444f, 664f)), PlayerUI.Window, "<b>Player Mod</b>");
                        break;
                    case MenuTab.Saiko:
                        TabMenuRect = GUI.Window(9001, new Rect(TabMenuRect.position, new Vector2(444f, 664f)), SaikoUI.Window, "<b>Saiko Mod</b>");
                        break;
                    case MenuTab.Settings:
                        TabMenuRect = GUI.Window(9001, new Rect(TabMenuRect.position, new Vector2(444f, 664f)), SettingsUI.Window, "<b>Settings</b>");
                        break;
                }
            }
        }

        private void MainMenu(int windowID)
        {
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
            if (GUILayout.Button("<b>Settings</b>", GUILayout.Height(21f)))
            {
                MenuTab = (MenuTab == MenuTab.Settings) ? MenuTab.Off : MenuTab.Settings;
            }
        }
    }
}