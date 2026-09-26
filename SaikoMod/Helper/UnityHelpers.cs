using System;
using System.Linq;
using UnityEngine;

namespace SaikoMod.Helper {
    public static class UnityHelpers {
        public static void RemoveAllComponents(GameObject obj) {
            foreach (Component component in obj.GetComponents<Component>()) {
                if (component is Transform) continue;
#if UNITY_EDITOR
                UnityEngine.Object.DestroyImmediate(component);
#else
                UnityEngine.Object.Destroy(component);
#endif
            }
        }

        public static void RemoveAllComponents(GameObject obj, params Type[] exclude) {
            foreach (Component component in obj.GetComponents<Component>()) {
                if (component is Transform) continue;
                if (exclude != null && exclude.Contains(component.GetType())) continue;
#if UNITY_EDITOR
                UnityEngine.Object.DestroyImmediate(component);
#else
                UnityEngine.Object.Destroy(component);
#endif
            }
        }

        public static bool IsVisiblyActive(GameObject obj) {
            if (obj == null || !obj.activeInHierarchy) return false;
            foreach (Transform t in obj.GetComponentsInChildren<Transform>(false)) {
                if (t.gameObject.activeSelf) {
                    Renderer r = t.GetComponent<Renderer>();
                    if (r && r.enabled) return true;
                    try {
                        foreach (Component c in t.GetComponents<Component>())
                            if (c.name.Contains("TextMeshPro")) return true;
                    } catch { }
                }
            }
            return false;
        }
    }
}
