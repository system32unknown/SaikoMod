using UnityEngine;

namespace SaikoMod.Core.Components {
    class CCTVManager {
        public static void AddMoreCam() {
            if (AddedMoreCam) return;

            CCTVCameraSystem cctv = Object.FindObjectOfType<CCTVCameraSystem>();
            if (cctv == null) return;
            Camera saikoCamera = GetHead("yandere").GetChild(2).GetComponent<Camera>();
            saikoCamera.name = "Saiko POV";
            cctv.cctvCams[cctv.cctvCams.Length - 1] = saikoCamera;
            cctv.activeCams[cctv.activeCams.Length - 1] = true;
            AddedMoreCam = true;
        }
        public static bool AddedMoreCam = false;

        public static Transform GetHead(string name) =>
    GameObject.Find(name).transform.Find("Armature").Find("mixamorig:Hips").Find("mixamorig:Spine").Find("mixamorig:Spine1")
        .Find("mixamorig:Spine2").Find("mixamorig:Neck").Find("mixamorig:Head");
    }
}
