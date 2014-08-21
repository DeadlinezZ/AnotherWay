using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour {

	PlayerDatabase playerDB;

	// Use this for initialization
	void Start () {
		Debug.Log ("networkViedID: " + networkView.viewID);
		playerDB = GameObject.Find ("PlayerDatabase").GetComponent<PlayerDatabase>();
	}

	void OnPlayerConnected(NetworkPlayer network){
		//Debug.Log ("Player connected from IP " + network.ipAddress);
	}

	void OnPlayerDisconnected(NetworkPlayer network){
		Debug.Log ("Player '" + playerDB.GetPlayerName(network) + "' disconnected.");
		playerDB.networkView.RPC("All_RemovePlayerFromList", RPCMode.All, network);
	}

	[RPC] // processed on the server only
	void Server_JoinServer(NetworkPlayer network, int id, string playerName, int exp, int level, int admin, int gm){
		Debug.Log("Player '" +playerName + "' (" + network.ipAddress + ") joined the server.");
		playerDB.networkView.RPC("All_AddPlayerToList", RPCMode.All, network, id, playerName, exp, level, admin, gm);
	}



}
