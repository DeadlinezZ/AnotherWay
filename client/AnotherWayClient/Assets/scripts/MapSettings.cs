using UnityEngine;
using System.Collections;

[System.Serializable]
public class MapSettings
{
	/// <summary>
	/// This Class is used to create MapSettings Objects that holds different
	/// important information about the maps.
	/// </summary>
	
	//The Map Name that is showed as map title
	public string MapName;
	
	//This name is used to hold the name of the scene fitting to the map
	public string MapLoadName;
	
	//This text is showed on the Map Loading Screen
	public string MapLoadText;
	
	//This texture is used to show a little texture in the map selection menu or game lobby
	public Texture MapPreviewTexture;
	
	//Holds the texture drawed on the loading screen
	public Texture MapLoadTexture;
	
	public MapSettings Constructor(){
		MapSettings capture = new MapSettings();
		
		capture.MapName = MapName;
		capture.MapLoadName = MapLoadName;
		capture.MapLoadText = MapLoadText;
		capture.MapPreviewTexture = MapPreviewTexture;
		capture.MapLoadTexture = MapLoadTexture;
		
		return capture;
	}
	
}
