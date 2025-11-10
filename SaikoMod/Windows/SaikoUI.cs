using RapidGUI;
using UnityEngine;
using SaikoMod.Controller;
using SaikoMod.Mods;

namespace SaikoMod.Windows
{
    public static class SaikoUI
    {
        public static bool showMenu;
        public static Rect rect = new Rect(1, 1, 100, 100);

        public static void Window(int windowID)
        {
            GUI.backgroundColor = Color.black;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            Title();

            GUILayout.BeginVertical("Box");
            if (RGUI.Button(YandModAI.notDetected, "No Detect"))
            {
                YandModAI.notDetected = !YandModAI.notDetected;
                YandModController.notDetected = !YandModController.notDetected;
            }
            if (RGUI.Button(YandModAI.notAttacted, "No Attact"))
            {
                YandModAI.notAttacted = !YandModAI.notAttacted;
            }
            if (RGUI.Button(YandModAI.noDistanceCheck, "No Distance Check"))
            {
                YandModAI.noDistanceCheck = !YandModAI.noDistanceCheck;
            }
            if (RGUI.Button(YandModController.lookingatPlayer, "Looking At Player"))
            {
                YandModController.lookingatPlayer = !YandModController.lookingatPlayer;
            }
            GUILayout.EndVertical();
            YandereController[] yanderes = Resources.FindObjectsOfTypeAll(typeof(YandereController)) as YandereController[];
            if (yanderes.Length != 0)
            {
                YandereController yand = yanderes[0];
                GUILayout.Label($"AI State: {yand.aI.currentState}");
            }
        }

        private static void Title()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>Saiko Mods</b>", GUILayout.Height(21f));
            if (GUILayout.Button("<b>X</b>", GUILayout.Height(19f), GUILayout.Width(32f)))
            {
                UIController.Instance.MenuTab = Core.Enums.MenuTab.Off;
            }
            GUILayout.EndHorizontal();
        }
    }
}
