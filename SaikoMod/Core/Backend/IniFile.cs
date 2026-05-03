using System.Runtime.InteropServices;
using System.Text;

namespace SaikoMod.Core.Backend {
    public class IniFile {
        public string Path { get; }

        public IniFile(string iniPath) {
            Path = iniPath;
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder returnValue, int size, string filePath);

        // Write a value
        public void Write(string section, string key, string value) {
            WritePrivateProfileString(section, key, value, Path);
        }

        // Read a value
        public string Read(string section, string key, string defaultValue = "") {
            StringBuilder result = new StringBuilder(255);
            GetPrivateProfileString(section, key, defaultValue, result, 255, Path);
            return result.ToString();
        }

        // Check if key exists
        public bool KeyExists(string section, string key) {
            return Read(section, key).Length > 0;
        }
    }
}