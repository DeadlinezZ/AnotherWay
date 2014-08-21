using UnityEngine;
using System.Collections;

[System.Serializable]
public class Player
{
	public NetworkPlayer network;
	public int id;				//Player DB ID
	public string name = "";	//Playername
	
	//These values are only for client information, everything is processed on the server  
	//not visible in public playerlist
	public string email;				
	public int ap;				
	public int coins;
	//visible in public playerlist
	public int exp;		
	public int level;	
	public int admin;	
	public int gm;		

	//Ingame values
	public int health = 100;
	public bool isAlive;
	public int kills;
	public int deaths;
}