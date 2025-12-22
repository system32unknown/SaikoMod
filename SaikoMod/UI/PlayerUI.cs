using RapidGUI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SaikoMod.Core.Components;
using SaikoMod.Mods;
using SaikoMod.Helper;

namespace SaikoMod.UI
{
    public class PlayerUI : BaseWindowUI
    {
        int page = 0;

        List<Vector3> tempPlayerPos = new List<Vector3>() { Vector3.zero };
        public static Vector3 curPlayerPos = Vector3.zero;
        int selectedWayPos = 0;

        PlayerFunctions pf;
        PlayerController player;
        CameraMotionController cam;
        HealthManager hm;
        DoorAndKeyManager dkm;

        int keyid = 0;
        string keyname = "";

        bool _regeneration = false;

        public void OnLoad() {
            dkm = Object.FindObjectOfType<DoorAndKeyManager>();
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
                    HealthMod.godModeType = RGUI.Field(HealthMod.godModeType, "Godmode Type");
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

                    if (hm) {
                        GUILayout.BeginVertical("Box");
                        hm.maxRegenerateHealth = hm.maximumHealth = RGUI.SliderFloat(hm.maximumHealth, 0f, 999f, 200f, "Max Health");
                        hm.Health = RGUI.SliderFloat(hm.Health, 0.5f, hm.maximumHealth, hm.maximumHealth, "Health");
                        GUILayout.EndVertical();
                    }

                    GUILayout.BeginVertical("Box");
                    GUILayout.Label("Waypoints");
                    if (tempPlayerPos.Count > 0)
                    {
                        selectedWayPos = RGUI.SliderInt(selectedWayPos, 0, tempPlayerPos.Count - 1, 0, "Selected Waypoint");
                        GUILayout.Label("Saved Pos: " + tempPlayerPos[selectedWayPos].ToString());
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Save Position"))
                        {
                            curPlayerPos = player.transform.position;
                            tempPlayerPos[selectedWayPos] = curPlayerPos;
                        };
                        if (GUILayout.Button("Reset Position")) tempPlayerPos[selectedWayPos] = Vector3.zero;
                        if (GUILayout.Button("Load Position")) player.transform.position = tempPlayerPos[selectedWayPos];
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add"))
                    {
                        tempPlayerPos.Add(Vector3.zero);
                        selectedWayPos = tempPlayerPos.Count - 1;
                    }
                    if (tempPlayerPos.Count > 1 && GUILayout.Button("Remove"))
                    {
                        tempPlayerPos.RemoveAt(tempPlayerPos.Count - 1);
                        selectedWayPos = tempPlayerPos.Count - 1;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    break;

                case 1:
                    if (player) {
                        AIRoom curRoom = player.currentRoom;
                        if (curRoom) {
                            GUILayout.BeginVertical("Box");
                            GUILayout.Label("Current Room: " + curRoom.roomName + " (" + curRoom.roomType + ")");
                            GUILayout.Label("Saiko Visited: " + curRoom.isVisitedRoom + " | Locked: " + curRoom.isLockedRoom + " | Lights: " + curRoom.AllowLights, RGUIStyle.centerLabel);
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button("Toggle Lights")) curRoom.AllowLights = !curRoom.AllowLights;
                            if (GUILayout.Button("Hide Room")) curRoom.HiddenHere();
                            GUILayout.EndHorizontal();
                            GUILayout.EndVertical();
                        }

                        GUILayout.BeginVertical("Box");
                        GUILayout.Label("Cameras");
                        if (pf) {
                            GUILayout.BeginVertical("Box");
                            GUILayout.Label("FOV");
                            pf.NormalFOV = RGUI.SliderFloat(pf.NormalFOV, 0f, 999f, 60f, "Normal FOV");
                            pf.ZoomFOV = RGUI.SliderFloat(pf.ZoomFOV, 0f, 999f, 35f, "Zoom FOV");
                            pf.ZoomSpeed = RGUI.SliderFloat(pf.ZoomSpeed, 0f, 999f, 5f, "Zoom Speed");
                            GUILayout.EndVertical();
                        }

                        GUILayout.BeginVertical("Box");
                        GUILayout.Label("Clip");
                        Camera.main.farClipPlane = RGUI.SliderFloat(Camera.main.farClipPlane, 0f, 5555f, 30f, "Far Clip");
                        Camera.main.nearClipPlane = RGUI.SliderFloat(Camera.main.nearClipPlane, 0f, 5555f, 1000f, "Near Clip");
                        GUILayout.EndVertical();
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical("Box");
                        if (cam && cam.chairWithRope.activeSelf && GUILayout.Button("Escape Chair")) {
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
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Drugged")) player.GetsDrugged();
                        if (GUILayout.Button("Poisoned")) player.GetsPoisoned();
                        GUILayout.EndHorizontal();
                        if (hm)
                        {
                            if (!_regeneration && GUILayout.Button("Start Regeneration"))
                            {
                                hm.StartCoroutine("Regenerate");
                                _regeneration = true;
                            }
                            if (_regeneration && GUILayout.Button("Stop Regeneration"))
                            {
                                hm.StopCoroutine("Regenerate");
                                _regeneration = false;
                            }
                            hm.regenerationSpeed = RGUI.SliderFloat(hm.regenerationSpeed, 0.5f, 99f, 1f, "Regeneration Speed");
                        }
                        GUILayout.EndVertical();
                    }

                    if (dkm)
                    {
                        GUILayout.BeginVertical("Box");
                        GUILayout.Label("Keys");
                        keyid = RGUI.Field(keyid, "Key Id");
                        keyname = RGUI.Field(keyname, "Key Name");
                        if (GUILayout.Button("Create Key")) dkm.SpawnKey(keyid, keyname, player.gameObject.transform.position);
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Hold Key")) player.HoldKey(keyid, keyname);
                        if (player.hasKeyInHand && GUILayout.Button("Drop Key")) player.DropKey(player.currentKeyIdInHand);
                        if (GUILayout.Button("TP Key to Player"))
                        {
                            foreach (InventoryItem keyItem in Resources.FindObjectsOfTypeAll<InventoryItem>().Where(x => x.gameObject.activeSelf))
                            {
                                keyItem.transform.position = player.transform.position + new Vector3(2f, 0f, 0f);
                            }
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }
                    break;
            }
            page = RGUI.Page(page, 3, true);
        }

        public override string Title => "Player";
    }
}