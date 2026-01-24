using System;
using UnityEngine;

namespace SaikoMod.Utils {
    public class ConverterUtils {
        public class Texture : IDisposable {
            public Texture(byte[] imageData) {
                tex = new Texture2D(1, 1);
                ImageConversion.LoadImage(tex, imageData);
                tex.filterMode = FilterMode.Point;
            }

            public Texture(Color32 color) {
                Color[] array = new Color[] { color, color, color, color };
                tex = new Texture2D(2, 2);
                tex.SetPixels(array);
                tex.Apply();
            }

            public Texture(Color color) : this((Color32)color) { }

            public void Dispose() => UnityEngine.Object.Destroy(tex);

            public Texture2D tex;
        }
    }
}
