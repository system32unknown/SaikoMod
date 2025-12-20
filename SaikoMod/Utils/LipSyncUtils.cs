using RogoDigital.Lipsync;
using UnityEngine;

namespace SaikoMod.Utils
{
    class LipSyncUtils
    {
        public static LipSyncData GetSampleData(float dur, PhonemeMarker[] p, int ph, int em)
        {
            return CreateData(CreateAudioClip("TMP_AUD", dur), "*Unintelligible*", p, ph, em, true);
        }
        public static LipSyncData GetEmptyData()
        {
            return CreateData(CreateEmptyClip(0.5f), "", new PhonemeMarker[0], 0, 0);
        }

        public static LipSyncData CreateData(AudioClip clip, string transcript, PhonemeMarker[] phonemeData, int phCount, int emCount, bool generate = false)
        {
            LipSyncData syncData = ScriptableObject.CreateInstance<LipSyncData>();
            syncData.clip = clip;
            syncData.transcript = transcript;
            syncData.phonemeData = phonemeData;
            syncData.version = 1.501f;
            if (generate) syncData.GenerateCurves(phCount, emCount);
            return syncData;
        }

        public static void Shufflevoices(LipSyncVoice[] voices) {
            foreach (LipSyncVoice voice in voices) {
                voice.voiceline = voices[Random.Range(0, voices.Length - 1)].voiceline;
                LipSyncData data = voice.clip;
                Shufflevoice(voice);
                data.transcript = voices[Random.Range(0, voices.Length - 1)].clip.transcript;
            }
        }
        public static void Shufflevoice(LipSyncVoice voice)
        {
            AudioClip[] clips = Resources.FindObjectsOfTypeAll<AudioClip>();
            voice.voiceline = RandomUtil.GetString(100, true);
            LipSyncData data = voice.clip;
            data.clip = clips[Random.Range(0, clips.Length - 1)];
            foreach (PhonemeMarker phoneme in data.phonemeData) {
                phoneme.useRandomness = RandomUtil.GetBool();
                phoneme.intensity = Random.Range(1f, 5f);
                phoneme.phonemeNumber = Random.Range(0, 10);
            }

            RandomUtil.ShuffleCurve(data.phonemePoseCurves);
            RandomUtil.ShuffleCurve(data.emotionPoseCurves);
        }

        public static void SetEmptyDatas(LipSyncVoice[] voices)
        {
            foreach (LipSyncVoice voice in voices) SetEmptyData(voice);
        }
        public static void SetEmptyData(LipSyncVoice voice)
        {
            LipSyncData data = voice.clip;
            data.clip = CreateEmptyClip(.5f);
            data.transcript = voice.voiceline = "";

            foreach (AnimationCurve curve in data.phonemePoseCurves) {
                for (int i = 0; i < curve.keys.Length; i++) {
                    Keyframe k = curve.keys[i];
                    k.outTangent = k.inTangent = k.value = 0f;
                    curve.MoveKey(i, k);
                }
            }
            foreach (AnimationCurve curve in data.emotionPoseCurves) {
                for (int i = 0; i < curve.keys.Length; i++) {
                    Keyframe k = curve.keys[i];
                    k.outTangent = k.inTangent = k.value = 0f;
                    curve.MoveKey(i, k);
                }
            }
        }

        public static AudioClip CreateAudioClip(string name, float dur) {
            const int sampleR = 44100;
            int totalSamples = (int)(sampleR * dur);
            AudioClip clip = AudioClip.Create(name, totalSamples, 1, sampleR, false);

            float[] samples = new float[totalSamples];
            for (int i = 0; i < totalSamples; i++) samples[i] = Random.Range(-1f, 1f);
            clip.SetData(samples, 0);
            return clip;
        }
        public static AudioClip CreateEmptyClip(float duration) {
            const int sampleR = 44100;
            int samplesCount = (int)(duration * sampleR);
            AudioClip clip = AudioClip.Create("SilentClip", samplesCount, 1, sampleR, false);

            float[] samples = new float[samplesCount];
            for (int i = 0; i < samplesCount; i++) samples[i] = 0f; // Fill with silence
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
