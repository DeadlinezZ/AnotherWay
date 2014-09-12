using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NetworkView))]
public class LevelManager : MonoBehaviour {
	

	//The MapList holding all the MapObjects
	public List<MapSettings> mapList = new List<MapSettings>();
	
	//The current selected Map
	public MapSettings curMap;
	
	//Holds the current MapIndex
	public int mapIndex = 0;
	
	//Holds the mainlevel that is preloaded at the gamestart and will be loaded when leaving a game
	public string mainlevel = "main";
	
	//Holds the last level's prefix (needs to be incremented when we change the level)
	private int lastLevelPrefix = 0;
	
	//Used to store a relation to the MenuManager script
	private Menu menu;


	//Is called before Start()
	void Awake(){
		networkView.group = 1;
		Application.LoadLevel(mainlevel);
	}
	
	// Use this for initialization
	void Start () {
		menu = GameObject.Find("GUI").GetComponent<Menu>();
	}
	
	
	[RPC]
	//Used to load the map on a client accross the network
	void Client_LoadMap(int mIndex, int levelPrefix){
		//Set the map to load
		mapIndex = mIndex;
		curMap = mapList[mIndex];
		StartCoroutine(LoadLevel(curMap.MapLoadName, levelPrefix));
	}


	private IEnumerator LoadLevel(string level, int levelPrefix){
		lastLevelPrefix = levelPrefix;
		
		// There is no reason to send any more data over the network on the default channel,
		// because we are about to load the level, thus all those objects will get deleted anyway
		Network.SetSendingEnabled(0, false);

		// We need to stop receiving because first the level must be loaded first.
		// Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
		Network.isMessageQueueRunning = false;
		
		// All network views loaded from a level will get a prefix into their NetworkViewID.
		// This will prevent old updates from clients leaking into a newly created scene.
		Network.SetLevelPrefix(lastLevelPrefix);
		Application.LoadLevel(level);
		
		//Wait for it to finish loading
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		

		// Allow receiving data again
		Network.isMessageQueueRunning = true;
		// Now the level has been loaded and we can start sending out data to clients
		Network.SetSendingEnabled(0, true);
		
		foreach(GameObject go in FindObjectsOfType(typeof(GameObject))){
			go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
		}
	}


	//Fires when the player or the server disconnects from the network
	void OnDisconnectedFromServer ()
	{
		//Load the startlevel/mainlevel
		Application.LoadLevel(mainlevel);
		
		//Switch the menu back
		menu.curMenu = Menu.MenuScreen.Multiplayer;
		
	}
}

