using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.AI;

namespace SaikoMod.Core.Components {
    public class SaikoTracker : MonoBehaviour {
        static LineRenderer lr;
        NavMeshPath path;

        public Transform from;
        public Transform to;

        static bool _RenderTop = false;
        public static bool RenderTop {
            get {
                return _RenderTop;
            }
            set {
                lr?.material.SetInt("_ZTest", (int)(value ? CompareFunction.Always : CompareFunction.LessEqual));
                _RenderTop = value;
            }
        }

        static bool _UpdateTracker = false;
        public static bool UpdateTracker {
            get {
                return _UpdateTracker;
            }
            set {
                lr.enabled = value;
                _UpdateTracker = value;
            }
        }
        float updateTimer = 0.0f;
        public static float updateRate = 10f;

        void Start() {
            lr = gameObject.AddComponent<LineRenderer>();
            path = new NavMeshPath();

            Material line_Material = new Material(Shader.Find("Hidden/Internal-Colored"));
            line_Material.renderQueue = 3999;                                                                       
            line_Material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            line_Material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            line_Material.SetInt("_Cull", (int)CullMode.Off);
            line_Material.SetInt("_ZWrite", 0);
            lr.material = line_Material;
            lr.useWorldSpace = true;
            lr.sortingOrder = 100;

            lr.startColor = Color.red;
            lr.endColor = Color.green;

            lr.endWidth = lr.startWidth = .1f;
        }

        void Update() {
            if (UpdateTracker) {
                updateTimer -= Time.deltaTime;
                if (updateTimer <= 0.0f) {
                    GeneratePath(from.position, to.position);
                    updateTimer = updateRate;
                }
            }
        }

        void GeneratePath(Vector3 from, Vector3 to) {
            NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
            Vector3[] corners = path.corners;
            lr.positionCount = corners.Length;
            lr.SetPositions(corners);
        }
    }
}
