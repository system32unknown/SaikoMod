using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using SaikoMod.Helper;
using SaikoMod.Core.Components;
using RapidGUI;

namespace SaikoMod.UI {
    public class AssetBundleUI : BaseWindowUI {
        List<string> ObjNames = new List<string>();
        List<string> ObjFilenames = new List<string>();

        AssetBundle ObjAsset;
        List<GameObject> gameObjects = new List<GameObject>();
        const string filePath = "mods/objects/";

        int objIdx = 0;
        Transform playerTransform;

        public AssetBundleUI()
        {
            AssetBundleHelper.InitBundle(filePath, ".unityobj", (string ename, string filename) => {
                ObjNames.Add(ename);
                ObjFilenames.Add(filename);
            });
        }
        public void OnLoad()
        {
            playerTransform = GameObject.Find("FPSPLAYER").transform;
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
            if (RGUI.ArrayNavigatorButton<GameObject>(ref objIdx, gameObjects, "objs"))
            {
                GameObject curObj = gameObjects[objIdx];
                if (curObj == null) return;
                GameObject cloned = Object.Instantiate(curObj, playerTransform.position, playerTransform.rotation);
                CustomDynamicObj cdo = cloned.AddComponent<CustomDynamicObj>();
                cdo.action += () => {
                    GameObject yand = GameObject.Find("yandere");
                    Vector3 originPlayer = playerTransform.position;
                    Vector3 originYand = yand.transform.position;
                    yand.GetComponent<NavMeshAgent>().enabled = false;
                    yand.transform.position = originPlayer;
                    yand.GetComponent<NavMeshAgent>().enabled = true;
                    playerTransform.position = originYand;
                    Object.Destroy(cloned);
                };
                cloned.name = curObj.name;
            }
        }
        public override string Title => "AssetBundle / Prefabs";
    }
}