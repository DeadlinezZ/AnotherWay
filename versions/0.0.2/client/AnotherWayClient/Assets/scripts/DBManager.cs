using UnityEngine;
using System.Collections;

public class DBManager : MonoBehaviour {

	public bool loggedIn = false;
	public string username = "";
	
	private static string host = "213.142.178.131";
	
	
	private static string secretKey = "YoloSwagHose";
	private string loginURL = "http://" + host + "/AnotherWay/login.php";
	private string registerURL = "http://" + host + "/AnotherWay/register.php";
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	public void Register(string username, string email, string password, string password2){
		// ##Validate here
		//Generate Hash code
		MD5 md5 = new MD5();
		string hash = md5.Md5Sum(username + email + password + password2 + secretKey);
		
		//Generate Form
		WWWForm form = new WWWForm(); 
		form.AddField( "hash", hash ); 
		form.AddField( "username", username);
		form.AddField( "email", email);
		form.AddField( "password", password);
		form.AddField( "password2", password2);
		WWW w = new WWW(registerURL, form); 
		
		StartCoroutine(WaitForRegisterRequest(w));
	}
	
	public void Login(string username, string password){
		// ##Validate here
		
		//Generate Hash code
		MD5 md5 = new MD5();
		string hash = md5.Md5Sum(username + password + secretKey);
		
		//Generate Form
		WWWForm form = new WWWForm(); 
		form.AddField( "hash", hash ); 
		form.AddField( "username", username);
		form.AddField( "password", password);
		WWW w = new WWW(loginURL, form); 
		
		StartCoroutine(WaitForLoginRequest(w));
		
	}
	
	IEnumerator WaitForLoginRequest(WWW w){
		yield return w;
		Menu menu = GameObject.Find ("GUI").GetComponent<Menu>();
		if (w.error == null) {
			if(w.text[0] == '1'){ //Login successful
				loggedIn = true;
				Debug.Log ("Successfully logged in");
				menu.loginMessage = "Sucessfully logged in";
				menu.curMenu = Menu.MenuScreen.Multiplayer;
				
			} else {
				menu.loginMessage = w.text;
			}
			w.Dispose(); 
			
		} else {
			Debug.Log ("Something went wrong when trying to login: " + w.error);
		} 
		
	}  
	
	
	IEnumerator WaitForRegisterRequest(WWW w){
		yield return w;
		Menu menu = GameObject.Find ("GUI").GetComponent<Menu>();
		if (w.error == null) {
			if(w.text == "1"){ //Register successful
				Debug.Log("Sucessfully registered");
				menu.registerMessage = "Sucessfully registered. You will be redirected automatically soon.";
				StartCoroutine(WaitForRedirection(3));
			} else {
				menu.registerMessage = w.text;
			}
			w.Dispose(); 
			
		} else {
			Debug.Log ("Something went wrong when trying to register: " + w.error);
			menu.registerMessage = "Something went wrong when trying to register: " + w.error;
		} 
		
	}
	
	IEnumerator WaitForRedirection(int seconds){
		yield return new WaitForSeconds(seconds);
		Menu menu = GameObject.Find ("GUI").GetComponent<Menu>();
		menu.lusername = menu.rusername;
		menu.lpassword = menu.rpassword;
		menu.curMenu = Menu.MenuScreen.MultiplayerLogin;
		menu.rusername = "";
		menu.remail = "";
		menu.rpassword = "";
		menu.rpassword2 = "";
	}
}
