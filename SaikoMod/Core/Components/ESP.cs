using System.Collections.Generic;
using SaikoMod.Helper;
using UnityEngine;

namespace SaikoMod.Core.Components {
    public class ESP : MonoBehaviour {
        public string objName;
        public Color color;
        public bool hasDistance = true;
        public bool stopDraw = false;
        public bool smartName = false;

        Camera _cam;
        public List<GameObject> targets = new List<GameObject>();

        void Start() {
            if (_cam == null) _cam = Camera.main;
        }

        public void OnGUI() {
            if (_cam == null || stopDraw) return;

            for (int i = targets.Count - 1; i >= 0; i--) {
                GameObject k = targets[i];
                if (k == null) {
                    targets.RemoveAt(i);
                    continue;
                }

                if (!UnityHelpers.IsVisiblyActive(k)) continue;
                Vector3 w2s = _cam.WorldToScreenPoint(k.transform.position);
                if (w2s.z > 0) {
                    string n = smartName ? SaikoHelpers.GetSmartName(k) : objName;
                    string d = " [" + (int)Vector3.Distance(_cam.transform.position, k.transform.position) + "m]";
                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                    GUI.color = color;
                    GUI.Label(new Rect(w2s.x - 50, Screen.height - w2s.y - 20, 100, 40), "<b>" + n + (hasDistance ? d : "") + "</b>");
                }
            }
            GUI.color = Color.white;
        }

        public void OnDestroy() {
            targets.Clear();
        }
    }
}