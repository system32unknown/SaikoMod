using System;
using System.Reflection;
using UnityEngine;

namespace SaikoMod.Helper {
    public static class SaikoHelpers {
        public static string GetSmartName(GameObject obj) {
            BindingFlags publicFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            try {
                foreach (Component c in obj.GetComponentsInChildren<Component>(false)) {
                    if (c == null) continue;

                    Type type = c.GetType();
                    string typeName = type.Name;

                    if (typeName.Contains("TextMeshPro") || typeName.Contains("TMP_Text")) {
                        PropertyInfo prop = type.GetProperty("text", publicFlag);
                        if (prop != null && prop.CanRead) {
                            object val = prop.GetValue(c, null);
                            if (val != null) return val.ToString();
                        }

                        FieldInfo field = type.GetField("m_text", publicFlag);
                        if (field != null) {
                            object val = field.GetValue(c);
                            if (val != null) return val.ToString();
                        }
                    }
                }
            } catch { } // Ignore reflection errors

            return obj.name.Replace("(Clone)", "").Replace("Door_", "").Replace("Drop_", "").Replace("Key", "").Replace("journal", "Diary").Trim();
        }
    }
}