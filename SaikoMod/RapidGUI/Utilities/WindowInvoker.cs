using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapidGUI {
    public static class WindowInvoker {
        static readonly HashSet<IDoGUIWindow> Windows = new HashSet<IDoGUIWindow>();

        static WindowInvoker() => RapidGUIBehaviour.Instance.onGUI += DoGUI;

        public static void Add(IDoGUIWindow window) => Windows.Add(window);
        public static void Remove(IDoGUIWindow window) => Windows.Remove(window);

        static void DoGUI() {
            Windows.ToList().ForEach(l => l?.DoGUIWindow());
            if (Event.current.type == EventType.Repaint) Windows.Clear();
        }
    }
}