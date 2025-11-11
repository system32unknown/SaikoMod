using UnityEngine;

namespace SaikoMod.Core.Components
{
    public class FPSDisplay : MonoBehaviour
    {
        private void OnGUI()
        {
            if (!ModBase.instance.showFPSDisplay.Value)
            {
                return;
            }
            this.Framerate();
            GUI.Label(new Rect(10f, 10f, 100f, 20f), this.fps_lastFramerate.ToString("#") + "fps");
        }

        private void Framerate()
        {
            if (this.fps_timeCounter < this.fps_refreshTime)
            {
                this.fps_timeCounter += Time.deltaTime;
                this.fps_frameCounter++;
                return;
            }
            this.fps_lastFramerate = (float)this.fps_frameCounter / this.fps_timeCounter;
            this.fps_frameCounter = 0;
            this.fps_timeCounter = 0f;
        }

        public float fps_timeCounter;

        public float fps_refreshTime = 1f;

        public float fps_lastFramerate;

        public int fps_frameCounter;
    }
}
