using UnityEngine;

namespace SaikoMod.Core.Components {
    public class FPSDisplay : MonoBehaviour {
        void OnGUI() {
            if (!ModBase.instance.showFPSDisplay.Value) return;
            Framerate();
            GUI.Label(new Rect(2f, 2f, 100f, 20f), fps_lastFramerate.ToString("#") + "fps");
        }

        void Framerate() {
            if (fps_timeCounter < fps_refreshTime) {
                fps_timeCounter += Time.deltaTime;
                fps_frameCounter++;
                return;
            }

            fps_lastFramerate = fps_frameCounter / fps_timeCounter;
            fps_frameCounter = 0;
            fps_timeCounter = 0f;
        }

        public float fps_timeCounter;
        public float fps_refreshTime = 1f;
        public float fps_lastFramerate;
        public int fps_frameCounter;
    }
}
