﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDatabase : MonoBehaviour {

	public Account account;

	public List<Player> playerList = new List<Player>();

	// Use this for initialization
	void Start () {
		account = new Account();
	}

	public void PlayerDisconnected(){
		playerList.Clear();
	}

	[RPC] //Executed on all clients and the server
	public void All_AddPlayerToList(NetworkPlayer network, int id, string name, int exp, int level, int admin, int gm){
			Player player = new Player();
			player.network = network;
			player.id = id;
			player.name = name;
			player.exp = exp;
			player.level = level;
			player.admin = admin;
			player.gm = gm;
			playerList.Add (player);
	}

	[RPC] //Executed on client and Server once a player left
	void All_RemovePlayerFromList(NetworkPlayer network){
		for(int i = 0; i < playerList.Count; i++){
			if(network == playerList[i].network){
				playerList.Remove(playerList[i]);
			}
		}
	}
}
