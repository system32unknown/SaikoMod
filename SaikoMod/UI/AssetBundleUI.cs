using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System.Collections.Generic;
using SaikoMod.Utils;
using SaikoMod.Core.Components;
using SaikoMod.Core.Lua;
using RapidGUI;

namespace SaikoMod.UI {
    public class AssetBundleUI : BaseWindowUI {
        List<string> ObjNames = new List<string>();
        List<string> ObjFilenames = new List<string>();

        AssetBundle ObjAsset;
        List<GameObject> objAssets = new List<GameObject>();
        const string filePath = "mods/objects/";

        int objIdx = 0;
        Transform playerTransform;
        GameObject gameObjParent;

        public AssetBundleUI() {
            PathUtils.ScanFolderFiles(filePath, ".unityobj", (string ename, string filename) => {
                ObjNames.Add(ename);
                ObjFilenames.Add(filename);
            });
        }

        public void OnLoad() {
            gameObjParent = new GameObject("gameObjAssets");
            playerTransform = GameObject.Find("FPSPLAYER").transform;

            foreach (string objName in ObjFilenames) {
                ObjAsset = AssetBundle.LoadFromFile(filePath + objName);
                objAssets.Add(ObjAsset.LoadAllAssets<GameObject>()[0]);
            }
        }

        public void OnUnload() {
            objAssets.Clear();
            AssetBundle.UnloadAllAssetBundles(true);
        }

        public override void Draw() {
            if (objAssets.Count <= 0) return;
            GUILayout.BeginVertical("Box");
            if (RGUI.ArrayNavigatorButton<GameObject>(ref objIdx, objAssets, "objs")) {
                GameObject curObj = objAssets[objIdx];
                if (curObj == null) return;

                GameObject cloned = Object.Instantiate(curObj, playerTransform.position, playerTransform.rotation);
                cloned.name = curObj.name;
                cloned.transform.parent = gameObjParent.transform;
                CustomDynamicObj cdo = cloned.AddComponent<CustomDynamicObj>();

                string bundleFile = ObjFilenames[objIdx]; // same index as objAssets
                string luaPath = Path.Combine(filePath, Path.GetFileNameWithoutExtension(bundleFile) + ".lua");

                LuaObjectScript lua = null;
                if (File.Exists(luaPath)) {
                    lua = cloned.AddComponent<LuaObjectScript>();
                    lua.InitFromFile(luaPath);

                    YandereController g = Object.FindObjectOfType<YandereController>();
                    lua.RegisterType<YandereController>();
                    lua.SetGlobal("yand", g);
                }

                cdo.action += () => {
                    lua?.CallAction();
                };
            }

            if (gameObjParent.transform.childCount > 0) {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Delete Object")) {
                    Transform childObj = gameObjParent.transform.GetChild(gameObjParent.transform.childCount - 1);
                    if (childObj) Object.Destroy(childObj.gameObject);
                }
                if (GUILayout.Button("Delete Objects")) {
                    for (int i = 0; i < gameObjParent.transform.childCount; i++) {
                        Object.Destroy(gameObjParent.transform.GetChild(i).gameObject);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        public override string Title => "AssetBundle";
    }
}