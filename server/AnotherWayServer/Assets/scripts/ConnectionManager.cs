﻿using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour {

	PlayerDatabase playerDB;
	GameLobby gameLobby;
	LevelManager levelManager;

	// Use this for initialization
	void Start () {
		playerDB = GameObject.Find ("PlayerDatabase").GetComponent<PlayerDatabase>();
		gameLobby = GameObject.Find ("GUI").GetComponent<GameLobby>();
		levelManager = GameObject.Find ("LevelManager").GetComponent<LevelManager>();
	}

	void OnPlayerConnected(NetworkPlayer network){
		//Debug.Log ("Player connected from IP " + network.ipAddress);
		levelManager.networkView.RPC("Client_LoadMap", network, levelManager.mapIndex, levelManager.lastLevelPrefix + 1);
	}

	void OnPlayerDisconnected(NetworkPlayer network){
		Debug.Log ("Player '" + playerDB.GetPlayerName(network) + "' disconnected.");
		playerDB.networkView.RPC("All_RemovePlayerFromList", RPCMode.AllBuffered, network);
		gameLobby.networkView.RPC("All_RemovePlayerFromTeam", RPCMode.AllBuffered, network);
	}

	[RPC] // processed on the server only
	void Server_JoinServer(NetworkPlayer network, int id, string playerName, int exp, int level, int admin, int gm){
		Debug.Log("Player '" +playerName + "' (" + network.ipAddress + ") joined the server.");
		playerDB.networkView.RPC("All_AddPlayerToList", RPCMode.AllBuffered, network, id, playerName, exp, level, admin, gm);
	}

	void OnDisconnectedFromServer(){
		gameLobby.ClearTeamLists();
	}


}
