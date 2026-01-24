using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SaikoMod.Helper {
    public static class ReflectionHelpers {
        /// <summary>
        /// Use sparingly, as these are not cached, and will waste memory if called constantly.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <param name="setTo"></param>
        public static void ReflectionSetVariable(this object me, string name, object setTo) {
            AccessTools.Field(me.GetType(), name).SetValue(me, setTo);
        }

        /// <summary>
        /// Use sparingly, as these are not cached, and will waste memory if called constantly.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object ReflectionGetVariable(this object me, string name) {
            return AccessTools.Field(me.GetType(), name).GetValue(me);
        }

        /// <summary>
        /// Use sparingly, as these are not cached, and will waste memory if called constantly.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ReflectionInvoke(this object obj, string methodName, params object[] methodParams) {
            Type[] array;
            if (methodParams == null) array = null;
            else array = methodParams.Select((object p) => p.GetType()).ToArray();

            Type[] array2 = array ?? new Type[0];
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            MethodInfo methodInfo = null;

            Type type = obj.GetType();
            while (methodInfo == null && type != null) {
                methodInfo = type.GetMethod(methodName, bindingFlags, Type.DefaultBinder, array2, null);
                type = type.BaseType;
            }
            if (methodInfo == null) return null;
            return methodInfo.Invoke(obj, methodParams);
        }

        /// <summary>
        /// Returns all public fields in 'targetType' that match 'fieldType'.
        /// </summary>
        public static T[] GetPublicFieldsOfType<T>(object instance) {
            if (instance == null) return Array.Empty<T>();

            List<T> list = new List<T>();
            FieldInfo[] fields = instance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields) {
                if (field.FieldType == typeof(T)) {
                    list.Add((T)field.GetValue(instance));
                }
            }

            return list.ToArray();
        }

        public static string GetNameIfExists(object obj) {
            if (obj == null) return null;

            Type type = obj.GetType();

            BindingFlags publicFlag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

            // Try to get property "name" (case-insensitive)
            PropertyInfo prop = type.GetProperty("name", publicFlag);
            if (prop != null && prop.PropertyType == typeof(string)) return prop.GetValue(obj) as string;

            // Try field "name"
            FieldInfo field = type.GetField("name", publicFlag);
            if (field != null && field.FieldType == typeof(string)) return field.GetValue(obj) as string;

            return null;
        }
    }
}