using RapidGUI;
using UnityEngine;
using SaikoMod.Controller;
using SaikoMod.Mods;
using SaikoMod.Utils;

namespace SaikoMod.Windows
{
    public static class SaikoUI
    {
        static YandereController yand;
        public static bool showMenu;
        public static Rect rect = new Rect(1, 1, 100, 100);

        public static void Window(int _)
        {
            GUI.backgroundColor = Color.black;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            Title();

            GUILayout.BeginVertical("Box");
            if (RGUI.Button(YandModController.notDetected, "No Detect"))
            {
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
            if (RGUI.Button(YandModController.notAlerted, "No Alerted"))
            {
                YandModController.notAlerted = !YandModController.notAlerted;
            }
            YandModController.lookMode = RGUI.Field(YandModController.lookMode, "Look Mode");
            GUILayout.EndVertical();

            yand = ReflectionHelpers.GetSingleResourceOfType<YandereController>();
            if (yand != null) {
                yand.aI.currentState = RGUI.Field(yand.aI.currentState, "AI State");
                yand.mood.mood = RGUI.Field(yand.mood.mood, "AI Mood");
                yand.mood.saikoState = RGUI.Field(yand.mood.saikoState, "Saiko State");
                yand.slowDownType = RGUI.Field(yand.slowDownType, "Slowdown Type");
                yand.mood.angerLevel = RGUI.SliderInt(yand.mood.angerLevel, 0, 10, 0, "Anger Level");
                if (RGUI.Button(yand.isActive, "Is Active")) yand.isActive = !yand.isActive;

                if (GUILayout.Button("RNG Voice")) {
                    foreach (LipSyncVoice[] voices in SaikoMod.Utils.ReflectionHelpers.GetPublicFieldsOfType<LipSyncVoice[]>(yand.facial)) {
                        LipSyncUtils.Shufflevoices(voices);
                    }
                }
            }
        }

        static void Title()
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
