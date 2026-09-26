using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidGUI {
    public static partial class TypeUtility {
        static readonly string ListInterfaceStr = "IList`1";

        public static Type GetListInterface(Type type) => type.GetInterface(ListInterfaceStr);

        public static bool IsList(Type type) => GetListInterface(type) != null;

        static readonly Dictionary<Type, bool> multiLineTable = new Dictionary<Type, bool>();
        public static bool IsMultiLine(Type type) {
            if (!multiLineTable.TryGetValue(type, out bool ret)) {
                List<MemberWrapper> infoList = GetMemberInfoList(type);

                ret = infoList.Any(info => info.range != null);
                if (!ret) {
                    IEnumerable<Type> elemtTypes = infoList.Select(info => info.MemberType);
                    ret = elemtTypes.Any(t => IsRecursive(t) || IsList(t)) || (elemtTypes.Count() > 4);
                }

                multiLineTable[type] = ret;
            }

            return ret;
        }

        static readonly Dictionary<Type, bool> isRecursiveTable = new Dictionary<Type, bool>();

        public static bool IsRecursive(Type type) {
            if (!isRecursiveTable.TryGetValue(type, out bool ret)) {
                ret = GetMemberInfoList(type).Any();
                isRecursiveTable[type] = ret;
            }
            return ret;
        }
    }
}