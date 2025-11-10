using RapidGUI;
using UnityEngine;
using SaikoMod.Controller;
using SaikoMod.Components;
using SaikoMod.Mods;

namespace SaikoMod.Windows
{
    public static class PlayerUI
    {
        public static bool showMenu;
        public static Rect rect = new Rect(1, 1, 100, 100);

        public static void Window(int windowID)
        {
            GUI.backgroundColor = Color.black;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            Title();

            GUILayout.BeginVertical("Box");
            if (RGUI.Button(SaikoTracker.updateTracker, "Update Tracker"))
            {
                SaikoTracker.updateTracker = !SaikoTracker.updateTracker;
            }
            SaikoTracker.updateRate = RGUI.SliderFloat(SaikoTracker.updateRate, 1f, 10f, 3f, "Update Rate");
            GUILayout.EndVertical();

            GUILayout.BeginVertical("Box");
            if (RGUI.Button(HealthMod.noKill, "No Kill"))
            {
                HealthMod.noKill = !HealthMod.noKill;
            }
            if (RGUI.Button(HealthMod.noDamage, "No Damage"))
            {
                HealthMod.noDamage = !HealthMod.noDamage;
            }
            GUILayout.EndVertical();
        }

        private static void Title()
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
