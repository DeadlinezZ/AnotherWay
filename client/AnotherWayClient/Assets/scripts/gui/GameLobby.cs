using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLobby : MonoBehaviour {


	Menu menu;
	public GUIStyle teamHeaderText;
	public GUIStyle rowHeaderText;
	public GUIStyle rowText;
	public int maxPlayers;

	public List<Player> team1 = new List<Player>();
	public List<Player> team2 = new List<Player>();

	void Awake(){
		networkView.group = 1;
	}

	void OnEnable(){
		networkView.RPC("Server_RequestMaxPlayers", RPCMode.Server, Network.player);
		networkView.RPC ("Server_RequestTeamJoin", RPCMode.Server, Network.player);
		menu = gameObject.GetComponent<Menu>();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.F3)){
			int team = 0;
			if(GetPlayerTeam(Network.player) == 1){ team = 0; } else { team = 1; }

			networkView.RPC ("Server_RequestTeamSwitch", RPCMode.Server, Network.player, team);
		}
	}


	void OnGUI(){
		GUI.DrawTexture(new Rect(Screen.width / 2 - 1, 60, 2, Screen.height - 60), menu.lobbyBar);

		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

		GUILayout.Label ("Game Lobby",  menu.headerText, GUILayout.Width(Screen.width));

		GUILayout.Space (20);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Team 1", teamHeaderText, GUILayout.Width(Screen.width / 2));
		GUILayout.Label("Team 2", teamHeaderText, GUILayout.Width(Screen.width / 2));
		GUILayout.EndHorizontal();

		GUILayout.Space (20);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Player", rowHeaderText, GUILayout.Width(Screen.width / 4));
		GUILayout.Label("State", rowHeaderText, GUILayout.Width(Screen.width / 4));
		GUILayout.Label("Player", rowHeaderText, GUILayout.Width(Screen.width / 4));
		GUILayout.Label("State", rowHeaderText, GUILayout.Width(Screen.width / 4));
		GUILayout.EndHorizontal();

		GUILayout.Space (20);

		//Team 1
		GUILayout.BeginArea(new Rect(0,140, Screen.width / 2, Screen.height - 140));
		for(int i = 0; i < team1.Count; i++){
			GUILayout.BeginHorizontal();
			GUILayout.Label(team1[i].name, rowHeaderText, GUILayout.Width(Screen.width / 4));
			GUILayout.Label("State", rowHeaderText, GUILayout.Width(Screen.width / 4));
			GUILayout.Space (10);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();

		//Team 2
		GUILayout.BeginArea(new Rect(Screen.width / 2, 140, Screen.width / 2, Screen.height - 140));
		for(int i = 0; i < team2.Count; i++){
			GUILayout.BeginHorizontal();
			GUILayout.Label(team2[i].name, rowText, GUILayout.Width(Screen.width / 4));
			GUILayout.Label("State", rowText, GUILayout.Width(Screen.width / 4));
			GUILayout.Space (10);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
		GUILayout.EndArea ();
	}

	private int GetPlayerTeam(NetworkPlayer network){
		PlayerDatabase playerDB = GameObject.Find ("PlayerDatabase").GetComponent<PlayerDatabase>();
		for(int i = 0; i < playerDB.playerList.Count; i++){
			if(playerDB.playerList[i].network == network){
				return playerDB.playerList[i].team;
			}
		}
		return -1;
	}

	public void ClearTeamLists(){
		team1.Clear();
		team2.Clear();
	}

	[RPC]
	void Server_RequestTeamJoin(NetworkPlayer network){}
	[RPC]
	void Server_RequestTeamSwitch(NetworkPlayer network, int team){}

	[RPC]
	void All_SetPlayerTeam(NetworkPlayer network, int team){
		PlayerDatabase playerDB = GameObject.Find ("PlayerDatabase").GetComponent<PlayerDatabase>();
		for(int i = 0; i < playerDB.playerList.Count; i++){
			if(playerDB.playerList[i].network == network){
				//Remove player from playerlist if he is there
				if(team1.Contains(playerDB.playerList[i])) team1.Remove(playerDB.playerList[i]);
				if(team2.Contains(playerDB.playerList[i])) team2.Remove(playerDB.playerList[i]);

				//Add player to new playerlist
				if(team == 0){ team1.Add(playerDB.playerList[i]); } else { team2.Add(playerDB.playerList[i]); }
				playerDB.playerList[i].team = team;
			}
		}
	}

	[RPC]
	void All_RemovePlayerFromTeam(NetworkPlayer network){
		//Remove player from playerlist if he is there
		for(int i = 0; i < team1.Count; i++){
			if(team1[i].network == network) {team1.RemoveAt (i);}
		}
		for(int i = 0; i < team2.Count; i++){
			if(team2[i].network == network) {team2.RemoveAt (i);}
		}
	}



	[RPC]
	void Server_RequestMaxPlayers(NetworkPlayer network){}

	[RPC]
	void Client_SetMaxPlayers(int maxPlayers){
		this.maxPlayers = maxPlayers;
	}
}
