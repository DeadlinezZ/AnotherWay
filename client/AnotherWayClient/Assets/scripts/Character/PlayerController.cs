using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private CharacterController controller;
	private Animator animator;
	public Camera playerCamera;
	public int speed;
	private Vector3 lastPos;
	private float magni;
	private GameObject weapon;
	public Transform hand;
	public Transform spine;
	
	// Use this for initialization
	void Start ()
	{
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		weapon = GameObject.Find("Speer");
		Debug.Log(weapon);
	}

	//WICHTIG ANIMATOR DARF KEINE ROOT MOTION EINGESCHALTEN HABEN
	void FixedUpdate ()
	{
		Quaternion qa = new Quaternion(0,playerCamera.transform.rotation.y,0,playerCamera.transform.rotation.w);
		lastPos = controller.transform.position;
		animator.SetBool("attack",false);
		animator.SetBool("def",false);
		animator.SetBool("draw",false);
		animator.SetBool("back",false);

		if(Input.GetKey(KeyCode.W)){
			if(Input.GetMouseButton(0)){
				playAttackAnimation();
			}

			Vector3 w = playerCamera.transform.TransformDirection(Vector3.forward);
			controller.SimpleMove(w * speed);
		}else if(Input.GetKey(KeyCode.S)) {
			Vector3 s = playerCamera.transform.TransformDirection(Vector3.back);
			controller.SimpleMove(s * speed);
			animator.SetBool("back",true);
			
		}else if(Input.GetKey(KeyCode.D)) {
			Vector3 d = playerCamera.transform.TransformDirection(Vector3.right);
			controller.SimpleMove(d * speed);
			
		}else if(Input.GetKey(KeyCode.A)) {
			Vector3 a = playerCamera.transform.TransformDirection(Vector3.left);
			controller.SimpleMove(a * speed);
			
		}else if(Input.GetKey(KeyCode.F)){
			weapon.transform.parent = null;
			playDrawendAnimation();
			weapon.transform.position = hand.position;
			weapon.transform.rotation = hand.rotation;
			weapon.transform.parent = hand;
		}else if(Input.GetMouseButton(0)){
			playAttackAnimation();
		}else if(Input.GetMouseButton(1)){
			playDefendAnimation();
		}

		magni = (controller.transform.position - lastPos).magnitude/Time.deltaTime;
		animator.SetFloat("speed",magni * 100);

		//Rotates the player
		transform.rotation = qa;
	}


	public void playAttackAnimation(){
		animator.SetInteger("attackInt",(int)Random.Range(1,3));
		animator.SetBool("attack",true);
	}

	public void playDefendAnimation(){
		animator.SetBool("def",true);
	}

	public void playDrawendAnimation(){
		animator.SetBool("draw",true);
	}

}
