using UnityEngine;
using UnityEngine.Rendering;

namespace SaikoMod.Utils {
    class MaterialUtils {
        public static Material black {
            get {
                Material m = new Material(Shader.Find("Unlit/Color")) {
                    color = Color.black
                };
                m.SetInt("_SrcBlend", (int)BlendMode.One);
                m.SetInt("_DstBlend", (int)BlendMode.Zero);
                m.SetInt("_ZWrite", 1);
                m.renderQueue = (int)RenderQueue.Geometry;
                return m;
            }
        }

        public static Material CorruptMaterial() {
            Shader[] shaders = Resources.FindObjectsOfTypeAll<Shader>();
            Texture[] texs = Resources.FindObjectsOfTypeAll<Texture>();

            Material m = new Material(shaders[Random.Range(0, shaders.Length)]);
            if (m.HasProperty("_Color")) m.SetColor("_Color", RandomUtil.GetColor(true));
            m.mainTexture = texs[Random.Range(0, texs.Length)];
            return m;
        }

        /// <summary>
        /// Creates a transparent material and returns it.
        /// Tries to use the Built-in Standard shader first, falls back to common URP shader names if necessary.
        /// </summary>
        /// <param name="color">Base color including alpha (default: white with 50% alpha)</param>
        /// <param name="name">Optional material name</param>
        /// <returns>Newly created Material configured for transparency</returns>
        public static Material CreateTransparent(Color? color = null) {
            Color col = color ?? new Color(1f, 1f, 1f, 0.5f);

            // Try builtin Standard shader
            Shader shader = Shader.Find("Standard");

            // If Standard not found (e.g., URP project), try common URP/Lit names
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("URP/Lit") ?? Shader.Find("HDRP/Lit");

            // If still null, fallback to an Unlit/Transparent shader that usually exists
            if (shader == null) shader = Shader.Find("Unlit/Transparent") ?? Shader.Find("Sprites/Default");

            Material mat = new Material(shader);

            // Configure for built-in Standard shader transparency
            if (shader != null && shader.name == "Standard") {
                // 3 = Transparent mode for Standard
                mat.SetFloat("_Mode", 3f);
                mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.SetOverrideTag("RenderType", "Transparent");
                mat.renderQueue = (int)RenderQueue.Transparent;

                // Assign base color (Standard uses _Color)
                if (mat.HasProperty("_Color")) mat.SetColor("_Color", col);
            } else {
                // Try to handle URP Lit (surface type = 1 -> Transparent)
                if (shader != null && (shader.name.Contains("Universal Render Pipeline") || shader.name.Contains("URP"))) {
                    // URP: _Surface 0 = Opaque, 1 = Transparent
                    if (mat.HasProperty("_Surface")) mat.SetFloat("_Surface", 1f);

                    // URP also uses blend settings under keywords/properties, but basic settings:
                    mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.renderQueue = (int)RenderQueue.Transparent;

                    if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", col); // URP Lit property
                    else if (mat.HasProperty("_Color")) mat.SetColor("_Color", col);
                } else {
                    // Fallback: most unlit/transparent shaders accept _Color
                    if (mat.HasProperty("_Color")) {
                        mat.SetColor("_Color", col);
                    } else if (mat.HasProperty("_BaseColor")) {
                        mat.SetColor("_BaseColor", col);
                    }

                    // Try to set basic blending and render queue anyway
                    mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.renderQueue = (int)RenderQueue.Transparent;
                }
            }

            return mat;
        }
    }
}
