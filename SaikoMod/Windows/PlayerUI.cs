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

        public static void Window(int _)
        {
            GUI.backgroundColor = Color.black;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            Title();

            GUILayout.BeginVertical("Box");
            if (RGUI.Button(SaikoTracker.updateTracker, "Update Tracker")) SaikoTracker.updateTracker = !SaikoTracker.updateTracker;
            SaikoTracker.updateRate = RGUI.SliderFloat(SaikoTracker.updateRate, 0.1f, 10f, 3f, "Update Rate");
            GUILayout.EndVertical();

            if (RGUI.Button(GameManagerMod.EyeEnabled, "Eye Vision")) GameManagerMod.EyeEnabled = !GameManagerMod.EyeEnabled;
            if (RGUI.Button(HealthMod.noKill, "No Kill")) HealthMod.noKill = !HealthMod.noKill;
            if (RGUI.Button(HealthMod.noDamage, "No Damage")) HealthMod.noDamage = !HealthMod.noDamage;
            if (RGUI.Button(YandModController.noChoke, "No Choking")) YandModController.noChoke = !YandModController.noChoke;

            PlayerController player = Object.FindObjectOfType<PlayerController>();
            CameraMotionController cam = Object.FindObjectOfType<CameraMotionController>();
            HealthManager hm = Object.FindObjectOfType<HealthManager>();

            if (player != null) {
                if (RGUI.Button(player.beingRide, "Being Ride")) player.beingRide = !player.beingRide;
                GUILayout.BeginVertical("Box");
                player.runSpeed = RGUI.SliderFloat(player.runSpeed, 0f, 999f, 200f, "Run Speed");
                player.walkSpeed = RGUI.SliderFloat(player.walkSpeed, 0f, 999f, 200f, "Walk Speed");
                player.crouchSpeed = RGUI.SliderFloat(player.crouchSpeed, 0f, 999f, 200f, "Crouch Speed");
                player.proneSpeed = RGUI.SliderFloat(player.proneSpeed, 0f, 999f, 200f, "Prone Speed");
                GUILayout.EndVertical();
            }

            if (cam != null && GUILayout.Button("Escape Chair")) {
                cam.playerController.playerAnimModel.SetBool("Sitting", false);
                ReflectionHelpers.ReflectionSetVariable(cam, "isFollowingIntroCam", false);
                cam.playerController.boundedInChair = false;
                cam.chairWithRope.SetActive(false);
                HFPS_GameManager.instance.scriptManager.GetScript<PlayerFunctions>().enabled = true;
                HFPS_GameManager.instance.cf2rig.enabled = true;
                HFPS_GameManager.instance.uiInteractive = true;
            }

            if (hm != null) {
                GUILayout.BeginVertical("Box");
                hm.maximumHealth = RGUI.SliderFloat(hm.maximumHealth, 0f, 999f, 200f, "Max Health");
                hm.Health = RGUI.SliderFloat(hm.Health, 0.5f, hm.maximumHealth, hm.maximumHealth, "Health");
                GUILayout.EndVertical();
            }
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
