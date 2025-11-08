using HarmonyLib;
using UnityEngine;
using UnityEngine.AI;

namespace SaikoMod.Mods {
    [HarmonyPatch(typeof(YandereController))]
    public class SaikoTracker {
        static LineRenderer lr;
        static YandereController hFPS;
        static PlayerController pc;

        public static bool updateTracker = false;
        static float updateTimer = 0.0f;
        public static float updateRate = 10f;

        [HarmonyPatch("Start"), HarmonyPostfix]
        static void initTracker(YandereController __instance) {
            hFPS = __instance;
            pc = Resources.FindObjectsOfTypeAll<PlayerController>()[0];

            lr = hFPS.gameObject.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));

            lr.startColor = Color.red;
            lr.endColor = Color.green;

            lr.startWidth = 0.2f;
            lr.endWidth = 0.2f;

            updateTimer = updateRate;
        }

        [HarmonyPatch("Update"), HarmonyPostfix]
        static void upTracker() {
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

        public static void reloadPath() {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(hFPS.transform.position, pc.transform.position, NavMesh.AllAreas, path); //Saves the path in the path variable.
            Vector3[] corners = path.corners;
            lr.positionCount = corners.Length;
            lr.SetPositions(corners);
        }
    }
}
