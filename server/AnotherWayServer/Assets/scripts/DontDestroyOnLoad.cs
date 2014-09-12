using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
	
	void Awake(){
		DontDestroyOnLoad(this);
	}
	
	// Use this for initialization
	void Start () {
		
	}
}
