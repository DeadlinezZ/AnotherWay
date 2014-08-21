using UnityEngine;
using System.Collections;

[System.Serializable]
public class Player
{
	//visible (needed!) in public playerlist
	public NetworkPlayer network;
	public int id;				//Player DB ID
	public string name = "";	//Playername
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