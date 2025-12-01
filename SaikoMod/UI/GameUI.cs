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
        int page = 0;
        string gameMessage = "";
        MessageType messageType = MessageType.Hint;

        public static string patternCode = "";

        InteractManager interact;
        DynamicObject dynamicObj;
        bool otherWay = false;

        ElectricPuzzle ep;
        Electricity ele;
        Keypad keypad;

        public static AIRoom[] aiRooms;
        public static AIRoom curRoom;
        int roomIdx = 0;

        public void OnLoad()
        {
            ep = Object.FindObjectOfType<ElectricPuzzle>();
            ele = Object.FindObjectOfType<Electricity>();
            aiRooms = Object.FindObjectsOfType<AIRoom>();
            interact = Object.FindObjectOfType<InteractManager>();
            curRoom = aiRooms[0];

            keypad = Object.FindObjectsOfType<Keypad>().Where(x => x.gameObject.name == "Keypad (1)").First();
        }

        public void OnUpdate() {
            GameObject rayObj = interact.RaycastObject;
            if (!rayObj) return;

            if (rayObj.GetComponent<DynamicObject>()) dynamicObj = rayObj.GetComponent<DynamicObject>();
            else if (rayObj.GetComponent<DynamicNode>()) dynamicObj = rayObj.GetComponent<DynamicNode>().door;
        }

        public override void Draw() {
            switch (page)
            {
                case 0:
                    GameData.instance.difficultyChosen = RGUI.Field(GameData.instance.difficultyChosen, "Game Difficulty");
                    if (GUILayout.Button("Fix CF2 Canvas")) HFPS_GameManager.instance.cf2rig.enabled = true;
                    if (!CCTVManager.AddedMoreCam && GUILayout.Button("Add POV CCTV")) CCTVManager.AddMoreCam();

                    if (HFPS_GameManager.instance)
                    {
                        GUILayout.BeginVertical("Box");
                        GUILayout.Label("Messages");
                        messageType = RGUI.Field(messageType, "Message Type");
                        GUILayout.BeginHorizontal();
                        gameMessage = GUILayout.TextField(gameMessage, GUILayout.Height(21f));
                        if (GUILayout.Button("Send Message", GUILayout.ExpandWidth(false)))
                        {
                            switch (messageType)
                            {
                                case MessageType.Hint: HFPS_GameManager.instance.ShowHint(gameMessage); break;
                                case MessageType.Message: HFPS_GameManager.instance.AddMessage(gameMessage); break;
                                case MessageType.ItemName: HFPS_GameManager.instance.AddPickupMessage(gameMessage); break;
                                case MessageType.Warning: HFPS_GameManager.instance.WarningMessage(gameMessage); break;
                                default: break;
                            }
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
                    }

                    if (ep)
                    {
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

                    if (HFPS_GameManager.instance)
                    {
                        GUILayout.BeginVertical("Box");
                        GUILayout.Label("Time: " + StringUtils.FormatTime(HFPS_GameManager.instance.seconds), RGUIStyle.centerLabel);
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Stop")) HFPS_GameManager.instance.StopTimer();
                        if (GUILayout.Button("Reset")) HFPS_GameManager.instance.seconds = 0;
                        if (GUILayout.Button("Start")) HFPS_GameManager.instance.StartTimer();
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }

                    if (interact)
                    {
                        interact.RayLength = RGUI.SliderFloat(interact.RayLength, 0f, 50f, 2.5f, "Interact Distance");
                        if (interact.RaycastObject && dynamicObj)
                        {
                            GUILayout.BeginVertical("Box");
                            if (RGUI.Button(dynamicObj.isLocked, "Locked")) dynamicObj.isLocked = !dynamicObj.isLocked;
                            if (RGUI.Button(otherWay, "Use Other Way")) otherWay = !otherWay;
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button("Use")) dynamicObj.UseObject(otherWay);
                            if (GUILayout.Button("Saiko Use")) dynamicObj.YandereUseObject(otherWay);
                            GUILayout.EndHorizontal();
                            GUILayout.EndVertical();
                        }
                    }
                    break;
            }
            page = RGUI.Page(page, 2, true);
        }

        public override string Title => "Game";
    }
}
