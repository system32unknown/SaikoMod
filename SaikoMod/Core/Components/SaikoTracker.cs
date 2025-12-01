using UnityEngine;
using UnityEngine.AI;

namespace SaikoMod.Core.Components {
    public class SaikoTracker : MonoBehaviour {
        static LineRenderer lr;

        public Transform from;
        public Transform to;

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

            Material line_Material = new Material(Shader.Find("Sprites/Default"));
            line_Material.renderQueue = 3999;
            lr.material = line_Material;
            lr.useWorldSpace = true;

            lr.startColor = Color.red;
            lr.endColor = Color.green;

            lr.endWidth = lr.startWidth = .1f;
        }

        void Update() {
            if (UpdateTracker) {
                updateTimer -= Time.deltaTime;
                if (updateTimer <= 0.0f)
                {
                    GeneratePath(from.position, to.position);
                    updateTimer = updateRate;
                }
            }
        }

        void GeneratePath(Vector3 from, Vector3 to) {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
            Vector3[] corners = path.corners;
            lr.positionCount = corners.Length;
            lr.SetPositions(corners);
        }
    }
}
