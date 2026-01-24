using System;
using System.Runtime.InteropServices;

namespace SaikoMod.WinAPI {
    public static class WinMessageBox {
        // MessageBox Types (Buttons)
        public enum MBButtons : uint {
            OK = 0x00000000,
            OKCancel = 0x00000001,
            AbortRetryIgnore = 0x00000002,
            YesNoCancel = 0x00000003,
            YesNo = 0x00000004,
            RetryCancel = 0x00000005,
            CancelTryContinue = 0x00000006
        }

        // MessageBox Icons
        public enum MBIcon : uint {
            None = 0x00000000,
            Error = 0x00000010,
            Question = 0x00000020,
            Warning = 0x00000030,
            Information = 0x00000040
        }

        // MessageBox Default Button
        public enum MBDefaultButton : uint {
            Button1 = 0x00000000,
            Button2 = 0x00000100,
            Button3 = 0x00000200
        }

        // MessageBox Modality Options
        public enum MBOptions : uint {
            None = 0,
            SystemModal = 0x00001000,
            TaskModal = 0x00002000,
            Help = 0x00004000,
            RightAlign = 0x00080000,
            RTLReading = 0x00100000
        }

        // MessageBox return result
        public enum MBResult : int {
            OK = 1,
            Cancel = 2,
            Abort = 3,
            Retry = 4,
            Ignore = 5,
            Yes = 6,
            No = 7,
            TryAgain = 10,
            Continue = 11
        }

        // WinAPI Import
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

        /// <summary>
        /// Shows a Windows MessageBox using WinAPI.
        /// </summary>
        public static MBResult Show(string text, string caption = "Message", MBButtons buttons = MBButtons.OK, MBIcon icon = MBIcon.None, MBDefaultButton defaultButton = MBDefaultButton.Button1, MBOptions options = MBOptions.None) {
            uint type = (uint)buttons | (uint)icon | (uint)defaultButton | (uint)options;
            int result = MessageBox(IntPtr.Zero, text, caption, type);
            return (MBResult)result;
        }

        public static bool Show(string text, MBIcon icon = MBIcon.None) {
            uint type = (uint)MBButtons.OK | (uint)icon;
            int result = MessageBox(IntPtr.Zero, text, "Saiko Mod Menu", type);
            return (MBResult)result == MBResult.OK;
        }
    }
}