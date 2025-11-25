using RapidGUI;
using UnityEngine;
using SaikoMod.Controller;
using SaikoMod.Mods;
using SaikoMod.Utils;
using RogoDigital.Lipsync;

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

            YandereController[] yanderes = Resources.FindObjectsOfTypeAll(typeof(YandereController)) as YandereController[];
            if (yanderes.Length != 0) {
                yand = yanderes[0];
                yand.aI.currentState = RGUI.Field(yand.aI.currentState, "AI State");
                yand.mood.mood = RGUI.Field(yand.mood.mood, "AI Mood");
                yand.mood.angerLevel = RGUI.SliderInt(yand.mood.angerLevel, 0, 10, 0, "Anger Level");
                if (RGUI.Button(yand.isActive, "Is Active"))
                {
                    yand.isActive = !yand.isActive;
                }

                if (GUILayout.Button("RNG Voice")) {
                    LipSyncUtils.Shufflevoices(yand.facial.foundYou);
                    LipSyncUtils.Shufflevoices(yand.facial.angryVoice);
                    LipSyncUtils.Shufflevoices(yand.facial.dontHide);
                    LipSyncUtils.Shufflevoices(yand.facial.heyyou);
                    LipSyncUtils.Shufflevoices(yand.facial.iwillpunish);
                    LipSyncUtils.Shufflevoices(yand.facial.laughs);
                    LipSyncUtils.Shufflevoices(yand.facial.introVoices);
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
