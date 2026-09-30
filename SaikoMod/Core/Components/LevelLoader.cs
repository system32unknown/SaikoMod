using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SaikoMod.Core.Components {
    public class LevelLoader : MonoBehaviour {
        public Text loadingText;
        public string loadingPrefix = "";

        public void LoadLevel(int sceneIdx) => StartCoroutine(LoadAsynchronously(sceneIdx));

        IEnumerator LoadAsynchronously(int sceneIdx) {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneIdx);
            while (!op.isDone) {
                loadingText.text = loadingPrefix + $"{Mathf.Clamp01(op.progress / .9f) * 100f:0.0}%";
                yield return null;
            }
        }
    }
}