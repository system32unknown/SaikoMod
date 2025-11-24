using UnityEngine;

namespace SaikoMod.Core.Components
{
    public class CustomCam : MonoBehaviour
    {
        public void AttachCam(Transform head)
        {
            GameObject cam = new GameObject("Camera");

            cam.AddComponent<Camera>();
            cam.transform.position = head.position;
            cam.transform.rotation = head.rotation;
            cam.transform.SetParent(head);
            cam.SetActive(true);
        }

        public Transform GetHead(string name) =>
    GameObject.Find(name).transform.Find("Armature" + (name.Contains("bunny") ? ".002" : "")).Find("mixamorig:Hips").Find("mixamorig:Spine").Find("mixamorig:Spine1")
        .Find("mixamorig:Spine2").Find("mixamorig:Neck").Find("mixamorig:Head");
    }
}
