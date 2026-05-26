using UnityEngine;

namespace SaikoMod.Utils
{
    class MeshUtils {
        public static void ScrambleVertices(Mesh mesh, float value) {
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++) vertices[i] += RandomUtils.GetVector3Range(value);
            mesh.vertices = vertices;
        }

        public static void ScrambleNormals(Mesh mesh, float value) {
            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++) normals[i] = RandomUtils.GetVector3Range(value).normalized;
            mesh.normals = normals;
        }

        public static void ScrambleTriangles(Mesh mesh) {
            int[] triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3) {
                int temp = triangles[i];
                triangles[i] = triangles[i + 1];
                triangles[i + 1] = triangles[i + 2];
                triangles[i + 2] = temp;
            }
            mesh.triangles = triangles;
        }
    }
}
