using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SaikoMod.Core.Components {
    public class LevelLoader : MonoBehaviour {
        public Text loadingText;
        public string loadingPrefix = "";

        public void LoadLevel(int sceneIdx) {
            StartCoroutine(LoadAsynchronously(sceneIdx));
        }

        IEnumerator LoadAsynchronously(int sceneIdx) {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneIdx);
            while (!op.isDone) {
                float progress = Mathf.Clamp01(op.progress / .9f);
                loadingText.text = loadingPrefix + $"{progress * 100f:0.0}%";
                yield return null;
            }
        }
    }
}