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
    }
}