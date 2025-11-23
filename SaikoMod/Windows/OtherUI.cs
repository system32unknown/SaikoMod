using RapidGUI;
using UnityEngine;
using SaikoMod.Controller;
using SaikoMod.Utils;

namespace SaikoMod.Windows
{
    public static class OtherUI
    {
        public static bool showMenu;
        public static Rect rect = new Rect(1, 1, 100, 100);

        static MinMaxFloat vertRange = new MinMaxFloat() {
            min = .1f,
            max = .1f
        };
        static MinMaxFloat normRange = new MinMaxFloat() {
            min = .1f,
            max = .1f
        };

        public static void Window(int _) {
            GUI.backgroundColor = Color.black;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            Title();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("Corruptions");
            GUILayout.BeginVertical("Box");
            vertRange.min = RGUI.SliderFloat(vertRange.min, .1f, 2f, .1f, "Vert Min");
            vertRange.max = RGUI.SliderFloat(vertRange.max, .1f, 2f, .1f, "Vert Max");
            GUILayout.EndVertical();

            GUILayout.BeginVertical("Box");
            normRange.min = RGUI.SliderFloat(normRange.min, .1f, 2f, .1f, "Norm Min");
            normRange.max = RGUI.SliderFloat(normRange.max, .1f, 2f, .1f, "Norm Max");
            GUILayout.EndVertical();

            if (GUILayout.Button("Corrupt Mesh")) {
                foreach (MeshFilter go in UnityEngine.Object.FindObjectsOfType<MeshFilter>()) {
                    try {
                        Mesh s_mesh = go.mesh;
                        if (!s_mesh.isReadable) continue;
                        if (Random.Range(0, 5) == 2) MeshUtils.ScrambleVertices(s_mesh, Random.Range(vertRange.min, vertRange.max));
                        if (Random.Range(0, 5) == 2) MeshUtils.ScrambleNormals(s_mesh, Random.Range(normRange.min, normRange.max));
                        if (Random.Range(0, 5) == 2) MeshUtils.ScrambleTriangles(s_mesh);
                        if (Random.Range(0, 5) == 2) s_mesh.RecalculateBounds();
                    }
                    catch { }
                }
            }
            GUILayout.EndVertical();
        }

        static void Title() {
            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>Other Mods</b>", GUILayout.Height(21f));
            if (GUILayout.Button("<b>X</b>", GUILayout.Height(19f), GUILayout.Width(32f)))
            {
                UIController.Instance.MenuTab = Core.Enums.MenuTab.Off;
            }
            GUILayout.EndHorizontal();
        }
    }
}
