using SaikoMod.Helper;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaikoMod.Core.Components {
    public class ESP : MonoBehaviour {
        public string objName;
        public Color color = Color.white;
        public bool hasDistance = true;
        public bool stopDraw = false;
        public bool smartName = false;

        public bool onRunFirst = false;
        public Action<GameObject> onRefresh;

        public List<GameObject> targets = new List<GameObject>();

        const float SLOW_UPDATE_RATE = 2.5f;

        Camera _cam;
        LayerMask _interactLayer = -1;
        float _slowUpdateTimer;

        readonly HashSet<GameObject> _targetSet = new HashSet<GameObject>();

        void Start() {
            _cam = Camera.main;
            _interactLayer = LayerMask.NameToLayer("Interact");

            if (_interactLayer == -1) Debug.LogWarning("ESP: Layer 'Interact' was not found.");
        }

        void Update() {
            if (!onRunFirst || !enabled) return;

            _slowUpdateTimer += Time.deltaTime;

            if (_slowUpdateTimer < SLOW_UPDATE_RATE) return;

            _slowUpdateTimer -= SLOW_UPDATE_RATE;
            Refresh();
        }

        public void Refresh() {
            if (_interactLayer == -1) return;

            _targetSet.Clear();

            Collider[] colliders = FindObjectsOfType<Collider>();

            for (int i = 0; i < colliders.Length; i++) {
                Collider col = colliders[i];

                if (col == null) continue;

                GameObject obj = col.gameObject;

                if (obj == null || obj.layer != _interactLayer) continue;
                if (!obj.activeInHierarchy) continue;
                if (!_targetSet.Add(obj)) continue;

                onRefresh?.Invoke(obj);
            }

            CleanupTargets();

            onRunFirst = true;
        }

        void CleanupTargets() {
            for (int i = targets.Count - 1; i >= 0; i--) {
                GameObject target = targets[i];
                if (target == null || !_targetSet.Contains(target)) targets.RemoveAt(i);
            }
        }

        void OnGUI() {
            if (stopDraw || _cam == null || targets.Count == 0) return;

            GUI.color = color;
            GUIStyle style = GUI.skin.label;
            TextAnchor oldAlignment = style.alignment;
            style.alignment = TextAnchor.MiddleCenter;

            for (int i = targets.Count - 1; i >= 0; i--) {
                GameObject target = targets[i];

                if (target == null) {
                    targets.RemoveAt(i);
                    continue;
                }

                if (!UnityHelpers.IsVisiblyActive(target)) continue;

                Vector3 screenPosition = _cam.WorldToScreenPoint(target.transform.position);
                if (screenPosition.z <= 0f)  continue;

                string name = smartName ? SaikoHelpers.GetSmartName(target) : objName;
                string distance = hasDistance ? " [" + (int)Vector3.Distance(_cam.transform.position, target.transform.position ) + "m]" : string.Empty;

                string text = "<b>" + name + distance + "</b>";
                GUI.Label(new Rect(screenPosition.x - 50f, Screen.height - screenPosition.y - 20f, 100f, 40f), text);
            }

            style.alignment = oldAlignment;
            GUI.color = Color.white;
        }

        void OnDestroy() {
            targets.Clear();
            _targetSet.Clear();
        }
    }
}