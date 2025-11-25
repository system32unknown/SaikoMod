using RapidGUI;
using UnityEngine;
using SaikoMod.Controller;
using SaikoMod.Core.Components;

namespace SaikoMod.Windows
{
    public static class GameUI
    {
        public static bool showMenu;
        public static Rect rect = new Rect(1, 1, 100, 100);
        static string gameMessage = "";

        public static void Window(int _)
        {
            GUI.backgroundColor = Color.black;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            Title();

            GameData.instance.difficultyChosen = RGUI.Field(GameData.instance.difficultyChosen, "Game Difficulty");
            if (GUILayout.Button("Fix CF2 Canvas")) HFPS_GameManager.instance.cf2rig.enabled = true;
            if (!CCTVManager.AddedMoreCam && GUILayout.Button("Add POV CCTV")) CCTVManager.AddMoreCam();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("Messages");
            gameMessage = GUILayout.TextField(gameMessage, GUILayout.Height(21f));
            if (GUILayout.Button("Show Hint")) {
                HFPS_GameManager.instance.ShowHint(gameMessage);
            }
            if (GUILayout.Button("Show Notification")) {
                HFPS_GameManager.instance.AddMessage(gameMessage);
            }
            if (GUILayout.Button("Show Pickup")) {
                HFPS_GameManager.instance.AddPickupMessage(gameMessage);
            }
            GUILayout.EndVertical();

            RGUI.Divider();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("Endings");
            if (GUILayout.Button("Good Ending")) HFPS_GameManager.instance.GoodEnding();
            if (GUILayout.Button("Bad Ending")) HFPS_GameManager.instance.BadEnding();
            GUILayout.EndVertical();
        }

        static void Title()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>Game Mods</b>", GUILayout.Height(21f));
            if (GUILayout.Button("<b>X</b>", GUILayout.Height(19f), GUILayout.Width(32f)))
            {
                UIController.Instance.MenuTab = Core.Enums.MenuTab.Off;
            }
            GUILayout.EndHorizontal();
        }
    }
}
