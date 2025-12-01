using System;
using System.IO;
using System.Security.Cryptography;

namespace SaikoMod.Utils
{
    class PathUtils
    {
		public static string GetFileHash(string filePath)
		{
			string text;
			if (!File.Exists(filePath))
			{
				text = null;
			}
			else
			{
				string text2;
				using (MD5 md = MD5.Create())
				{
					using (FileStream fileStream = File.OpenRead(filePath))
					{
						text2 = BitConverter.ToString(md.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
					}
				}
				text = text2;
			}
			return text;
		}

		public static bool FileInUse(string filePath)
		{
			createDir(filePath);
			if (File.Exists(filePath))
			{
				try
				{
					using (File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
					{
					}
				}
				catch (IOException)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		public static bool createDir(string path)
		{
			try
			{
				string directoryName = Path.GetDirectoryName(path);
				if (directoryName.Length > 0 && !Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
