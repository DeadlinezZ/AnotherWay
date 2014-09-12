using UnityEngine;
using System.Collections;

public class ServerGUI : MonoBehaviour {

	public enum Menu {
		general,
		gamemode,
		players,
		start,
		overview,
		admin,
		console
	}

	public Menu menu;

	private int windowWidth;
	private int windowHeight;

	private float middleSpacer;
	private	float menuTopIndent;
	private float menuWidth;
	private float menuHeight;

	public GUISkin defaultSkin;

	private float settingsTopIndent;
	private float settingsLeftIndent;
	private float settingsWidth;
	private float settingsHeight;

	private float buttonWidth;
	private float buttonHeight;

	private float labelWidth;
	private float labelHeight;

	private float textfieldIntWidth;

	public GUIStyle headerText;
	private GUIStyle buttonText;
	private GUIStyle buttonTextNormal;
	public GUIStyle labelText;
	public GUIStyle labelTextBlack;
	private GUIStyle guiElementStyle;
	private GUIStyle dropDownStyle;
	private Core core;

	private bool serverStarted = false;

	//Gamemode Selection dropdown
	private Vector2 scrollViewVector = Vector2.zero;
	private Rect gamemodeSelectionRect;
	public string[] gamemodeSelectionList;
	private int gamemodeSelectionIndexNumber;
	private bool showGamemodeSelection = false;


	public Texture2D backgroundTexMenu;
	public Texture2D backgroundTexSettings;
	public Texture2D backgroundTexMiddleBar;
	public Texture2D backgroundBackground;
	public Texture2D lobbyBar;


	// Use this for initialization
	void Start () {
		windowWidth = Screen.width;
		windowHeight = Screen.height;

		menuTopIndent = 0;
		menuHeight = windowHeight;
		menuWidth = windowWidth / 3;

		middleSpacer = 1;
		settingsTopIndent = 0;
		settingsLeftIndent = windowWidth / 3 + middleSpacer;
		settingsHeight = windowHeight;
		settingsWidth = windowWidth / 3 * 2 - middleSpacer;

		buttonWidth = menuWidth;
		buttonHeight = 40;

		labelWidth = 100;
		labelHeight = 30;

		textfieldIntWidth = 60f;

		core = GameObject.Find ("Core").GetComponent<Core>();

		//Button Style
		buttonText = new GUIStyle();
		buttonText.normal.textColor = Color.white;
		//buttonText.hover.textColor = Color.red;
		buttonText.alignment = TextAnchor.MiddleCenter;
		buttonText.border.left = 0;
		buttonText.border.bottom = 0;
		buttonText.border.right = 0;
		buttonText.border.top = 0;
		buttonText.active.textColor = Color.white;
		buttonText.fontSize = 20;
		buttonText.normal.background = null;
		buttonText.hover.background = new Texture2D(10,10);
		buttonText.active.background = new Texture2D(10,10);
		buttonText.font = defaultSkin.font;

		buttonTextNormal = new GUIStyle();
		buttonTextNormal.normal.textColor = Color.black;
		//buttonTextNormal.hover.textColor = Color.red;
		buttonTextNormal.alignment = TextAnchor.MiddleCenter;
		buttonTextNormal.border.left = 0;
		buttonTextNormal.border.bottom = 0;
		buttonTextNormal.border.right = 0;
		buttonTextNormal.border.top = 0;
		buttonTextNormal.fontSize = 20;
		buttonTextNormal.normal.background = new Texture2D(10,10);
		buttonTextNormal.hover.background = lobbyBar;
		buttonTextNormal.font = defaultSkin.font;


		//GUI element style
		guiElementStyle = new GUIStyle();
		guiElementStyle.font = defaultSkin.font;
		guiElementStyle.normal.background = new Texture2D(10,10);
		guiElementStyle.padding = new RectOffset(5,2,2,2);
		guiElementStyle.normal.textColor = Color.black;
		guiElementStyle.active.textColor = Color.black;


		//Drop down style
		dropDownStyle = new GUIStyle();
		dropDownStyle.font = defaultSkin.font;
		dropDownStyle.normal.background = new Texture2D(10,10);
		dropDownStyle.padding = new RectOffset(5,2,2,2);
		dropDownStyle.normal.textColor = Color.black;
		dropDownStyle.active.textColor = Color.black;
		dropDownStyle.hover.background = lobbyBar;


	}
	

	void OnGUI(){

		//Background Boxes
		GUI.DrawTexture(new Rect(0, menuTopIndent, menuWidth, menuHeight), backgroundTexMenu); // menu box
		GUI.DrawTexture(new Rect(settingsLeftIndent, settingsTopIndent, settingsWidth, settingsHeight), backgroundTexSettings); // settings box
		GUI.DrawTexture(new Rect(settingsLeftIndent - 100, settingsTopIndent, settingsWidth, settingsHeight), backgroundBackground); // background image
		GUI.DrawTexture(new Rect(settingsLeftIndent - middleSpacer, 0, middleSpacer, settingsHeight), lobbyBar); //middle bar
		GUI.Box (new Rect(0,0, Screen.width, Screen.height), ""); //dark overlay

		//Buttons on the left
		if(!serverStarted){
			if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "General", buttonText)){
				menu = Menu.general;
			}
			if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Gamemode", buttonText)){
				menu = Menu.gamemode;
			}
			if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Players", buttonText)){
				menu = Menu.players;
			}
			if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 3 + 3 * 10, buttonWidth, buttonHeight), "Start", buttonText)){
				menu = Menu.start;
			}
			if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Exit", buttonText)){
				Application.Quit();
			}
		} else {
			if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Overview", buttonText)){
				menu = Menu.overview;
				GameObject.Find ("Console").GetComponent<Console>().show = false;
			}
			if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Admin", buttonText)){
				menu = Menu.admin;
				GameObject.Find ("Console").GetComponent<Console>().show = false;
			}
			if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Console", buttonText)){
				menu = Menu.console;
				GameObject.Find ("Console").GetComponent<Console>().show = true;
			}
		}

		//Content
		if(menu == Menu.general){
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth / 2, settingsTopIndent + 100, labelWidth, labelHeight), "General Settings", headerText);

			//Servername
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 200, 200, 20), "Servername:", labelText);
			core.serverName = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 225, 200, 20), core.serverName, guiElementStyle);

			//Port
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 255, 200, 20), "Port:", labelText);
			int.TryParse(GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 280, textfieldIntWidth, 20), core.port.ToString(), guiElementStyle), out core.port);

			//Public/Lan
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 310, 200, 20), "Server Visibility", labelText);
			core.publicServer = GUI.Toggle(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 330, 280, 20), core.publicServer, " Public (off = lan)");
		}

		else if(menu == Menu.gamemode){
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth / 2, settingsTopIndent + 100, labelWidth, labelHeight), "Gamemode", headerText);

			//Gamemode Selection Dropdown
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth / 2 + 40 - labelWidth, settingsTopIndent + 200, 200, 25), "Choose gamemode:", labelText);
			gamemodeSelectionRect = new Rect(settingsLeftIndent + settingsWidth / 2  + 40, settingsTopIndent + 225,125, gamemodeSelectionList.Length * 25);

			if(GUI.Button(new Rect((gamemodeSelectionRect.x - 100), gamemodeSelectionRect.y, 125, 25), "", dropDownStyle)){
				if(!showGamemodeSelection)
				{
					showGamemodeSelection = true;
				}
				else
				{
					showGamemodeSelection = false;
				}
			}
			
			if(showGamemodeSelection){
				scrollViewVector = GUI.BeginScrollView(new Rect((gamemodeSelectionRect.x - 100), (gamemodeSelectionRect.y + 25), gamemodeSelectionRect.width, gamemodeSelectionRect.height),scrollViewVector,new Rect(0, 0, gamemodeSelectionRect.width, Mathf.Max(gamemodeSelectionRect.height, (gamemodeSelectionList.Length*25))));
				
				GUI.Box(new Rect(0, 0, gamemodeSelectionRect.width, Mathf.Max(gamemodeSelectionRect.height, (gamemodeSelectionList.Length*25))), "");
				
				for(int index = 0; index < gamemodeSelectionList.Length; index++)
				{
					
					if(GUI.Button(new Rect(0, (index*25), gamemodeSelectionRect.width, 25), "", dropDownStyle))
					{
						showGamemodeSelection = false;
						gamemodeSelectionIndexNumber = index;
						if(gamemodeSelectionIndexNumber == 0){
							core.gamemode = Core.Gamemode.Normal;
						} else if(gamemodeSelectionIndexNumber == 1){
							core.gamemode = Core.Gamemode.Coop;
						}

					}
					
					GUI.Label(new Rect(5, (index*25), gamemodeSelectionRect.width, 25), gamemodeSelectionList[index], labelTextBlack);
					
				}
				
				GUI.EndScrollView();   
			} else {
				GUI.Label(new Rect((gamemodeSelectionRect.x - 95), gamemodeSelectionRect.y, 300, 25), gamemodeSelectionList[gamemodeSelectionIndexNumber], labelTextBlack);
			}

		}

		else if(menu == Menu.players){
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth / 2, settingsTopIndent + 100, labelWidth, labelHeight), "Player Settings", headerText);

			//Max Player on whole Server
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 200, 200, 20), "Max Allowed Player On Server:", labelText);
			int.TryParse(GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 225, textfieldIntWidth, 20), core.maxPlayers.ToString(), guiElementStyle), out core.maxPlayers);
		}

		else if(menu == Menu.start){
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth / 2, settingsTopIndent + 100, labelWidth, labelHeight), "Start Server", headerText);

			//Servername
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 200, 200, 20), "Servername", labelText);
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 215, 200, 20), core.serverName);

			//Port
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 245, 200, 20), "Port", labelText);
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 260, 200, 20), core.port.ToString());

			//Gamemode
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 290, 200, 20), "Gamemode", labelText);
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 305, 200, 20), core.gamemode.ToString());

			//Max Allowed Player On Server
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 335, 200, 20), "Max. Allowed Player On Server", labelText);
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 350, 200, 20), core.maxPlayers.ToString());

			//servername
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 380, 200, 20), "Server visibility", labelText);
			string visibility;
			if(core.publicServer){
				visibility = "Public";
			} else {
				visibility = "Lan";
			}
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 395, 200, 20), visibility);

			//Startbutton
			if(GUI.Button(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth - 1, settingsTopIndent + 435, 200, 40), "Start Server", buttonTextNormal)){
				core.StartServer();
				serverStarted = true;
				menu = Menu.overview;
			}
		}

		else if(menu == Menu.overview){
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth / 2, settingsTopIndent + 100, labelWidth, labelHeight), "Overview", headerText);

			//servername
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 200, 200, 20), "Servername", labelText);
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 215, 200, 20), core.serverName);

			//num of connections
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 245, 200, 20), "Number of connections", labelText);
			if(Network.connections.Length >= 1){
				GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 260, 200, 20), Network.connections.Length.ToString() + " / " + core.maxPlayers.ToString());
			} else {
				GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 260, 200, 20), "No players connected. (Max. " + core.maxPlayers + ")");
			}

			//average ping
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 290, 200, 20), "Average Ping", labelText);
			if(Network.connections.Length >= 1){
				GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 305, 200, 20), Network.GetAveragePing(Network.connections[0]).ToString());
			} else {
				GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 305, 200, 20), "No players connected.");
			}

			//servername
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 335, 200, 20), "Server visibility", labelText);
			string visibility;
			if(core.publicServer){
				visibility = "Public";
			} else {
				visibility = "Lan";
			}
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 350, 200, 20), visibility);

			//Stopbutton
			if(GUI.Button(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth - 1, settingsTopIndent + 390, 200, 40), "Stop Server", buttonTextNormal)){
				core.StopServer();
				serverStarted = false;
				menu = Menu.start;
			}
		}
		else if(menu == Menu.admin){
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth / 2, settingsTopIndent + 100, labelWidth, labelHeight), "Adminpanel", headerText);
		}
		else if(menu == Menu.console){
			GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth / 2, settingsTopIndent + 100, labelWidth, labelHeight), "Console", headerText);

			//The console itself is handled on the console-gameobject, which is a child of the GUI gameobject
		}
	}
}
