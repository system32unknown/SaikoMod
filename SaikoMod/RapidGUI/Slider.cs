using System;
using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        public static class SliderSetting
        {
            public static float minWidth = 150f;
            public static float fieldWidth = 50f;
        }

        public static float SliderFloat(float v, float min, float max, float defaultValue, string label = null)
        {
            GUI.backgroundColor = Color.white;
            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>" + label + "</b>", GUILayout.MinWidth(SliderSetting.minWidth));
            float ret = GUILayout.HorizontalSlider(v, min, max, GUILayout.MinWidth(SliderSetting.minWidth));
            ret = (float)StandardField(ret, v.GetType(), GUILayout.Width(SliderSetting.fieldWidth));

            GUI.backgroundColor = Color.black;
            if (GUILayout.Button("<b>Reset</b>", GUILayout.Height(21f), GUILayout.ExpandWidth(false)))
            {
                ret = defaultValue;
            }
            GUILayout.EndHorizontal();
            return ret;
        }

        public static int SliderInt(int v, int min, int max, int defaultValue, string label = null)
        {
            GUI.backgroundColor = Color.white;
            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>" + label + "</b>", GUILayout.MinWidth(SliderSetting.minWidth));
            int ret = (int)GUILayout.HorizontalSlider(v, min, max, GUILayout.MinWidth(SliderSetting.minWidth));
            ret = (int)StandardField(ret, v.GetType(), GUILayout.Width(SliderSetting.fieldWidth));

            GUI.backgroundColor = Color.black;
            if (GUILayout.Button("<b>Reset</b>", GUILayout.Height(21f), GUILayout.ExpandWidth(false)))
            {
                ret = defaultValue;
            }
            GUILayout.EndHorizontal();
            return ret;
        }
    }
}