using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

namespace SaikoMod.Core.Components
{
    public class CamRenderer
    {
        public Camera cam;
        public Vector2Int res = new Vector2Int(800, 600);

        public byte[] CamCapture()
        {
            RenderTexture rt = null;
            if (cam.targetTexture == null)
            {
                rt = new RenderTexture(res.x, res.y, 24);
                cam.targetTexture = rt;
            }

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = cam.targetTexture;

            cam.Render();

            Texture2D Image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false);
            Image.filterMode = FilterMode.Point;
            Image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
            Image.Apply();
            RenderTexture.active = currentRT;
            cam.targetTexture = null;

            byte[] Bytes = Image.EncodeToPNG();
            UnityEngine.Object.Destroy(Image);
            UnityEngine.Object.Destroy(rt);

            return Bytes;
        }

        public IEnumerator Upload(string url)
        {
            DateTime now = DateTime.Now;
            WWWForm form = new WWWForm();
            form.AddBinaryData("image", CamCapture(), now.ToString("yyyy'_'MM'_'dd") + ".png", "image/png");

            using (UnityWebRequest req = UnityWebRequest.Post(url, form))
            {
                yield return req.SendWebRequest();

                if (req.isNetworkError || req.isHttpError) Debug.LogError("Upload failed: " + req.error);
                else Debug.Log("Upload success: " + req.downloadHandler.text);
            }
        }
    }
}