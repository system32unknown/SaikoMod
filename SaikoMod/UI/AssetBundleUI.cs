using UnityEngine;

namespace SaikoMod.UI {
    public class AssetBundleUI : BaseWindowUI {

        public void OnLoad()
        {
        }

        public override void Draw()
        {
            GUILayout.Label("WIP");
        }
        public override string Title => "AssetBundle / Prefabs";
    }
}