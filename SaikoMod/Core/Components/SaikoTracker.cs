using UnityEngine;
using UnityEngine.AI;

namespace SaikoMod.Components {
    public class SaikoTracker : MonoBehaviour {
        LineRenderer lr;
        public PlayerController pc;

        public static bool updateTracker = false;
        float updateTimer = 0.0f;
        public static float updateRate = 10f;

        void Start() {
            lr = pc.yandereController.gameObject.AddComponent<LineRenderer>();

            Material line_Material = new Material(Shader.Find("Sprites/Default"));
            line_Material.renderQueue = 3999;
            lr.material = line_Material;

            lr.startColor = Color.red;
            lr.endColor = Color.green;

            lr.endWidth = lr.startWidth = .1f;
        }

        void Update() {
            if (updateTracker)
            {
                updateTimer -= Time.deltaTime;
                if (updateTimer <= 0.0f)
                {
                    reloadPath();
                    updateTimer = updateRate;
                }
            }
        }

        void reloadPath() {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(pc.yandereController.transform.position, pc.transform.position, NavMesh.AllAreas, path); // Saves the path in the path variable.
            Vector3[] corners = path.corners;
            lr.positionCount = corners.Length;
            lr.SetPositions(corners);
        }
    }
}
