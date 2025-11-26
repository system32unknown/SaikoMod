using System;
using UnityEngine;

namespace SaikoMod.Core
{
	public class Converter
	{
		public class Texture : IDisposable
		{
			public Texture(byte[] imageData)
			{
				this.tex = new Texture2D(1, 1);
				ImageConversion.LoadImage(this.tex, imageData);
				this.tex.filterMode = FilterMode.Point;
			}

			public Texture(Color32 color)
			{
				Color[] array = new Color[] { color, color, color, color };
				this.tex = new Texture2D(2, 2);
				this.tex.SetPixels(array);
				this.tex.Apply();
			}

			public Texture(Color color) : this((Color32)color) {}

			public void Dispose()
			{
				UnityEngine.Object.Destroy(this.tex);
			}

			public Texture2D tex;
		}
	}
}
