using UnityEngine;
using RapidGUI;
using UnityEngine.SceneManagement;

namespace SaikoMod.Controller
{
    public abstract class BaseController : MonoBehaviour, IDoGUI {
        void Start() {
            SceneManager.sceneLoaded += onSceneLoad;
        }
        void OnDestroy() {
            SceneManager.sceneLoaded -= onSceneLoad;
        }

        void OnGUI() {
            if (transform.parent == null) DoGUI();
        }

        public abstract void onSceneLoad(Scene scene, LoadSceneMode loadMode);
        public abstract void DoGUI();
    }
}
