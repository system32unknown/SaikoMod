using RapidGUI;
using UnityEngine;
using UnityEngine.AI;
using SaikoMod.Core.Components;
using SaikoMod.Mods;
using SaikoMod.Utils;

namespace SaikoMod.UI
{
    public class PlayerUI : BaseWindowUI
    {
        int page = 0;

        Vector3[] tempPlayerPos = {
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            Vector3.zero
        };
        public static Vector3 curPlayerPos = Vector3.zero;
        int selectedWayPos = 0;

        PlayerFunctions pf;
        PlayerController player;
        CameraMotionController cam;
        HealthManager hm;

        public void OnLoad() {
            pf = Object.FindObjectOfType<PlayerFunctions>();
            player = Object.FindObjectOfType<PlayerController>();
            cam = Object.FindObjectOfType<CameraMotionController>();
            hm = Object.FindObjectOfType<HealthManager>();
        }

        public override void Draw() {
            
            switch (page)
            {
                case 0:
                    GUILayout.BeginVertical("Box");
                    if (RGUI.Button(SaikoTracker.UpdateTracker, "Update Tracker")) SaikoTracker.UpdateTracker = !SaikoTracker.UpdateTracker;
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
                        GUILayout.BeginVertical("Box");
                        player.runSpeed = RGUI.SliderFloat(player.runSpeed, 0f, 999f, 7f, "Run Speed");
                        player.walkSpeed = RGUI.SliderFloat(player.walkSpeed, 0f, 999f, 4f, "Walk Speed");
                        player.crouchSpeed = RGUI.SliderFloat(player.crouchSpeed, 0f, 999f, 2f, "Crouch Speed");
                        player.proneSpeed = RGUI.SliderFloat(player.proneSpeed, 0f, 999f, 1f, "Prone Speed");
                        player.state = RGUI.SliderInt(player.state, 0, 2, 0, "Player State");
                        GUILayout.EndVertical();
                    }

                    if (cam && GUILayout.Button("Escape Chair"))
                    {
                        player.playerAnimModel.SetBool("Sitting", false);
                        player.boundedInChair = false;
                        cam.ReflectionSetVariable("isFollowingIntroCam", false);
                        cam.chairWithRope.SetActive(false);
                        cam.enabled = false;
                        pf.enabled = true;
                        GameObject.Find("yandere").GetComponent<NavMeshAgent>().enabled = true;
                        HFPS_GameManager.instance.cf2rig.enabled = true;
                        HFPS_GameManager.instance.uiInteractive = true;
                    }

                    if (hm) {
                        GUILayout.BeginVertical("Box");
                        hm.maximumHealth = RGUI.SliderFloat(hm.maximumHealth, 0f, 999f, 200f, "Max Health");
                        hm.Health = RGUI.SliderFloat(hm.Health, 0.5f, hm.maximumHealth, hm.maximumHealth, "Health");
                        GUILayout.EndVertical();
                    }

                    GUILayout.BeginVertical("Box");
                    GUILayout.Label("Waypoints");
                    selectedWayPos = RGUI.SliderInt(selectedWayPos, 0, tempPlayerPos.Length - 1, 0, "Selected Waypoint");
                    GUILayout.Label("Saved Pos: " + tempPlayerPos[selectedWayPos].ToString());
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Save Position")) {
                        curPlayerPos = player.transform.position;
                        tempPlayerPos[selectedWayPos] = curPlayerPos;
                    };
                    if (GUILayout.Button("Reset Position")) tempPlayerPos[selectedWayPos] = Vector3.zero;
                    if (GUILayout.Button("Load Position")) player.transform.position = tempPlayerPos[selectedWayPos];
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    break;

                case 1:
                    if (player) {
                        AIRoom curRoom = player.currentRoom;
                        if (curRoom) {
                            GUILayout.BeginVertical("Box");
                            GUILayout.Label("Current Room: " + curRoom.roomName + " (" + curRoom.roomType + ")");
                            GUILayout.Label("- Saiko Visited: " + curRoom.isVisitedRoom);
                            GUILayout.Label("- Locked: " + curRoom.isLockedRoom);
                            GUILayout.Label("- Lights: " + curRoom.AllowLights);

                            if (GUILayout.Button("Toggle Lights")) curRoom.AllowLights = !curRoom.AllowLights;
                            if (GUILayout.Button("Hide Room")) curRoom.HiddenHere();

                            GUILayout.EndVertical();
                        }

                        GUILayout.BeginVertical("Box");
                        GUILayout.Label("Cameras");
                        if (pf) {
                            GUILayout.BeginVertical("Box");
                            GUILayout.Label("FOV");
                            pf.NormalFOV = RGUI.SliderFloat(pf.NormalFOV, 0f, 999f, 60f, "Normal FOV");
                            pf.ZoomFOV = RGUI.SliderFloat(pf.ZoomFOV, 0f, 999f, 35f, "Zoom FOV");
                            GUILayout.EndVertical();
                        }

                        GUILayout.BeginVertical("Box");
                        GUILayout.Label("Clip");
                        Camera.main.farClipPlane = RGUI.SliderFloat(Camera.main.farClipPlane, 0f, 5555f, 30f, "Far Clip");
                        Camera.main.nearClipPlane = RGUI.SliderFloat(Camera.main.nearClipPlane, 0f, 5555f, 1000f, "Near Clip");
                        GUILayout.EndVertical();
                        GUILayout.EndVertical();
                    }
                    break;
            }
            page = RGUI.Page(page, 3, true);
        }

        public override string Title => "Player";
    }
}