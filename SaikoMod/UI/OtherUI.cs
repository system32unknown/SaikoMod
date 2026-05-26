using RapidGUI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using SaikoMod.Utils;
using SaikoMod.Helper;

namespace SaikoMod.UI {
    public class OtherUI : BaseWindowUI {
        MinMaxFloat vertRange = new MinMaxFloat() {
            min = .1f,
            max = .1f
        };
        MinMaxFloat normRange = new MinMaxFloat() {
            min = .1f,
            max = .1f
        };

        int selMenu = 0;

        public YandereController yand;

        LipSyncVoice[][] voices1;
        LipSyncVoice[] voices2;

        Material[] mats;
        Texture2D[] tex2ds;

        HashSet<Material> protectedMaterials = new HashSet<Material>();

        public void OnLoad() {
            voices1 = ReflectionHelpers.GetPublicFieldsOfType<LipSyncVoice[]>(yand.facial);
            voices2 = ReflectionHelpers.GetPublicFieldsOfType<LipSyncVoice>(yand.facial);
            mats = Resources.FindObjectsOfTypeAll<Material>();
            tex2ds = Resources.FindObjectsOfTypeAll<Texture2D>();

            TMP_Text[] tmpTexts = Resources.FindObjectsOfTypeAll<TMP_Text>();
            Text[] uiTexts = Resources.FindObjectsOfTypeAll<Text>();

            foreach (TMP_Text text in tmpTexts) {
                if (text.fontMaterial != null) protectedMaterials.Add(text.fontMaterial);
                if (text.fontSharedMaterial != null) protectedMaterials.Add(text.fontSharedMaterial);
            }

            // Legacy UI Text
            foreach (Text text in uiTexts) {
                if (text.material != null) protectedMaterials.Add(text.material);
                if (text.font != null && text.font.material != null) protectedMaterials.Add(text.font.material);
                if (text.defaultMaterial != null) protectedMaterials.Add(text.defaultMaterial);
            }
        }

        public override void Draw() {
            selMenu = GUILayout.SelectionGrid(selMenu, new string[] { "Corruptions", "Fun" }, 2);
            switch (selMenu) {
                case 0:
                    GUILayout.BeginVertical("Box");
                    GUILayout.Label("Corruptions");
                    GUILayout.BeginVertical("Box");
                    vertRange.min = RGUI.SliderFloat(vertRange.min, .1f, 2f, .1f, "Vert Min");
                    vertRange.max = RGUI.SliderFloat(vertRange.max, .1f, 2f, .1f, "Vert Max");
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("Box");
                    normRange.min = RGUI.SliderFloat(normRange.min, .1f, 2f, .1f, "Norm Min");
                    normRange.max = RGUI.SliderFloat(normRange.max, .1f, 2f, .1f, "Norm Max");
                    GUILayout.EndVertical();

                    if (GUILayout.Button("Corrupt Mesh")) {
                        foreach (MeshFilter go in Resources.FindObjectsOfTypeAll<MeshFilter>()) {
                            Mesh s_mesh = go.mesh;
                            if (!s_mesh.isReadable) continue;
                            if (Random.Range(0, 5) == 2) MeshUtils.ScrambleVertices(s_mesh, Random.Range(vertRange.min, vertRange.max));
                            if (Random.Range(0, 5) == 2) MeshUtils.ScrambleNormals(s_mesh, Random.Range(normRange.min, normRange.max));
                            if (Random.Range(0, 5) == 2) MeshUtils.ScrambleTriangles(s_mesh);
                            if (Random.Range(0, 5) == 2) s_mesh.RecalculateBounds();
                        }
                    }

                    if (GUILayout.Button("Corrupt Material")) {
                        foreach (Material _mat in mats) {
                            if (_mat == null) continue;

                            if (protectedMaterials.Contains(_mat)) continue;
                            if (IsBuiltInMaterial(_mat)) continue;

                            _mat.color = RandomUtils.GetColor(true);

                            Texture curTex = tex2ds[Random.Range(0, tex2ds.Length)];
                            if (curTex != null) {
                                _mat.mainTexture = curTex;
                                curTex.filterMode = RandomUtils.RandomEnum<FilterMode>();
                                curTex.wrapMode = RandomUtils.RandomEnum<TextureWrapMode>();
                            }

                            _mat.mainTextureScale = RandomUtils.GetVector2(max: 10f);
                            _mat.mainTextureOffset = RandomUtils.GetVector2(max: 10f);
                        }
                    }

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Corrupt Audios")) {
                        AudioClip[] clips = Resources.FindObjectsOfTypeAll<AudioClip>();
                        foreach (AudioSource source in Resources.FindObjectsOfTypeAll<AudioSource>()) {
                            source.pitch = Random.Range(-3f, 3f);
                            source.clip = clips[Random.Range(0, clips.Length - 1)];
                            source.loop = RandomUtils.GetBool();
                        }
                    }

                    if (GUILayout.Button("Corrupt Voices")) {
                        foreach (LipSyncVoice[] voices in voices1) LipSyncUtils.Shufflevoices(voices);
                        foreach (LipSyncVoice voices in voices2) LipSyncUtils.Shufflevoice(voices);
                    }
                    if (GUILayout.Button("Empty Voices")) {
                        foreach (LipSyncVoice[] voices in voices1) LipSyncUtils.SetEmptyDatas(voices);
                        foreach (LipSyncVoice voices in voices2) LipSyncUtils.SetEmptyData(voices);
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    break;

                case 1:
                    break;
            }
        }

        bool IsBuiltInMaterial(Material mat) {
            if (mat == null) return true;

            if ((mat.hideFlags & HideFlags.NotEditable) != 0) return true;
            if ((mat.hideFlags & HideFlags.HideAndDontSave) != 0) return true;

#if UNITY_EDITOR
            string path = UnityEditor.AssetDatabase.GetAssetPath(mat);
            if (string.IsNullOrEmpty(path)) return true;
            if (path.StartsWith("Resources/unity_builtin")) return true;
#endif

            // TMP default materials
            if (TMP_Settings.defaultFontAsset != null) {
                if (mat == TMP_Settings.defaultFontAsset.material) return true;
            }

            if (mat.shader != null) {
                string shaderName = mat.shader.name;

                if (shaderName.StartsWith("Hidden/")) return true;

                // TMP shaders
                if (shaderName.Contains("TextMeshPro")) return true;
                if (shaderName.Contains("TMP")) return true;

                // Unity UI shaders
                if (shaderName.Contains("UI/")) return true;
                if (shaderName.Contains("Text")) return true;
            }

            return false;
        }

        public override string Title => "Other";
    }
}
