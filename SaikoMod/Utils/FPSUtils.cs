using System.Collections.Generic;
using UnityEngine;

namespace SaikoMod.Utils {
    public class FPSUtils {
        readonly List<int> times = new List<int>();
        int sum = 0;
        int sliceCnt = 0;
        int cacheCount = 0;

        /// <summary>
        /// Current FPS.
        /// </summary>
        public int CurFPS { get; private set; }

        /// <summary>
        /// Total accumulated FPS.
        /// </summary>
        public int TotalFPS { get; private set; }

        /// <summary>
        /// Raw averaged FPS.
        /// </summary>
        public float AvgFPS { get; private set; }

        public bool ClampFPS = true;

        public int TargetFPS { get; private set; }

        public FPSUtils() {
            CurFPS = 0;
            AvgFPS = 0f;
            TotalFPS = 0;
            sum = 0;
            sliceCnt = 0;

            TargetFPS = Application.targetFrameRate > 0 ? Application.targetFrameRate : Screen.currentResolution.refreshRate > 0 ? Mathf.RoundToInt(Screen.currentResolution.refreshRate) : 999;
        }

        public void Update(float dt) {
            sliceCnt = 0;

            int delta = Mathf.RoundToInt(dt);

            times.Add(delta);
            sum += delta;

            while (sum > 1000 && sliceCnt < times.Count) {
                sum -= times[sliceCnt];
                sliceCnt++;
            }

            if (sliceCnt > 0)
                times.RemoveRange(0, sliceCnt);

            int curCount = times.Count;

            TotalFPS = Mathf.FloorToInt(CurFPS + (curCount / 8f));

            if (curCount != cacheCount) {
                AvgFPS = curCount > 0  ? 1000f / ((float)sum / curCount) : 0f;

                int roundedAvgFPS = Mathf.RoundToInt(AvgFPS);
                CurFPS = ClampFPS ? Mathf.Min(roundedAvgFPS, TargetFPS) : roundedAvgFPS;
            }

            cacheCount = curCount;
        }

        /// <summary>
        /// Returns true if FPS is below half target framerate.
        /// </summary>
        public bool Lagged {
            get {
                return CurFPS < TargetFPS * .5f;
            }
        }
    }
}
