using RogoDigital.Lipsync;
using UnityEngine;

namespace SaikoMod.Utils
{
    class LipSyncUtils
    {
        public static LipSyncData CreateData(AudioClip clip, string transcript, PhonemeMarker[] phonemeData, int phCount, int emCount)
        {
            LipSyncData syncData = ScriptableObject.CreateInstance<LipSyncData>();
            syncData.clip = clip;
            syncData.transcript = transcript;
            syncData.phonemeData = phonemeData;
            syncData.version = 1.501f;
            syncData.GenerateCurves(phCount, emCount);
            return syncData;
        }

        public static void Shufflevoices(LipSyncVoice[] voices)
        {
            AudioClip[] clips = Resources.FindObjectsOfTypeAll<AudioClip>();
            foreach (LipSyncVoice voice in voices) {
                LipSyncData data = voice.clip;
                data.clip = clips[Random.Range(0, clips.Length - 1)];
                data.transcript = voices[Random.Range(0, voices.Length - 1)].clip.transcript;
                foreach (PhonemeMarker phoneme in data.phonemeData)
                {
                    phoneme.useRandomness = RandomUtil.GetBool();
                    phoneme.intensity = Random.Range(1f, 5f);
                    phoneme.phonemeNumber = Random.Range(0, 10);
                }

                foreach (AnimationCurve curve in data.phonemePoseCurves)
                {
                    for (int i = 0; i < curve.keys.Length; i++)
                    {
                        Keyframe k = curve.keys[i];
                        k.value = Random.Range(-2f, 2f);
                        k.outTangent = Random.Range(0f, 10f);
                        k.inTangent = Random.Range(0f, 10f);
                        curve.MoveKey(i, k);
                    }
                }

                foreach (AnimationCurve curve in data.emotionPoseCurves)
                {
                    for (int i = 0; i < curve.keys.Length; i++)
                    {
                        Keyframe k = curve.keys[i];
                        k.value = Random.Range(-2f, 2f);
                        k.outTangent = Random.Range(0f, 10f);
                        k.inTangent = Random.Range(0f, 10f);
                        curve.MoveKey(i, k);
                    }
                }
            }
        }

        public static void Shufflevoice(LipSyncVoice voice)
        {
            AudioClip[] clips = Resources.FindObjectsOfTypeAll<AudioClip>();
            LipSyncData data = voice.clip;
            data.clip = clips[Random.Range(0, clips.Length - 1)];
            foreach (PhonemeMarker phoneme in data.phonemeData)
            {
                phoneme.useRandomness = RandomUtil.GetBool();
                phoneme.intensity = Random.Range(1f, 5f);
                phoneme.phonemeNumber = Random.Range(0, 10);
            }

            foreach (AnimationCurve curve in data.phonemePoseCurves)
            {
                for (int i = 0; i < curve.keys.Length; i++)
                {
                    Keyframe k = curve.keys[i];
                    k.value = Random.Range(-2f, 2f);
                    k.outTangent = Random.Range(0f, 10f);
                    k.inTangent = Random.Range(0f, 10f);
                    curve.MoveKey(i, k);
                }
            }

            foreach (AnimationCurve curve in data.emotionPoseCurves)
            {
                for (int i = 0; i < curve.keys.Length; i++)
                {
                    Keyframe k = curve.keys[i];
                    k.value = Random.Range(-2f, 2f);
                    k.outTangent = Random.Range(0f, 10f);
                    k.inTangent = Random.Range(0f, 10f);
                    curve.MoveKey(i, k);
                }
            }
        }
    }
}
