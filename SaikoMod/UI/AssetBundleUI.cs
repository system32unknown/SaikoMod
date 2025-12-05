using UnityEngine;
using System.Collections.Generic;
using SaikoMod.Helper;

namespace SaikoMod.UI {
    public class AssetBundleUI : BaseWindowUI {
        List<string> ObjNames = new List<string>();
        List<string> ObjFilenames = new List<string>();

        AssetBundle ObjAsset;
        List<GameObject> gameObjects = new List<GameObject>();
        const string filePath = "mods/objects/";

        public AssetBundleUI()
        {
            AssetBundleHelper.InitBundle(filePath, ".unityobj", (string ename, string filename) => {
                ObjNames.Add(ename);
                ObjFilenames.Add(filename);
            });
        }
        public void OnLoad()
        {
            foreach (string objName in ObjFilenames)
            {
                ObjAsset = AssetBundle.LoadFromFile(filePath + objName);
                gameObjects.Add(ObjAsset.LoadAllAssets<GameObject>()[0]);
            }
        }

        public void OnUnload()
        {
            gameObjects.Clear();
            AssetBundle.UnloadAllAssetBundles(true);
        }

        public override void Draw()
        {
            GUILayout.Label("WIP");
        }
        public override string Title => "AssetBundle / Prefabs";
    }
}