using System;
using UnityEngine;

namespace RapidGUI {
    public class RapidGUIBehaviour : MonoBehaviour {
        #region static 
        static RapidGUIBehaviour instance;
        public static RapidGUIBehaviour Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<RapidGUIBehaviour>();
                    if (instance == null) {
                        GameObject ga = new GameObject("RapidGUI");
                        instance = ga.AddComponent<RapidGUIBehaviour>();
                    }

                    if (Application.isPlaying) DontDestroyOnLoad(instance);
                }

                return instance;
            }
        }
        #endregion

        public Action onGUI;
        public void OnGUI() => onGUI?.Invoke();
    }
}