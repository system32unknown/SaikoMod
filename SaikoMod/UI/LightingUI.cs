using UnityEngine;
using System.Linq;
using RapidGUI;

namespace SaikoMod.UI
{
    public class LightingUI : BaseWindowUI {
        int page = 0;

        Light directionLight;
        public void Reload()
        {
            directionLight = Object.FindObjectsOfType<Light>().Where(x => x.type == LightType.Directional).First();
        }

        public override void Draw()
        {
            switch (page)
            {
                case 0:
                    GUILayout.BeginVertical("Box");
                    if (RGUI.Button(RenderSettings.fog, "Fog")) RenderSettings.fog = !RenderSettings.fog;
                    RenderSettings.fogColor = RGUI.ColorPicker(RenderSettings.fogColor);
                    RenderSettings.fogDensity = RGUI.SliderFloat(RenderSettings.fogDensity, 0f, 2f, 0.05f, "Fog Density");
                    RenderSettings.fogMode = RGUI.Field(RenderSettings.fogMode, "Fog Mode");
                    RenderSettings.fogStartDistance = RGUI.SliderFloat(RenderSettings.fogStartDistance, 0f, 9999f, 0f, "Fog Start Distance");
                    RenderSettings.fogEndDistance = RGUI.SliderFloat(RenderSettings.fogEndDistance, 0f, 9999f, 30f, "Fog Start Distance");
                    GUILayout.EndVertical();
                    if (directionLight)
                    {
                        GUILayout.BeginVertical("Box");
                        if (RGUI.Button(directionLight.enabled, "Light")) directionLight.enabled = !directionLight.enabled;
                        directionLight.type = RGUI.Field(directionLight.type, "Light Type");
                        directionLight.color = RGUI.ColorPicker(directionLight.color);
                        directionLight.intensity = RGUI.SliderFloat(directionLight.intensity, 0f, 100f, 0f, "Direction Intensity");
                        directionLight.shadows = RGUI.Field(directionLight.shadows, "Light Shadows");
                        GUILayout.EndVertical();
                    }
                    break;
                case 1:
                    GUILayout.BeginVertical("Box");
                    RenderSettings.ambientMode = RGUI.Field(RenderSettings.ambientMode, "Ambient Mode");
                    RenderSettings.ambientLight = RGUI.ColorPicker(RenderSettings.ambientLight);
                    RenderSettings.ambientIntensity = RGUI.SliderFloat(RenderSettings.ambientIntensity, 0f, 2f, 0.05f, "Ambient Intensity");
                    RenderSettings.ambientEquatorColor = RGUI.ColorPicker(RenderSettings.ambientEquatorColor, "Equator");
                    RenderSettings.ambientGroundColor = RGUI.ColorPicker(RenderSettings.ambientGroundColor, "Ground");
                    RenderSettings.ambientSkyColor = RGUI.ColorPicker(RenderSettings.ambientSkyColor, "Sky");
                    GUILayout.EndVertical();
                    break;
            }

            page = RGUI.Page(page, 1, true);
        }

        public override string Title => "Lighting";
    }
}
