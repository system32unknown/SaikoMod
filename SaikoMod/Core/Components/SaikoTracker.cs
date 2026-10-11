using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

namespace SaikoMod.Core.Components {
    [DisallowMultipleComponent]
    public class SaikoTracker : MonoBehaviour {
        LineRenderer lr;
        NavMeshPath path;
        Material lineMaterial;
        float updateTimer;

        public Transform from;
        public Transform to;

        [Header("Appearance")]
        public Color startColor = Color.red;
        public Color endColor = Color.green;
        public float lineWidth = 0.1f;

        [Header("Path Settings")]
        public static float updateRate = 0.1f;
        public bool hideWhenUnreachable = true;

        static bool _RenderTop = false;
        public static bool RenderTop {
            get => _RenderTop;
            set {
                _RenderTop = value;
                foreach (SaikoTracker tracker in FindObjectsOfType<SaikoTracker>()) tracker.ApplyRenderMode();
            }
        }

        static bool _UpdateTracker = false;
        public static bool UpdateTracker {
            get => _UpdateTracker;
            set {
                _UpdateTracker = value;
                foreach (SaikoTracker tracker in FindObjectsOfType<SaikoTracker>()) {
                    if (tracker.lr != null) tracker.lr.enabled = value && tracker.lr.positionCount > 0;
                }
            }
        }

        void Awake() {
            path = new NavMeshPath();

            lr = gameObject.AddComponent<LineRenderer>();
            lr.useWorldSpace = true;
            lr.startWidth = lr.endWidth = lineWidth;
            lr.startColor = startColor;
            lr.endColor = endColor;
            lr.sortingOrder = 100;
            lr.positionCount = 0;
            lr.enabled = false;

            lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored")) { renderQueue = 3999 };
            lineMaterial.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);

            lr.material = lineMaterial;

            ApplyRenderMode();
        }

        void Update() {
            if (!_UpdateTracker || !enabled) return;

            if (from == null || to == null) {
                ClearPath();
                return;
            }

            updateTimer -= Time.deltaTime;
            if (updateTimer > 0f) return;
            updateTimer = Mathf.Max(.1f, updateRate);

            GeneratePath(from.position, to.position);
        }

        void GeneratePath(Vector3 start, Vector3 destination) {
            if (lr == null || path == null) return;

            if (!NavMesh.CalculatePath(start, destination, NavMesh.AllAreas, path)) {
                ClearPath();
                return;
            }

            if (path.status != NavMeshPathStatus.PathComplete) {
                if (hideWhenUnreachable) {
                    ClearPath();
                    return;
                }
            }

            SetPositions(path.corners);
        }

        void SetPositions(Vector3[] positions) {
            if (lr == null) return;

            if (positions == null || positions.Length < 2) {
                ClearPath();
                return;
            }

            lr.positionCount = positions.Length;
            lr.SetPositions(positions);
            lr.startColor = startColor;
            lr.endColor = endColor;
            lr.startWidth = lr.endWidth = lineWidth;
            lr.enabled = _UpdateTracker;

            ApplyRenderMode();
        }

        void ClearPath() {
            if (lr == null) return;
            lr.positionCount = 0;
            lr.enabled = false;
        }

        void ApplyRenderMode() {
            if (lineMaterial == null) return;
            lineMaterial.SetInt("_ZTest", (int)(_RenderTop ? CompareFunction.Always : CompareFunction.LessEqual));
        }

        void OnDisable() => ClearPath();

        void OnDestroy() {
            if (lr != null) {
                lr.positionCount = 0;
                lr.enabled = false;
                Destroy(lr);
            }

            if (lineMaterial != null) Destroy(lineMaterial);
            if (path != null) path.ClearCorners();
        }
    }
}