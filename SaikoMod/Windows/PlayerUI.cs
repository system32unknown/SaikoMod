using RapidGUI;
using UnityEngine;
using SaikoMod.Controller;
using SaikoMod.Core.Components;
using SaikoMod.Mods;
using SaikoMod.Utils;

namespace SaikoMod.Windows
{
    public static class PlayerUI
    {
        public static bool showMenu;
        public static Rect rect = new Rect(1, 1, 100, 100);
        static int page = 0;

        static Vector3[] tempPlayerPos = {
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            Vector3.zero
        };
        static int selectedWayPos = 0;

        static Fold doorfold;

        static bool _inited = false;
        static void Init()
        {
            if (_inited) return;
            doorfold = new Fold("Doors:");
            doorfold.Add(() => {
                GUILayout.Label("door here.");
            });
            _inited = true;
        }

        public static void Window(int _)
        {
            GUI.backgroundColor = Color.black;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            Title();
            Init();
            PlayerFunctions pf = Object.FindObjectOfType<PlayerFunctions>();
            PlayerController player = Object.FindObjectOfType<PlayerController>();
            CameraMotionController cam = Object.FindObjectOfType<CameraMotionController>();
            HealthManager hm = Object.FindObjectOfType<HealthManager>();
            switch (page)
            {
                case 0:
                    GUILayout.BeginVertical("Box");
                    if (RGUI.Button(SaikoTracker.updateTracker, "Update Tracker")) SaikoTracker.updateTracker = !SaikoTracker.updateTracker;
                    SaikoTracker.updateRate = RGUI.SliderFloat(SaikoTracker.updateRate, 0.1f, 10f, 3f, "Update Rate");
                    GUILayout.EndVertical();

                    if (RGUI.Button(GameManagerMod.EyeEnabled, "Eye Vision")) GameManagerMod.EyeEnabled = !GameManagerMod.EyeEnabled;
                    if (RGUI.Button(HealthMod.noKill, "No Kill")) HealthMod.noKill = !HealthMod.noKill;
                    if (RGUI.Button(HealthMod.noDamage, "No Damage")) HealthMod.noDamage = !HealthMod.noDamage;
                    if (RGUI.Button(YandModController.noChoke, "No Choking")) YandModController.noChoke = !YandModController.noChoke;

                    if (player)
                    {
                        if (RGUI.Button(player.beingRide, "Being Ride")) player.beingRide = !player.beingRide;
                        if (RGUI.Button(player.hasShoes, "Has Shoes")) player.hasShoes = !player.hasShoes;
                        if (RGUI.Button(player.canRun, "Can Run")) player.canRun = !player.beingRide;
                        GUILayout.BeginVertical("Box");
                        player.runSpeed = RGUI.SliderFloat(player.runSpeed, 0f, 999f, 200f, "Run Speed");
                        player.walkSpeed = RGUI.SliderFloat(player.walkSpeed, 0f, 999f, 200f, "Walk Speed");
                        player.crouchSpeed = RGUI.SliderFloat(player.crouchSpeed, 0f, 999f, 200f, "Crouch Speed");
                        player.proneSpeed = RGUI.SliderFloat(player.proneSpeed, 0f, 999f, 200f, "Prone Speed");
                        player.state = RGUI.SliderInt(player.state, 0, 2, 0, "Player State");
                        GUILayout.EndVertical();
                    }

                    if (cam && GUILayout.Button("Escape Chair"))
                    {
                        cam.playerController.playerAnimModel.SetBool("Sitting", false);
                        cam.ReflectionSetVariable("isFollowingIntroCam", false);
                        cam.playerController.boundedInChair = false;
                        cam.chairWithRope.SetActive(false);
                        pf.enabled = true;
                        HFPS_GameManager.instance.cf2rig.enabled = true;
                        HFPS_GameManager.instance.uiInteractive = true;
                    }

                    if (hm) {
                        GUILayout.BeginVertical("Box");
                        hm.maximumHealth = RGUI.SliderFloat(hm.maximumHealth, 0f, 999f, 200f, "Max Health");
                        hm.Health = RGUI.SliderFloat(hm.Health, 0.5f, hm.maximumHealth, hm.maximumHealth, "Health");
                        GUILayout.EndVertical();
                    }

                    if (pf) {
                        GUILayout.BeginVertical("Box");
                        pf.NormalFOV = RGUI.SliderFloat(pf.NormalFOV, 0f, 999f, 60f, "Normal FOV");
                        pf.ZoomFOV = RGUI.SliderFloat(pf.ZoomFOV, 0f, 999f, 60f, "Zoom FOV");
                        GUILayout.EndVertical();
                    }
                    break;

                case 1:
                    if (player) {
                        GUILayout.BeginVertical("Box");
                        GUILayout.Label("Waypoints");
                        selectedWayPos = RGUI.SliderInt(selectedWayPos, 0, tempPlayerPos.Length - 1, 0, "Selected Waypoint");
                        GUILayout.Label("Saved Pos: " + tempPlayerPos[selectedWayPos].ToString());
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Save Position", GUILayout.Width(206f))) tempPlayerPos[selectedWayPos] = player.transform.position;
                        if (GUILayout.Button("Load Position", GUILayout.Width(206f))) player.transform.position = tempPlayerPos[selectedWayPos];
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();

                        AIRoom curRoom = player.currentRoom;
                        if (curRoom) {
                            GUILayout.BeginVertical("Box");
                            GUILayout.Label("Current Room: " + curRoom.roomName + " (" + curRoom.roomType + ")");
                            GUILayout.Label("- Saiko Visited: " + curRoom.isVisitedRoom);
                            GUILayout.Label("- Locked: " + curRoom.isLockedRoom);
                            doorfold.DoGUI();
                            GUILayout.Label("- Lights: " + curRoom.AllowLights);
                            if (GUILayout.Button("Toggle Lights")) curRoom.AllowLights = !curRoom.AllowLights;
                            if (GUILayout.Button("Hide Room")) curRoom.HiddenHere();

                            GUILayout.EndVertical();
                        }
                    }
                    break;
            }
            page = RGUI.Page(page, 3, true);
        }

        static void Title()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>Player Mods</b>", GUILayout.Height(21f));
            if (GUILayout.Button("<b>X</b>", GUILayout.Height(19f), GUILayout.Width(32f)))
            {
                UIController.Instance.MenuTab = Core.Enums.MenuTab.Off;
            }
            GUILayout.EndHorizontal();
        }
    }
}
