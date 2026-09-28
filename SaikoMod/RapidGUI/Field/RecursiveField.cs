using System;
using System.Text;
using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        static object RecursiveField(object obj) {
            return DoRecursiveSafe(obj, () => DoRecursiveField(obj));
        }

        static object DoRecursiveField(object obj) {
            if (obj is IDoGUI doGuiObj) {
                GUILayout.EndHorizontal();
                using (new PrefixLabelIndentScope()) doGuiObj.DoGUI();
                GUILayout.BeginHorizontal();
            } else {
                Type type = obj.GetType();
                if (TypeUtility.IsMultiLine(type)) {
                    GUILayout.EndHorizontal();
                    using (new PrefixLabelIndentScope()) DoFields(obj, type);
                    GUILayout.BeginHorizontal();
                } else {
                    float tmp = PrefixLabelSetting.width;
                    PrefixLabelSetting.width = 0f;

                    DoFields(obj, type);

                    GUILayout.FlexibleSpace();
                    PrefixLabelSetting.width = tmp;
                }
            }

            return obj;
        }

        static readonly StringBuilder tmpStringBuilder = new StringBuilder();
        static void DoFields(object obj, Type type) {
            System.Collections.Generic.List<TypeUtility.MemberWrapper> infos = TypeUtility.GetMemberInfoList(type);
            for (int i = 0; i < infos.Count; ++i) {
                TypeUtility.MemberWrapper info = infos[i];
                if (CheckIgnoreField(info.Name)) continue;

                object v = info.GetValue(obj);
                string elemName = CheckCustomLabel(info.Name) ?? info.label;

                // for the bug that short label will be strange word wrap at unity2019
                tmpStringBuilder.Clear();
                tmpStringBuilder.Append(elemName);
                tmpStringBuilder.Append(" ");

                v = Field(v, info.MemberType, tmpStringBuilder.ToString());
                info.SetValue(obj, v);
            }
        }
    }
}