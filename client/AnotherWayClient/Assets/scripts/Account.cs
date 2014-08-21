using UnityEngine;
using System.Collections;

[System.Serializable]
public class Account
{	
	//Playerinfo
	public Player player;

	//Account Shared info
	public string email;				
	public int ap;				
	public int coins;
	
	//Player stats
	public int kills;
	public int deaths;
	public int assits;
	public int gamesPlayed;
	public int wins;
}