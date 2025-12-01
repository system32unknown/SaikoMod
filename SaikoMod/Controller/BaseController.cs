using UnityEngine;
using RapidGUI;
using UnityEngine.SceneManagement;

namespace SaikoMod.Controller
{
    public abstract class BaseController : MonoBehaviour, IDoGUI {
        void Start() {
            SceneManager.sceneLoaded += OnSceneLoad;
            SceneManager.sceneUnloaded += OnSceneUnload;
        }
        void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoad;
        void OnApplicationQuit() => SceneManager.sceneUnloaded -= OnSceneUnload;

        void OnGUI() {
            if (transform.parent == null) DoGUI();
        }

        public abstract void OnSceneLoad(Scene scene, LoadSceneMode loadMode);
        public abstract void OnSceneUnload(Scene scene);
        public abstract void DoGUI();
    }
}
