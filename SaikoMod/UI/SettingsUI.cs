using UnityEngine;
using System.Linq;
using System.Text;
using RapidGUI;

namespace SaikoMod.UI {
    public class SettingsUI : BaseWindowUI {
        public override string Title => "Settings";

        bool allPoint = false;

        string _softInfo = "";
        void GetSoftware() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Unity Version: " + Application.unityVersion);
            sb.AppendLine("Platform: " + Application.platform.ToString());
            sb.AppendLine("Max FPS: " + (Application.targetFrameRate > 0 ? Application.targetFrameRate.ToString() : "Unlimited"));

            _softInfo = sb.ToString();
        }

        public void OnLoad()
        {
            if (allPoint)
            {
                foreach (Texture2D tex in Resources.FindObjectsOfTypeAll<Texture2D>().Where(x => x.filterMode != FilterMode.Point).ToArray()) tex.filterMode = FilterMode.Point;
                foreach (RenderTexture tex in Resources.FindObjectsOfTypeAll<RenderTexture>().Where(x => x.filterMode != FilterMode.Point).ToArray()) tex.filterMode = FilterMode.Point;
            }
            GetSoftware();
        }

        public override void Draw()
        {
            if (RGUI.Button(allPoint, "All Points")) allPoint = !allPoint;
            GUILayout.BeginVertical("Box");
            GUILayout.Label(_softInfo);
            GUILayout.EndVertical();
        }
    }
}