using HarmonyLib;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace SaikoMod.Utils
{
    public static class ReflectionHelpers
    {
        /// <summary>
        /// Use sparingly, as these are not cached, and will waste memory if called constantly.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <param name="setTo"></param>
        public static void ReflectionSetVariable(this object me, string name, object setTo)
        {
            AccessTools.Field(me.GetType(), name).SetValue(me, setTo);
        }

        /// <summary>
        /// Use sparingly, as these are not cached, and will waste memory if called constantly.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object ReflectionGetVariable(this object me, string name)
        {
            return AccessTools.Field(me.GetType(), name).GetValue(me);
        }

        /// <summary>
        /// Use sparingly, as these are not cached, and will waste memory if called constantly.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ReflectionInvoke(this object me, string name, object[] parameters)
        {
            return AccessTools.Method(me.GetType(), name).Invoke(me, parameters);
        }

        /// <summary>
        /// Gets a single object of type T from Resources using FindObjectsOfTypeAll.
        /// Returns the first found item, or null if none exist.
        /// </summary>
        public static T GetSingleResourceOfType<T>() where T : UnityEngine.Object {
            T[] objs = UnityEngine.Resources.FindObjectsOfTypeAll<T>();
            if (objs != null && objs.Length > 0) return objs[0];

            return null;
        }

        /// <summary>
        /// Returns all public fields in 'targetType' that match 'fieldType'.
        /// </summary>
        public static T[] GetPublicFieldsOfType<T>(object instance) {
            if (instance == null) return Array.Empty<T>();

            List<T> list = new List<T>();
            FieldInfo[] fields = instance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields) {
                if (field.FieldType == typeof(T)) {
                    list.Add((T)field.GetValue(instance));
                }
            }

            return list.ToArray();
        }
    }
}