using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour {

	public static string gameKey = "AnotherWayUniqueCode00001";
	public Ping masterServerPing;

	PlayerDatabase playerDB;
	GameLobby gameLobby;

	// Use this for initialization
	void Start () {
		playerDB = GameObject.Find ("PlayerDatabase").GetComponent<PlayerDatabase>();
		gameLobby = GameObject.Find ("GUI").GetComponent<GameLobby>();

		//Request a list of online servers (that got our key)
		MasterServer.RequestHostList(gameKey);
		
		//Ping the masterserver so we get its ip and stuff (is set to 0.0.0.0 otherwise)
		masterServerPing = new Ping(MasterServer.ipAddress);
	}


	void OnConnectedToServer(){
		Player p = playerDB.account.player;
		networkView.RPC ("Server_JoinServer", RPCMode.Server, Network.player, p.id, p.name, p.exp, p.level, p.admin, p.gm);
		GameObject.Find ("GUI").GetComponent<Menu>().enabled = false;
		GameObject.Find ("GUI").GetComponent<GameLobby>().enabled = true;
	}

	void OnDisconnectedFromServer()
	{
		playerDB.PlayerDisconnected();
		GameObject.Find ("GUI").GetComponent<Menu>().enabled = true;
		gameLobby.enabled = false;
		gameLobby.ClearTeamLists();
	}

	[RPC] // processed on the server only
	void Server_JoinServer(NetworkPlayer network, int id, string playerName, int exp, int level, int admin, int gm){}


}
