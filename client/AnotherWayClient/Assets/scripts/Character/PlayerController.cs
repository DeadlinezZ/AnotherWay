using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private CharacterController controller;
	private Animator animator;
	public Camera playerCamera;
	public int speed;
	private Vector3 lastPos;
	private float magni;
	private GameObject speer;
	public Transform hand;
	public Transform spine;
	
	// Use this for initialization
	void Start ()
	{
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		speer = GameObject.Find("Speer");
	}

	//WICHTIG ANIMATOR DARF KEINE ROOT MOTION EINGESCHALTEN HABEN
	void FixedUpdate ()
	{
		Quaternion qa = new Quaternion(0,playerCamera.transform.rotation.y,0,playerCamera.transform.rotation.w);
		lastPos = controller.transform.position;
		animator.SetBool("attack",false);

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
			
		}else if(Input.GetKey(KeyCode.F)){
			speer.transform.position = hand.position;
			speer.transform.rotation = hand.rotation;
			speer.transform.parent = hand;
		}else if(Input.GetMouseButton(0)){
			animator.SetBool("attack",true);
		}

		magni = (controller.transform.position - lastPos).magnitude/Time.deltaTime;
		animator.SetFloat("speed",magni * 100);

		//Rotates the player
		transform.rotation = qa;
	}
}
