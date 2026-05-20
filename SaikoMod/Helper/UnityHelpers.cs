using UnityEngine;
using System;
using System.Linq;

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
    }
}
