using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace SaikoMod.Utils
{
    class PathUtils
    {
		public static string GetFileHash(string filePath)
		{
			if (!File.Exists(filePath)) return null;

			string text2;
			using (MD5 md = MD5.Create())
			{
				using (FileStream fileStream = File.OpenRead(filePath))
				{
					text2 = BitConverter.ToString(md.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
				}
			}
			return text2;
		}

		public static bool FileInUse(string filePath)
		{
			CreateDir(filePath);
			if (!File.Exists(filePath)) return false;
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

		public static bool CreateDir(string path)
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

		public static Texture2D TextureFromFile(string path, TextureFormat format)
		{
			byte[] array = File.ReadAllBytes(path);
			Texture2D texture2D = new Texture2D(2, 2, format, false);
			texture2D.LoadImage(array);
			texture2D.filterMode = FilterMode.Point;
			texture2D.name = Path.GetFileNameWithoutExtension(path);
			return texture2D;
		}
	}
}
