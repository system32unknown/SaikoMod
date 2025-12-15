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
        bool timerStarted = true;
        MessageType messageType = MessageType.Hint;

        public static string patternCode = "";

        InteractManager interact;
        DynamicObject dynamicObj;
        bool otherWay = false;

        ElectricPuzzle ep;
        Electricity ele;
        Keypad keypad;
        HFPS_GameManager gm;

        public static AIRoom[] aiRooms;
        public static AIRoom curRoom;
        int roomIdx = 0;

        public void OnLoad()
        {
            ep = Object.FindObjectOfType<ElectricPuzzle>();
            ele = Object.FindObjectOfType<Electricity>();
            aiRooms = Object.FindObjectsOfType<AIRoom>();
            interact = Object.FindObjectOfType<InteractManager>();
            gm = HFPS_GameManager.instance;
            curRoom = aiRooms[0];

            keypad = Object.FindObjectsOfType<Keypad>().Where(x => x.gameObject.name == "Keypad (1)").First();
            CCTVManager.AddedMoreCam = false;
        }

        public void OnUnload()
        {
            timerStarted = true;
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
                    if (GUILayout.Button("Fix CF2 Canvas")) gm.cf2rig.enabled = true;
                    if (!CCTVManager.AddedMoreCam && GUILayout.Button("Add POV CCTV")) CCTVManager.AddMoreCam();

                    if (gm)
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
                                case MessageType.Hint: gm.ShowHint(gameMessage); break;
                                case MessageType.Message: gm.AddMessage(gameMessage); break;
                                case MessageType.ItemName: gm.AddPickupMessage(gameMessage); break;
                                case MessageType.Warning: gm.WarningMessage(gameMessage); break;
                                default: break;
                            }
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical("Box");
                        GUILayout.Label("Endings");
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Good Ending")) gm.GoodEnding();
                        if (GUILayout.Button("Bad Ending")) gm.BadEnding();
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
                    if (curRoom) curRoom = RGUI.ArrayNavigator<AIRoom>(aiRooms, ref roomIdx);

                    if (gm)
                    {
                        GUILayout.BeginVertical("Box");
                        GUILayout.Label("Time: " + StringUtils.FormatTime(gm.seconds), RGUIStyle.centerLabel);
                        GUILayout.BeginHorizontal();
                        if (timerStarted && GUILayout.Button("Stop")) {
                            timerStarted = false;
                            gm.StopTimer();
                        }
                        if (!timerStarted && GUILayout.Button("Start")) {
                            timerStarted = true;
                            gm.StartTimer();
                        }
                        if (GUILayout.Button("Reset")) gm.seconds = 0;
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }

                    if (interact)
                    {
                        interact.RayLength = RGUI.SliderFloat(interact.RayLength, 0f, 50f, 2.5f, "Interact Distance");
                        if (interact.RaycastObject && dynamicObj && dynamicObj.dynamicType == Type_Dynamic.Door)
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
