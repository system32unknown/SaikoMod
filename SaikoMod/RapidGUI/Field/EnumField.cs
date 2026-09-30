using System;
using System.Linq;
using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        static object EnumField(object v) {
            Type type = v.GetType();
            System.Collections.Generic.List<object> enumValues = Enum.GetValues(type).Cast<object>().ToList();

            if (type.GetCustomAttributes(typeof(FlagsAttribute), true).Any()) {
                ulong flagV = Convert.ToUInt64(Convert.ChangeType(v, type));
                enumValues.ForEach(value => {
                    ulong flag = Convert.ToUInt64(value);
                    if (flag > 0) {
                        bool has = (flag & flagV) == flag;
                        has = GUILayout.Toggle(has, value.ToString());
                        flagV = has ? (flagV | flag) : (flagV & ~flag);
                    }
                });

                v = Enum.ToObject(type, flagV);
            } else {
                int idx = enumValues.IndexOf(v);
                idx = SelectionPopup(idx, enumValues.Select(value => value.ToString()).ToArray());
                v = enumValues.ElementAtOrDefault(idx);
            }
            return v;
        }
    }
}