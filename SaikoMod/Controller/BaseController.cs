using UnityEngine;
using RapidGUI;

namespace SaikoMod.Controller
{
    public abstract class BaseController : MonoBehaviour, IDoGUI {
        private void OnGUI()
        {
            if (transform.parent == null) DoGUI();
        }

        public abstract void DoGUI();
    }
}
