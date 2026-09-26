using System;
using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        static bool BoolField(object v) => GUILayout.Toggle(Convert.ToBoolean(v), "");
    }
}