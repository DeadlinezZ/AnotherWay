using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLobby : MonoBehaviour {

	public List<Player> team1 = new List<Player>();
	public List<Player> team2 = new List<Player>();

	void Start(){
	}

	[RPC]
	void Server_RequestTeamJoin(NetworkPlayer network){
		int team = 0;
		if(team1.Count > team2.Count){
			team = 1;
		} 
		networkView.RPC ("All_SetPlayerTeam", RPCMode.AllBuffered, network, team);
	}

	[RPC]
	void Server_RequestTeamSwitch(NetworkPlayer network, int team){
		switch(team){
		case 0:
			if(team1.Count < team2.Count) Server_TeamSwitch(network, team);
			break;
		case 1:
			if(team2.Count < team1.Count) Server_TeamSwitch(network, team);
			break;
		}

	}

	void Server_TeamSwitch(NetworkPlayer network, int team){
		networkView.RPC ("All_SetPlayerTeam", RPCMode.AllBuffered, network, team);
	}

	public void ClearTeamLists(){
		team1.Clear();
		team2.Clear();
	}

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

				Debug.Log ("Player '" + playerDB.playerList[i].name + "' joined Team " + team);
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
	void Server_RequestMaxPlayers(NetworkPlayer network){
		networkView.RPC ("Client_SetMaxPlayers", network, GameObject.Find ("Core").GetComponent<Core>().maxPlayers);
	}

	[RPC]
	void Client_SetMaxPlayers(int maxPlayers){}
}
