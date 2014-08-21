using UnityEngine;
using System.Collections;

public class Core : MonoBehaviour {

	public enum Gamemode {
		Normal,
		Coop
	}

	private static string gameKey;
	public int maxPlayers;
	public int port;
	public string serverName;
	public Gamemode gamemode;
	public bool publicServer;

	
	void Start () {
		gameKey = "AnotherWayUniqueCode00001";
		maxPlayers = 4;
		port = 25565;
		serverName = "ServerDefault";
		publicServer = true;
		gamemode = Gamemode.Normal;
	}

	public void StartServer(){

		if(publicServer){
			Network.InitializeServer(maxPlayers, port, !Network.HavePublicAddress());
			MasterServer.RegisterHost(gameKey, serverName, gamemode.ToString());
		} else {
			Network.InitializeServer(maxPlayers, port, false);
		}
		Debug.Log("Server started\n----------------------------------------\nmaxPlayer:\t" + maxPlayers.ToString() + "\nport:\t\t\t\t" + port.ToString() + "\nservername:\t" + serverName + "\ngamemode:\t" + gamemode.ToString()+ "\npublic:\t\t\t" + publicServer.ToString() + "\n----------------------------------------");
	}
		
	public void StopServer(){
		Network.Disconnect();
		if(publicServer){
			MasterServer.UnregisterHost();
		}
		Debug.Log("Server stopped");
	}




	//Runs on the client when a player has disconnected
	void OnDisconnectedFromServer(){

	}
		
	//Runs on the server when a player has disconnected
	void OnPlayerDisconnected(NetworkPlayer networkPlayer){

	}


}
