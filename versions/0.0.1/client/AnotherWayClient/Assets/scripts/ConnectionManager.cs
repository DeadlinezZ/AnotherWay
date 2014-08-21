using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour {
	public bool loggedIn = false;
	public string username = "";

	private static string secretKey = "YoloSwagHose";
	private static string loginURL = "http://localhost/AnotherWay/login.php";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool Login(string username, string password){
		// ##Validate here

		//Generate Hash code
		MD5 md5 = new MD5();
		string hash = md5.Md5Sum(username + password + secretKey);

		//Generate Form
		WWWForm form = new WWWForm(); 
		form.AddField( "hash", hash ); 
		form.AddField( "username", username );
		form.AddField( "password", password );
		WWW w = new WWW(loginURL, form); 

		StartCoroutine(WaitForRequest(w));

	}

	IEnumerator WaitForRequest(WWW w){
		yield return w;

		if (w.error == null) {
			Debug.Log ("Result: " + w.text); 
			if(w.text == "1"){ //Login successful
				loggedIn = true;
				yield return null;
			} 
			w.Dispose(); 
			
		} else {
			Debug.Log ("Something went wrong when trying to login: " + w.error);
		} 

	}    
}
