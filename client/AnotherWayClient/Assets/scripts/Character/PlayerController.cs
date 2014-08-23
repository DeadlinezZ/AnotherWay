using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private CharacterController controller;
	private Animator animator;
	public Camera playerCamera;
	public int speed;
	
	// Use this for initialization
	void Start ()
	{
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
	}

	//TODO
	//Animator braucht noch speed value
	//Aufeinmal funtioniert das laufen nicht mehr.
	//Von Legacy auf Humanoid gewechselt. ?=? sollte aber keinen Einfluss haben.
	// Update is called once per frame
	void FixedUpdate ()
	{
		Quaternion qa = new Quaternion(0,playerCamera.transform.rotation.y,0,playerCamera.transform.rotation.w);
		

		if(Input.GetKey(KeyCode.W)){
			Vector3 w = playerCamera.transform.TransformDirection(Vector3.forward);
			controller.SimpleMove(w * speed);
			
		}else if(Input.GetKey(KeyCode.S)) {
			Vector3 s = playerCamera.transform.TransformDirection(Vector3.back);
			controller.SimpleMove(s * speed);

			
		}else if(Input.GetKey(KeyCode.D)) {
			Vector3 d = playerCamera.transform.TransformDirection(Vector3.right);
			controller.SimpleMove(d * speed);
			
		}else if(Input.GetKey(KeyCode.A)) {
			Vector3 a = playerCamera.transform.TransformDirection(Vector3.left);
			controller.SimpleMove(a * speed);
			
		}
		//Rotates the player
		transform.rotation = qa;
	}
}
