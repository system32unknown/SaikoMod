using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using RapidGUI;

namespace SaikoMod.UI
{
    public class LightingUI : BaseWindowUI {
        int page = 0;

        Light directionLight;
        Transform playerTransform;

        Vector3 originalLightPos;
        Quaternion originalLightRot;
        public void OnLoad()
        {
            directionLight = Object.FindObjectsOfType<Light>().Where(x => x.type == LightType.Directional).First();
            playerTransform = GameObject.Find("FPSPLAYER").transform;
            originalLightPos = directionLight.transform.position;
            originalLightRot = directionLight.transform.rotation;
        }

        public void OnUpdate()
        {
            if (playerTransform && directionLight.type == LightType.Point) {
                directionLight.transform.position = playerTransform.position;
                directionLight.transform.rotation = playerTransform.rotation;
            } else if (Camera.main && directionLight.type == LightType.Spot) {
                directionLight.transform.position = Camera.main.transform.position;
                directionLight.transform.rotation = Camera.main.transform.rotation;
            }
        }

        public override void Draw()
        {
            switch (page)
            {
                case 0:
                    GUILayout.BeginVertical("Box");
                    if (RGUI.Button(RenderSettings.fog, "Fog")) RenderSettings.fog = !RenderSettings.fog;
                    RenderSettings.fogColor = RGUI.ColorPicker(RenderSettings.fogColor);
                    RenderSettings.fogMode = RGUI.Field(RenderSettings.fogMode, "Fog Mode");
                    switch (RenderSettings.fogMode)
                    {
                        case FogMode.Linear:
                            RenderSettings.fogStartDistance = RGUI.SliderFloat(RenderSettings.fogStartDistance, 0f, 9999f, 0f, "Fog Start Distance");
                            RenderSettings.fogEndDistance = RGUI.SliderFloat(RenderSettings.fogEndDistance, 0f, 9999f, 30f, "Fog Start Distance");
                            break;
                        case FogMode.Exponential:
                        case FogMode.ExponentialSquared:
                            RenderSettings.fogDensity = RGUI.SliderFloat(RenderSettings.fogDensity, 0f, 2f, 0.05f, "Fog Density");
                            break;
                    }
                    GUILayout.EndVertical();
                    if (directionLight)
                    {
                        GUILayout.BeginVertical("Box");
                        if (RGUI.Button(directionLight.enabled, "Light")) directionLight.enabled = !directionLight.enabled;
                        directionLight.type = RGUI.Field(directionLight.type, "Light Type");
                        switch (directionLight.type)
                        {
                            case LightType.Directional:
                                if (directionLight.transform.parent != null) directionLight.transform.parent = null;
                                directionLight.transform.position = originalLightPos;
                                directionLight.transform.rotation = originalLightRot;
                                break;
                            case LightType.Point:
                                if (directionLight.transform.parent != null) directionLight.transform.parent = null;
                                directionLight.range = RGUI.SliderFloat(directionLight.range, 0f, 999f, 0f, "Range");
                                break;
                            case LightType.Spot:
                                if (directionLight.transform.parent == null) directionLight.transform.parent = Camera.main.transform;
                                directionLight.range = RGUI.SliderFloat(directionLight.range, 0f, 999f, 0f, "Range");
                                directionLight.spotAngle = RGUI.SliderFloat(directionLight.spotAngle, 1f, 179f, 30f, "Spot Range");
                                break;
                        }
                        directionLight.color = RGUI.ColorPicker(directionLight.color);
                        directionLight.intensity = RGUI.SliderFloat(directionLight.intensity, 0f, 100f, 2f, "Intensity");
                        directionLight.shadows = RGUI.Field(directionLight.shadows, "Light Shadows");
                        GUILayout.EndVertical();
                    }
                    break;
                case 1:
                    GUILayout.BeginVertical("Box");
                    RenderSettings.ambientMode = RGUI.Field(RenderSettings.ambientMode, "Ambient Mode");
                    switch (RenderSettings.ambientMode)
                    {
                        case AmbientMode.Skybox:
                        case AmbientMode.Flat:
                            RenderSettings.ambientLight = RGUI.ColorPicker(RenderSettings.ambientLight);
                            break;
                        case AmbientMode.Trilight:
                            RenderSettings.ambientSkyColor = RGUI.ColorPicker(RenderSettings.ambientSkyColor, "Sky");
                            RenderSettings.ambientEquatorColor = RGUI.ColorPicker(RenderSettings.ambientEquatorColor, "Equator");
                            RenderSettings.ambientGroundColor = RGUI.ColorPicker(RenderSettings.ambientGroundColor, "Ground");
                            break;
                    }
                    GUILayout.EndVertical();
                    break;
            }

            page = RGUI.Page(page, 1, true);
        }

        public override string Title => "Lighting";
    }
}
