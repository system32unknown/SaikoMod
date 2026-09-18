using SaikoMod.Helper;
using System.Collections.Generic;
using UnityEngine;

namespace SaikoMod.Core.Components {
    public class ESP : MonoBehaviour {
        public string objName;
        public Color color;
        public bool hasDistance = true;
        public bool stopDraw = false;
        public bool smartName = false;

        LayerMask interactLayerIndex = -1;

        public bool onRunFirst = false;
        public System.Action<GameObject> onRefresh;
        float slowUpdateTimer = 0f;
        const float SLOW_UPDATE_RATE = 2.5f;

        Camera _cam;
        public List<GameObject> targets = new List<GameObject>();

        void Start() {
            if (_cam == null) _cam = Camera.main;
            if (interactLayerIndex == -1) interactLayerIndex = LayerMask.NameToLayer("Interact");
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

        public void Refresh() {
            targets.Clear();

            foreach (Collider col in FindObjectsOfType<Collider>()) {
                GameObject obj = col.gameObject;

                if (obj.layer != interactLayerIndex) continue;
                if (!obj.activeInHierarchy) continue;

                onRefresh(obj);
            }
            if (!onRunFirst) onRunFirst = true;
        }

        void Update() {
            if (onRunFirst) {
                slowUpdateTimer += Time.deltaTime;
                if (slowUpdateTimer >= SLOW_UPDATE_RATE) {
                    if (enabled) Refresh();
                    slowUpdateTimer = 0;
                }
            }
        }

        void OnDestroy() {
            targets.Clear();
        }
    }
}