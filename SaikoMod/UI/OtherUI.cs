using RapidGUI;
using UnityEngine;
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

        int page = 0;

        public YandereController yand;

        LipSyncVoice[][] voices1;
        LipSyncVoice[] voices2;

        public void OnLoad() {
            voices1 = ReflectionHelpers.GetPublicFieldsOfType<LipSyncVoice[]>(yand.facial);
            voices2 = ReflectionHelpers.GetPublicFieldsOfType<LipSyncVoice>(yand.facial);
        }

        public override void Draw() {
            switch (page) {
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
                        foreach (MeshFilter go in Object.FindObjectsOfType<MeshFilter>()) {
                            Mesh s_mesh = go.mesh;
                            if (!s_mesh.isReadable) continue;
                            if (Random.Range(0, 5) == 2) MeshUtils.ScrambleVertices(s_mesh, Random.Range(vertRange.min, vertRange.max));
                            if (Random.Range(0, 5) == 2) MeshUtils.ScrambleNormals(s_mesh, Random.Range(normRange.min, normRange.max));
                            if (Random.Range(0, 5) == 2) MeshUtils.ScrambleTriangles(s_mesh);
                            if (Random.Range(0, 5) == 2) s_mesh.RecalculateBounds();
                        }
                    }

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Corrupt Audios")) {
                        AudioClip[] clips = Resources.FindObjectsOfTypeAll<AudioClip>();
                        foreach (AudioSource source in Resources.FindObjectsOfTypeAll<AudioSource>()) {
                            source.pitch = Random.Range(-3f, 3f);
                            source.clip = clips[Random.Range(0, clips.Length - 1)];
                            source.loop = RandomUtil.GetBool();
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
            page = RGUI.Page(page, 3, true);
        }

        public override string Title => "Other";
    }
}
