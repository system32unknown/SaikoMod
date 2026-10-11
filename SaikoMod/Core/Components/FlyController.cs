using UnityEngine;

namespace SaikoMod.Core.Components {
    [RequireComponent(typeof(Rigidbody))]
    public class FlyController : MonoBehaviour {
        [Header("Fly Settings")]
        public float speed = 50f;
        public float acc = 12f;

        Transform cam;
        Vector3 curVel;

        void Awake() => cam = Camera.main?.transform;
        void OnDisable() => curVel = Vector3.zero;

        void Update() {
            curVel = Vector3.Lerp(curVel, GetInputDirection() * speed, acc * Time.deltaTime);
            transform.position += curVel * Time.deltaTime;
            RotateTowardsCamera(r => transform.rotation = r);
        }

        Vector3 GetInputDirection() {
            Vector3 dir = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) dir += cam.forward;
            if (Input.GetKey(KeyCode.S)) dir -= cam.forward;
            if (Input.GetKey(KeyCode.D)) dir += cam.right;
            if (Input.GetKey(KeyCode.A)) dir -= cam.right;
            if (Input.GetKey(KeyCode.Space)) dir += Vector3.up;
            if (Input.GetKey(KeyCode.LeftControl)) dir -= Vector3.up;

            return dir.sqrMagnitude > 0f ? dir.normalized : Vector3.zero;
        }

        void RotateTowardsCamera(System.Action<Quaternion> applyRotation) {
            Vector3 forward = cam.forward;
            forward.y = 0f;
            if (forward.sqrMagnitude < 0.001f) return;

            applyRotation(Quaternion.LookRotation(forward));
        }
    }
}