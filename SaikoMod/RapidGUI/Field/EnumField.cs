using System;
using System.Linq;
using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        static object EnumField(object v) {
            Type type = v.GetType();
            System.Collections.Generic.List<object> enumValues = Enum.GetValues(type).Cast<object>().ToList();

            bool isFlag = type.GetCustomAttributes(typeof(FlagsAttribute), true).Any();
            if (isFlag) {
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
                string[] valueNames = enumValues.Select(value => value.ToString()).ToArray();
                {
                    idx = SelectionPopup(idx, valueNames);
                }

                v = enumValues.ElementAtOrDefault(idx);
            }
            return v;
        }
    }
}