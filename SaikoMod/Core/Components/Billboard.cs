using UnityEngine;

public class Billboard : MonoBehaviour {
    void Start() => cam = Camera.main;
    void LateUpdate() {
        camRot.eulerAngles = Vector3.up * cam.transform.rotation.eulerAngles.y + Vector3.right * cam.transform.rotation.eulerAngles.x;
        transform.LookAt(transform.position + camRot * Vector3.forward, transform.up);
    }

    Quaternion camRot;
    Camera cam;
}