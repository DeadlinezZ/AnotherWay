using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Camera playerCamera;
	public float horizontalSpeed = 2.0F;
	public float verticalSpeed = 1.2F;
	public float yOffset = 4.0f;
	public float zOffset = 4.0f;
	
	private CharacterController controller;
	private Vector3 offset;
	
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		offset = new Vector3(controller.transform.position.x,controller.transform.position.y + yOffset,controller.transform.position.z + zOffset);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float h = horizontalSpeed * Input.GetAxis("Mouse X");
		float v = verticalSpeed * Input.GetAxis("Mouse Y");
		offset = Quaternion.AngleAxis(h,Vector3.up) * offset;
		offset = Quaternion.AngleAxis(v,Vector3.right) * offset;
		
		playerCamera.transform.position = controller.transform.position + offset;
		playerCamera.transform.LookAt(controller.transform.position);
		
		
		
	}
}
