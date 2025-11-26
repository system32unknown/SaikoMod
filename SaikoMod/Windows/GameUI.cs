using RapidGUI;
using UnityEngine;
using SaikoMod.Mods;
using SaikoMod.Controller;
using SaikoMod.Core.Components;

namespace SaikoMod.Windows
{
    public static class GameUI
    {
        public static bool showMenu;
        public static Rect rect = new Rect(1, 1, 100, 100);
        static string gameMessage = "";
        static Message.MessageType messageType = Message.MessageType.Hint;

        public static string patternCode = "";

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
            messageType = RGUI.Field(messageType, "Message Type");
            if (GUILayout.Button("Send Message")) switch (messageType) {
                case Message.MessageType.Hint: HFPS_GameManager.instance.ShowHint(gameMessage); break;
                case Message.MessageType.Message: HFPS_GameManager.instance.AddMessage(gameMessage); break;
                case Message.MessageType.ItemName: HFPS_GameManager.instance.AddPickupMessage(gameMessage); break;
                default: break;
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("Endings");
            if (GUILayout.Button("Good Ending")) HFPS_GameManager.instance.GoodEnding();
            if (GUILayout.Button("Bad Ending")) HFPS_GameManager.instance.BadEnding();
            if (RGUI.Button(YandModController.noBadEnding, "No Bad Ending")) YandModController.noBadEnding = !YandModController.noBadEnding;
            GUILayout.EndVertical();

            ElectricPuzzle ep = Object.FindObjectOfType<ElectricPuzzle>();
            Electricity ele = Object.FindObjectOfType<Electricity>();
            if (ep != null)
            {
                GUILayout.BeginVertical("Box");
                GUILayout.Label("Electric Puzzle");
                patternCode = GUILayout.TextField(patternCode, GUILayout.Height(21f));
                if (GUILayout.Button("Set Pattern")) PuzzleMod.SetPuzzle(patternCode);
                if (!ep.puzzleSolved && GUILayout.Button("Solve Puzzle")) ep.PuzzleSolved();
                if (ep.puzzleSolved && !ele.isPoweredOn && GUILayout.Button("Switch On"))
                {
                    ele.SwitcherUp();
                }
                GUILayout.EndVertical();
            }
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
