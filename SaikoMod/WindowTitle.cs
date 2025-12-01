using System;
using System.Runtime.InteropServices;

namespace SaikoMod
{
    public static class WindowTitle {
        delegate bool EnumThreadDelegate(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("Kernel32.dll")]
        static extern int GetCurrentThreadId();

        static IntPtr GetWindowHandle() {
            IntPtr returnHwnd = IntPtr.Zero;
            int threadId = GetCurrentThreadId();
            EnumThreadWindows(threadId,
                (hWnd, lParam) => {
                    if (returnHwnd == IntPtr.Zero) returnHwnd = hWnd;
                    return true;
                }, IntPtr.Zero);
            return returnHwnd;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        static extern bool SetWindowText(IntPtr hwnd, string lpString);

        static void SetTextInternal(string text) {
            SetWindowText(GetWindowHandle(), text);
        }

        static bool osChecked = false;
        static bool enabled;

        //SET FUNCTION
        public static void SetText(string text) {
            if (!osChecked) {
                osChecked = true;
                enabled = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            }
            if (enabled) SetTextInternal(text);
        }
    }
}
