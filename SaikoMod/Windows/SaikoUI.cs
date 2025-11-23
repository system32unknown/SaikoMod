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
                GUILayout.Label($"AI State: {yand.aI.currentState}");
                GUILayout.Label($"Anger Level: {yand.mood.angerLevel}");

                GUILayout.BeginVertical("Box");
                GUILayout.Label("LipSync");
                if (GUILayout.Button("RNG Voice"))
                {
                    foreach (LipSyncVoice voice in yand.facial.foundYou) {
                        LipSyncData data = voice.clip;
                        foreach (PhonemeMarker phoneme in data.phonemeData) {
                            phoneme.useRandomness = RandomUtil.GetBool();
                            phoneme.intensity = Random.Range(1f, 5f);
                            phoneme.phonemeNumber = Random.Range(0, 10);
                        }

                        foreach (AnimationCurve curve in data.phonemePoseCurves)
                        {
                            for (int i = 0; i < curve.keys.Length; ++i)
                            {
                                Keyframe k = curve.keys[i];
                                k.value = Random.Range(-2f, 2f);
#pragma warning disable CS0618 // Type or member is obsolete
                                k.tangentMode = Random.Range(0, 21);
#pragma warning restore CS0618 // Type or member is obsolete
                                curve.MoveKey(i, k);
                            }
                        }
                    }
                }
                GUILayout.EndVertical();
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
