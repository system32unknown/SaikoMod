using System;
using System.Collections.Generic;
using UnityEngine;

namespace RapidGUI {
    using FieldFunc = Func<object, Type, object>;

    public static partial class RGUI {
        public static T Field<T>(T v, string label, params GUILayoutOption[] options) {
            Type type = typeof(T);
            return (T)Convert.ChangeType(Field(v, type, label, options), type);
        }

        public static object Field(object obj, Type type, string label, params GUILayoutOption[] options) {
            return DoField(obj, type, label, DispatchFieldFunc(type), options);
        }

        static bool drawingInlineFields;
        static readonly GUILayoutOption[] inlineFieldOptions = {
            GUILayout.ExpandWidth(false),
            GUILayout.MinWidth(48f),
        };

        static object DoField(object obj, Type type, string label, FieldFunc fieldFunc, GUILayoutOption[] options) {
            using (new GUILayout.VerticalScope(options))
            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label("<b>" + label + "</b>", GUILayout.ExpandWidth(!drawingInlineFields));
                obj = fieldFunc(obj, type);
            }

            return obj;
        }

        static readonly Dictionary<Type, FieldFunc> fieldFuncTable = new Dictionary<Type, FieldFunc>() {
            {typeof(bool), new FieldFunc((obj, t) => BoolField(obj)) }
        };

        static FieldFunc DispatchFieldFunc(Type type) {
            if (!fieldFuncTable.TryGetValue(type, out FieldFunc func)) {
                if (type.IsEnum) func = new FieldFunc((obj, t) => EnumField(obj));
                else if (TypeUtility.IsList(type)) func = ListField;
                else if (TypeUtility.IsRecursive(type)) func = new FieldFunc((obj, t) => RecursiveField(obj));
                else func = StandardField;

                fieldFuncTable[type] = func;
            }

            return func;
        }
    }
}