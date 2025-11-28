using System.Linq;
using RapidGUI;
using UnityEngine;
using SaikoMod.Mods;
using SaikoMod.Utils;
using SaikoMod.Core.Enums;
using SaikoMod.Core.Components;

namespace SaikoMod.UI
{
    public class GameUI : BaseWindowUI
    {
        string gameMessage = "";
        MessageType messageType = MessageType.Hint;

        public static string patternCode = "";

        ElectricPuzzle ep;
        Electricity ele;
        Keypad keypad;

        public static AIRoom[] aiRooms;
        public static AIRoom curRoom;
        int roomIdx = 0;

        public void Reload()
        {
            ep = Object.FindObjectOfType<ElectricPuzzle>();
            ele = Object.FindObjectOfType<Electricity>();
            aiRooms = Object.FindObjectsOfType<AIRoom>();
            curRoom = aiRooms[0];

            keypad = Object.FindObjectsOfType<Keypad>().Where(x => x.gameObject.name == "Keypad (1)").First();
        }

        public override void Draw() {
            GameData.instance.difficultyChosen = RGUI.Field(GameData.instance.difficultyChosen, "Game Difficulty");
            if (GUILayout.Button("Fix CF2 Canvas")) HFPS_GameManager.instance.cf2rig.enabled = true;
            if (!CCTVManager.AddedMoreCam && GUILayout.Button("Add POV CCTV")) CCTVManager.AddMoreCam();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("Messages");
            messageType = RGUI.Field(messageType, "Message Type");
            GUILayout.BeginHorizontal();
            gameMessage = GUILayout.TextField(gameMessage, GUILayout.Height(21f));
            if (GUILayout.Button("Send Message")) switch (messageType) {
                case MessageType.Hint: HFPS_GameManager.instance.ShowHint(gameMessage); break;
                case MessageType.Message: HFPS_GameManager.instance.AddMessage(gameMessage); break;
                case MessageType.ItemName: HFPS_GameManager.instance.AddPickupMessage(gameMessage); break;
                case MessageType.Warning: HFPS_GameManager.instance.WarningMessage(gameMessage); break;
                default: break;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("Endings");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Good Ending")) HFPS_GameManager.instance.GoodEnding();
            if (GUILayout.Button("Bad Ending")) HFPS_GameManager.instance.BadEnding();
            GUILayout.EndHorizontal();
            if (RGUI.Button(YandModController.noBadEnding, "No Bad Ending")) YandModController.noBadEnding = !YandModController.noBadEnding;
            GUILayout.EndVertical();

            if (ep) {
                GUILayout.BeginVertical("Box");
                GUILayout.Label("Electric Puzzle & KeyPad");
                patternCode = GUILayout.TextField(patternCode, GUILayout.Height(21f));
                if (GUILayout.Button("Set Pattern")) PuzzleMod.SetPuzzle(patternCode);
                if (!ep.puzzleSolved && GUILayout.Button("Solve Puzzle")) ep.PuzzleSolved();
                if (ep.puzzleSolved && !ele.isPoweredOn && GUILayout.Button("Switch On")) ele.SwitcherUp();
                keypad.AccessCode = RGUI.Field(keypad.AccessCode, "Keycode Access");
                GUILayout.EndVertical();
            }
            if (curRoom) curRoom = RGUI.ArrayNavigator(aiRooms, ref roomIdx);
        }

        public override string Title => "Game";
    }
}
