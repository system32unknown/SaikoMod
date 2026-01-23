using System;
using System.Collections.Generic;
using UnityEngine;

namespace RapidGUI {
    using FieldFunc = Func<object, Type, object>;

    public static partial class RGUI {
        // dummy GUIStyle.none.
        // unity is optimized to GUIStyle.none.
        // it seems to occur indent mismatch for complex Vertical/Horizontal Scope.
        static readonly GUIStyle styleNone = new GUIStyle(GUIStyle.none);

        public static T Field<T>(T v, string label = null, params GUILayoutOption[] options) => Field(v, label, styleNone, options);

        public static T Field<T>(T v, string label, GUIStyle style, params GUILayoutOption[] options) {
            Type type = typeof(T);
            return (T)Convert.ChangeType(Field(v, type, label, style, options), type);
        }

        public static object Field(object obj, Type type, string label = null, params GUILayoutOption[] options) => Field(obj, type, label, GUIStyle.none, options);

        public static object Field(object obj, Type type, string label, GUIStyle style, params GUILayoutOption[] options) {
            return DoField(obj, type, label, DispatchFieldFunc(type), options);
        }

        static object DoField(object obj, Type type, string label, FieldFunc fieldFunc, GUILayoutOption[] options) {
            using (new GUILayout.VerticalScope(options))
            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label("<b>" + label + "</b>");
                obj = fieldFunc(obj, type);
            }

            return obj;
        }

        static readonly Dictionary<Type, FieldFunc> fieldFuncTable = new Dictionary<Type, FieldFunc>()
        {
            {typeof(bool), new FieldFunc((obj,t) => BoolField(obj)) }
        };

        static FieldFunc DispatchFieldFunc(Type type) {
            if (!fieldFuncTable.TryGetValue(type, out var func)) {
                if (type.IsEnum) {
                    func = new FieldFunc((obj, t) => EnumField(obj));
                } else if (TypeUtility.IsList(type)) {
                    func = ListField;
                } else if (TypeUtility.IsRecursive(type)) {
                    func = new FieldFunc((obj, _) => RecursiveField(obj));
                } else {
                    func = StandardField;
                }

                fieldFuncTable[type] = func;
            }

            return func;
        }
    }
}