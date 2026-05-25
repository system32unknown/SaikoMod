using UnityEngine;
using FPSUtils = SaikoMod.Utils.FPSUtils;
using MathUtils = SaikoMod.Utils.MathUtils;

namespace SaikoMod.Core.Components {
    public enum FPSLagMode {
        INSTANT,
        LERP
    };

    public class FPSDisplay : MonoBehaviour {
        public FPSUtils fps;
        GUIStyle FpsStyle;

        public static FPSLagMode lagMode = FPSLagMode.LERP;

        void Awake() {
            fps = new FPSUtils();
            FpsStyle = new GUIStyle();
            FpsStyle.normal.textColor = Color.white;
        }

        void OnGUI() {
            if (!ModBase.instance.showFPSDisplay.Value) return;
            fps.Update(Time.unscaledDeltaTime * 1000f);
            switch (lagMode) {
                case FPSLagMode.INSTANT:
                    FpsStyle.normal.textColor = fps.Lagged ? Color.red : Color.white;
                    break;
                case FPSLagMode.LERP:
                    float SquaredFPS = fps.TargetFPS * .5f;
                    float green = MathUtils.Normalize(fps.CurFPS, 1f, SquaredFPS);
                    float blue = MathUtils.Normalize(fps.CurFPS, SquaredFPS, fps.TargetFPS);

                    FpsStyle.normal.textColor = new Color32(255, (byte)Mathf.RoundToInt(green * 255f), (byte)Mathf.RoundToInt(blue * 255f), 255);
                    break;
            }
            GUI.Label(new Rect(2f, 2f, 100f, 20f), $"{fps.TotalFPS:#}fps", FpsStyle);
        }
    }
}