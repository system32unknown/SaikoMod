using UnityEngine;

public class Billboard : MonoBehaviour
{
	void Start() => m_Camera = Camera.main;
	void LateUpdate() => transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward);

	Camera m_Camera;
}
