using System;
using System.Text;
using UnityEngine;

namespace RapidGUI {
    public static partial class RGUI {
        static object RecursiveField(object obj) {
            return DoRecursiveSafe(obj, () => DoRecursiveField(obj));
        }

        static object DoRecursiveField(object obj) {
            Type type = obj.GetType();
            if (TypeUtility.IsMultiLine(type)) {
                DoFields(obj, type);
            } else {
                bool tmpInline = drawingInlineFields;
                drawingInlineFields = true;
                DoFields(obj, type);
                drawingInlineFields = tmpInline;
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

                v = Field(v, info.MemberType, tmpStringBuilder.ToString(), drawingInlineFields ? inlineFieldOptions : Array.Empty<GUILayoutOption>());
                info.SetValue(obj, v);
            }
        }
    }
}