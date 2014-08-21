using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Menu : MonoBehaviour {

	public enum MenuScreen {
		None,
		Main,
		Multiplayer,
		MultiplayerLogin,
		MultiplayerRegister,
		MultiplayerLobby,
		MultiplayerDirect,
		MultiplayerGameLobby,
		Singleplayer,
		SingleplayerLoadGame,
		SingleplayerNewGame,
		SettingsKeys,
		SettingsAudio,
		SettingsGraphics,
		Credits
	}

	public MenuScreen curMenu;

	//Used to store the current Lobby-Server-Searching-State
	public enum ServerSearch {
		Searching,
		NoServerFound,
		ServerFound
	}

	private enum Filter {
		Servername_Asc,
		Servername_Desc,
		Players_Asc,
		Players_Desc,
		Ping_Asc,
		Ping_Desc,
		Gamemode_All,
		Gamemode_Normal,
		Gamemode_Coop
	}
	private Filter filter;

	//Lobby Filter
	//bool showGamemodeFilterDropdown = false;
	//private Vector2 scrollViewVector = Vector2.zero;
	private Rect gamemodeFilterRect;
	public string[] gamemodeFilterList;
	private int gamemodeFilterIndexNumber;

	//used to show a info whether the client is searching or not
	public ServerSearch serverSearch;
	
	private DBManager DBManager;
	private ConnectionManager ConnectionManager;

	public GUISkin defaultSkin;

	private int windowWidth;
	private int windowHeight;

	private float middleSpacer;
	private	float menuTopIndent;
	private float menuWidth;
	private float menuHeight;
	
	private float settingsTopIndent;
	private float settingsLeftIndent;
	private float settingsWidth;
	private float settingsHeight;
	
	private float buttonWidth;
	private float buttonHeight;
	
	private float labelWidth;
	private float labelHeight;

	private float headerLeftIndent;
	private float headerTopIndent;

	[HideInInspector]
	public string registerMessage = "";
	[HideInInspector]
	public string loginMessage = "";

	private float textfieldIntWidth;
	
	public GUIStyle headerText;
	public GUIStyle labelText;
	public GUIStyle	registerAndLoginMessage;
	private GUIStyle buttonText;

	public Texture2D backgroundTexMenu;
	public Texture2D backgroundTexSettings;
	public Texture2D backgroundTexMiddleBar;
	public Texture2D backgroundBackground;
	public Texture2D lobbyBar;

	[HideInInspector]
	public string lusername = "";
	[HideInInspector]
	public string lpassword = "";
	[HideInInspector]
	public string rusername = "";
	[HideInInspector]
	public string rpassword = "";
	[HideInInspector]
	public string rpassword2 = "";
	[HideInInspector]
	public string remail = "";


	//Multiplayer Lobby Variables
	private HostData[] hostData;
	private Vector2 scrollPosition = Vector2.zero;
	private int lobbyRowHeight = 25;
	private int lobbyRowWidth = 100;
	private GUIStyle lobbyHeaderStyle;
	public GUIStyle lobbyRowStyle = new GUIStyle();
	public GUIStyle lobbyFilterStyle = new GUIStyle();
	private GUIStyle lobbyButton;
	private int lobbyFontSize = 13;
	private GUIStyle lobbyBoxStyle;
	private GUIStyle guiElementStyle;
	public List<Server> serverList = new List<Server>();

	private bool enterPressed = false;

	// Use this for initialization
	void Start () {

		curMenu = MenuScreen.Main;
		serverSearch = ServerSearch.Searching;
		filter = Filter.Gamemode_All;

		DBManager = GameObject.Find ("DBManager").GetComponent<DBManager>();
		ConnectionManager = GameObject.Find ("ConnectionManager").GetComponent<ConnectionManager>();

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

		//textfieldIntWidth = 60f;

		headerLeftIndent = settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth / 2;
		headerTopIndent = settingsTopIndent + 100;

		//Lobby Row style


		//Button Style
		buttonText = new GUIStyle();
		buttonText.fontSize = 20;
		buttonText.normal.textColor = Color.white;
		buttonText.normal.background = null;
		buttonText.alignment = TextAnchor.MiddleCenter;
		buttonText.border.left = 0;
		buttonText.border.bottom = 0;
		buttonText.border.right = 0;
		buttonText.border.top = 0;
		buttonText.hover.background = new Texture2D(10,10);
		buttonText.active.textColor = Color.white;
		buttonText.active.background = new Texture2D(10,10);
		buttonText.font = defaultSkin.font;

		//Lobby Button Style
		lobbyButton = new GUIStyle();
		lobbyButton.fontSize = lobbyFontSize;
		lobbyButton.normal.textColor = Color.black;
		lobbyButton.normal.background = new Texture2D(10,10);
		lobbyButton.alignment = TextAnchor.MiddleCenter;
		lobbyButton.border.left = 0;
		lobbyButton.border.bottom = 0;
		lobbyButton.border.right = 0;
		lobbyButton.border.top = 0;
		lobbyButton.hover.background = lobbyBar;
		lobbyButton.hover.textColor = Color.black;
		lobbyButton.active.textColor = Color.black;
		lobbyButton.active.background = new Texture2D(10,10);
		lobbyButton.font = defaultSkin.font;

		//GUI element style
		guiElementStyle = new GUIStyle();
		guiElementStyle.font = defaultSkin.font;
		guiElementStyle.normal.background = new Texture2D(10,10);
		guiElementStyle.padding = new RectOffset(5,2,2,2);

		//Lobby Box Style
		lobbyBoxStyle = new GUIStyle();
		lobbyBoxStyle.normal.background = lobbyBar;

		//Lobby Header Style
		lobbyHeaderStyle = new GUIStyle();
		lobbyHeaderStyle.fontSize = lobbyFontSize;
		lobbyHeaderStyle.normal.textColor = Color.white;
		lobbyHeaderStyle.normal.background = null;
		lobbyHeaderStyle.alignment = TextAnchor.MiddleCenter;
		lobbyHeaderStyle.border.left = 0;
		lobbyHeaderStyle.border.bottom = 0;
		lobbyHeaderStyle.border.right = 0;
		lobbyHeaderStyle.border.top = 0;
		lobbyHeaderStyle.hover.background = new Texture2D(10,10);
		lobbyHeaderStyle.hover.textColor = Color.black;
		lobbyHeaderStyle.active.textColor = Color.black;
		lobbyHeaderStyle.active.background = lobbyBar;
		lobbyHeaderStyle.font = defaultSkin.font;


	}
	
	// Update is called once per frame
	void Update () {
		if(curMenu == MenuScreen.Multiplayer && !DBManager.loggedIn){
			curMenu = MenuScreen.MultiplayerLogin;
		} 
	}

	void OnGUI(){


		if (Event.current.keyCode == KeyCode.Return){
			enterPressed = true;
		} else {
			enterPressed = false;
		}


		//GUI.depth = 0;
		//Background Boxes
		GUI.DrawTexture(new Rect(0, menuTopIndent, menuWidth, menuHeight), backgroundTexMenu); // menu box
		GUI.DrawTexture(new Rect(settingsLeftIndent, settingsTopIndent, settingsWidth, settingsHeight), backgroundTexSettings); // settings box
		GUI.DrawTexture(new Rect(settingsLeftIndent - 100, settingsTopIndent, settingsWidth, settingsHeight), backgroundBackground); // background image
		GUI.DrawTexture(new Rect(settingsLeftIndent - middleSpacer, 0, middleSpacer, settingsHeight), lobbyBar); //middle bar
		GUI.Box (new Rect(0,0, Screen.width, Screen.height), ""); //dark overlay


		switch(curMenu){
			case MenuScreen.Main:
				MainMenu();
				break;

			case MenuScreen.Multiplayer:
				Multiplayer();
				break;

			case MenuScreen.MultiplayerLogin:
				MultiplayerLogin();
				break;

			case MenuScreen.MultiplayerRegister:
				MultiplayerRegister();
				break;
			
			case MenuScreen.MultiplayerLobby:
				MultiplayerLobby();
				break;

			case MenuScreen.MultiplayerDirect:
				MultiplayerDirect();
				break;

			case MenuScreen.Singleplayer:
				Singleplayer();
				break;

			case MenuScreen.SingleplayerLoadGame:
				SingleplayerLoadGame();
				break;

			case MenuScreen.SingleplayerNewGame:
				SingleplayerNewGame();
				break;
			
			case MenuScreen.SettingsAudio:
				SettingsAudio();
				break;

			case MenuScreen.SettingsGraphics:
				SettingsGraphics();
				break;

			case MenuScreen.SettingsKeys:
				SettingsKeys();
				break;

			case MenuScreen.Credits:
				Credits();
				break;
		}
	}
	private void MainMenu(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Main Menu", headerText);
		MenuButtons(new string[]{ "Multiplayer", "Singleplayer", "Settings", "Credits"}, 
		new MenuScreen[]{MenuScreen.Multiplayer, MenuScreen.Singleplayer, MenuScreen.SettingsAudio, MenuScreen.Credits});
		ExitButton();
	}
	private void Multiplayer(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Overview", headerText);
		if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Overview", buttonText)){
			curMenu = MenuScreen.Multiplayer;
		}
		if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Lobby", buttonText)){
			curMenu = MenuScreen.MultiplayerLobby;
			RefreshServerList();
		}
		if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Direct Join", buttonText)){
			curMenu = MenuScreen.MultiplayerDirect;
		}
		BackButton(MenuScreen.Main);
	}
	private void MultiplayerLogin(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Login", headerText);
		
		//Username
		GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 200, 200, 20), "Username", labelText);
		GUI.SetNextControlName("Username");
		lusername = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 220, 200, 20), lusername, guiElementStyle);
		
		//Password
		GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 250, 200, 20), "Password", labelText);
		GUI.SetNextControlName("Password");
		lpassword = GUI.PasswordField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 270, 200, 20), lpassword, "*"[0], 30, guiElementStyle);                
		
		if(GUI.Button (new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 310, 200, 35),"Login", lobbyButton) || enterPressed){
			loginMessage = "Logging in...";	
			DBManager.Login(lusername, lpassword);
		}
		
		//Login Message
		GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 350, 200, 20), loginMessage, registerAndLoginMessage);
		
		
		if (GUI.GetNameOfFocusedControl() == string.Empty) {
			GUI.FocusControl("Username");
		}
		
		MenuButtons(new string[]{ "Login", "Register"}, 
		new MenuScreen[]{MenuScreen.MultiplayerLogin, MenuScreen.MultiplayerRegister});
		BackButton(MenuScreen.Main);
	}
	private void MultiplayerRegister(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Register", headerText);
		
		
		//Username
		GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 200, 200, 20), "Username", labelText);
		GUI.SetNextControlName("Username");
		rusername = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 220, 200, 20), rusername, guiElementStyle);
		
		//Email
		GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 250, 200, 20), "E-Mail", labelText);
		GUI.SetNextControlName("Email");
		remail = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 270, 200, 20), remail, guiElementStyle);
		
		//Password 1
		GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 300, 200, 20), "Password", labelText);
		GUI.SetNextControlName("Password1");		
		rpassword = GUI.PasswordField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 320, 200, 20), rpassword, "*"[0], 30, guiElementStyle);
		
		//Password 2
		GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 350, 200, 20), "Repeat Password", labelText);
		GUI.SetNextControlName("Password2");		
		rpassword2 = GUI.PasswordField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 370, 200, 20), rpassword2, "*"[0], 30, guiElementStyle);
		
		if(GUI.Button (new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 410, 200, 35),"Register", lobbyButton) || enterPressed){
			registerMessage = "Registering...";
			DBManager.Register(rusername, remail, rpassword, rpassword2);
		}
		//Register Message
		GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 450, 200, 20), registerMessage, registerAndLoginMessage);
		
		if (GUI.GetNameOfFocusedControl() == string.Empty) {
			GUI.FocusControl("Username");
		}
		
		MenuButtons(new string[]{ "Login", "Register"}, 
		new MenuScreen[]{MenuScreen.MultiplayerLogin, MenuScreen.MultiplayerRegister});
		BackButton(MenuScreen.Main);
	}
	private void MultiplayerDirect(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Direct Join", headerText);
		if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Overview", buttonText)){
			curMenu = MenuScreen.Multiplayer;
		}
		if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Lobby", buttonText)){
			curMenu = MenuScreen.MultiplayerLobby;
			RefreshServerList();
		}
		if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Direct Join", buttonText)){
			curMenu = MenuScreen.MultiplayerDirect;
		}
		BackButton(MenuScreen.Main);
	}

	private void Singleplayer(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Singleplayer", headerText);
		MenuButtons(new string[]{ "New Game", "Load Game"}, 
		new MenuScreen[]{MenuScreen.SingleplayerNewGame, MenuScreen.SingleplayerLoadGame});
		BackButton(MenuScreen.Main);
	}
	private void SingleplayerLoadGame(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Load Singleplayer Game", headerText);				
		MenuButtons(new string[]{ "New Game", "Load Game"}, 
		new MenuScreen[]{MenuScreen.SingleplayerNewGame, MenuScreen.SingleplayerLoadGame});				
		BackButton(MenuScreen.Main);	
	}
	private void SingleplayerNewGame(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "New Singleplayer Game", headerText);				
		MenuButtons(new string[]{ "New Game", "Load Game"}, 
		new MenuScreen[]{MenuScreen.SingleplayerNewGame, MenuScreen.SingleplayerLoadGame});					
		BackButton(MenuScreen.Main);
	}
	private void SettingsAudio(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Audio Settings", headerText);
		MenuButtons(new string[]{ "Audio", "Graphics", "Keys & Mouse"}, 
		new MenuScreen[]{MenuScreen.SettingsAudio, MenuScreen.SettingsGraphics, MenuScreen.SettingsKeys});					
		BackButton(MenuScreen.Main);
	}
	private void SettingsGraphics(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Graphics Settings", headerText);
		MenuButtons(new string[]{ "Audio", "Graphics", "Keys & Mouse"}, 
		new MenuScreen[]{MenuScreen.SettingsAudio, MenuScreen.SettingsGraphics, MenuScreen.SettingsKeys});					
		BackButton(MenuScreen.Main);
	}
	private void SettingsKeys(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Keys & Mouse Settings", headerText);
		MenuButtons(new string[]{ "Audio", "Graphics", "Keys & Mouse"}, 
		new MenuScreen[]{MenuScreen.SettingsAudio, MenuScreen.SettingsGraphics, MenuScreen.SettingsKeys});					
		BackButton(MenuScreen.Main);
	}
	private void Credits(){
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Credits", headerText);
		BackButton(MenuScreen.Main);
	}
	private void MultiplayerLobby(){
		//Header and Left side buttons
		GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Lobby", headerText);
		if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Overview", buttonText)){
			curMenu = MenuScreen.Multiplayer;
		}
		if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Lobby", buttonText)){
			curMenu = MenuScreen.MultiplayerLobby;
			RefreshServerList();
		}
		if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Direct Join", buttonText)){
			curMenu = MenuScreen.MultiplayerDirect;
		}
		BackButton(MenuScreen.Main);
	

		//Serverlist
		GUILayout.BeginArea(new Rect(settingsLeftIndent + 50, headerTopIndent + 50, settingsWidth - 100, settingsHeight - headerTopIndent - 100), "");

		GUILayout.Box("", lobbyBoxStyle, GUILayout.Height(2));
		GUILayout.Space (15);

		//If at least one server was found
		if(serverSearch == ServerSearch.ServerFound){
			//Headers
			GUILayout.BeginHorizontal();

			if(GUILayout.Button ("Server", lobbyHeaderStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth))){
				if(filter == Filter.Servername_Asc){
					filter = Filter.Servername_Desc;
				} else if (filter == Filter.Servername_Desc){
					filter = Filter.Servername_Asc;
				} else {
					filter = Filter.Servername_Asc;
				}
				RefreshFilter();
			}

			GUILayout.Space(10);

			if(GUILayout.Button ("Players", lobbyHeaderStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth))){
				if(filter == Filter.Players_Asc){
					filter = Filter.Players_Desc;
				} else if (filter == Filter.Players_Desc){
					filter = Filter.Players_Asc;
				} else {
					filter = Filter.Players_Asc;
				}
				RefreshFilter();
			}

			GUILayout.Space(10);

			if(GUILayout.Button ("Ping", lobbyHeaderStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth))){
				if(filter == Filter.Ping_Asc){
					filter = Filter.Ping_Desc;
				} else if (filter == Filter.Ping_Desc){
					filter = Filter.Ping_Asc;
				} else {
					filter = Filter.Ping_Asc;
				}
				RefreshFilter();
			}

			GUILayout.Space(10);

			if(GUILayout.Button ("Gamemode", lobbyHeaderStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth))){
				if(filter == Filter.Gamemode_All){
					filter = Filter.Gamemode_Normal;
				} else if (filter == Filter.Gamemode_Normal){
					filter = Filter.Gamemode_Coop;
				} else {
					filter = Filter.Gamemode_All;
				}
				RefreshFilter();
			}			
			
			GUILayout.Space(10);

			GUILayout.Label ("", lobbyHeaderStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));

			GUILayout.EndHorizontal();

			//Hosts
			scrollPosition = GUILayout.BeginScrollView(scrollPosition,false,false);
			
			//Go through the hostlist and set them on the list
			for(int i = 0; i < serverList.Count; i++){
				if(filter == Filter.Gamemode_Normal && serverList[i].hostData.comment != "Normal") continue;
				if(filter == Filter.Gamemode_Coop && serverList[i].hostData.comment != "Coop") continue;

					GUILayout.BeginHorizontal();
					
					//ServerName
					GUILayout.Label(serverList[i].hostData.gameName, lobbyRowStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));
					GUILayout.Space(10);

					//Connected Players
					GUILayout.Label ((serverList[i].hostData.connectedPlayers -1) + "/" + (serverList[i].hostData.playerLimit - 1), lobbyRowStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));
					GUILayout.Space(10);

					GUILayout.Label(serverList[i].ping ,lobbyRowStyle, GUILayout.Height(lobbyRowHeight), GUILayout.Width (lobbyRowWidth));
					GUILayout.Space(10);

					//Gamemode
					GUILayout.Label(serverList[i].hostData.comment, lobbyRowStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));

					GUILayout.Space(10);

					//A Button for each host to connect
					if(GUILayout.Button("Connect", lobbyButton, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth))){		
						//Connect to the server
						Network.Connect(serverList[i].hostData);
					}
					
					GUILayout.EndHorizontal();
					GUILayout.Space(10);
			}
			
			GUILayout.EndScrollView();
			scrollPosition = Vector2.zero;
		}
		
		else if(serverSearch == ServerSearch.NoServerFound){
			scrollPosition = GUILayout.BeginScrollView(scrollPosition,false,false);
			TextAnchor defLobbyRowStyle = lobbyRowStyle.alignment;
			lobbyRowStyle.alignment = TextAnchor.MiddleLeft;
			GUILayout.Label("No servers found..", lobbyRowStyle);
			lobbyRowStyle.alignment = defLobbyRowStyle;
			GUILayout.EndScrollView();
			scrollPosition = Vector2.zero;

		}
		
		//If there was no server found yet
		else if(serverSearch == ServerSearch.Searching){
			scrollPosition = GUILayout.BeginScrollView(scrollPosition,false,false);
			TextAnchor defLobbyRowStyle = lobbyRowStyle.alignment;
			lobbyRowStyle.alignment = TextAnchor.MiddleLeft;
			GUILayout.Label("Searching for servers..", lobbyRowStyle);
			lobbyRowStyle.alignment = defLobbyRowStyle;
			GUILayout.EndScrollView();
			scrollPosition = Vector2.zero;
		}

		GUILayout.Space(15);
		GUILayout.Box ("", lobbyBoxStyle, GUILayout.Height (2));
		GUILayout.Space (10);

		GUILayout.BeginHorizontal();
		//Refresh Button
		if(GUILayout.Button("Refresh", lobbyButton, GUILayout.Height(lobbyRowHeight), GUILayout.Width (lobbyRowWidth))){
			RefreshServerList();
		}

		GUILayout.Space (100);
		//Show Filter message
		GUILayout.Label("Current Filter: " + filter.ToString(), lobbyRowStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

		//Gamemode Filter
		//FilterByGamemode();
	}

	IEnumerator TalkToMasterServer(){

		//Reset Search state
		serverSearch = ServerSearch.Searching;

		//Clear Hostdata list
		hostData = new HostData[0];
		
		//Clear Server Host List
		MasterServer.ClearHostList();
		//Get all active Servers
		MasterServer.RequestHostList(ConnectionManager.gameKey);
		
		
		//Wait a bit until the host list is complete
		yield return new WaitForSeconds(ConnectionManager.masterServerPing.time/100 + 0.1f);		
		
		//Put them in an array
		hostData = MasterServer.PollHostList();

		//Create a new filtered serverlist
		CreateServerList();

		//Set the filter
		RefreshFilter();

		//Fill the list with all server pings
		if(hostData.Length != 0){
			serverSearch = ServerSearch.ServerFound;
		}
		else {
			serverSearch = ServerSearch.NoServerFound;
		} 

	}

	private void MenuButtons(string[] buttonTexts, MenuScreen[] menus){
		int amount = buttonTexts.Length;
		//Menu Buttons
		for(int i = 0; i < amount; i++){
			if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * i + 0 * 10, buttonWidth, buttonHeight), buttonTexts[i], buttonText)){
				curMenu = menus[i];
			}
		}
	}

	private void BackButton(MenuScreen backButtonLocation){
		//Back Button
		if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
			curMenu = backButtonLocation;
		}
	}

	private void ExitButton(){
		if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Exit", buttonText)){
			Application.Quit();
		}
	}

	private void RefreshServerList(){
		StartCoroutine(TalkToMasterServer());
	}
	


	private void CreateServerList(){

		//Clear serverList
		serverList.Clear();
		serverList.TrimExcess();

		foreach(HostData hd in hostData){
			Server server = new Server();
			server.hostData = hd;
			Ping ping = new Ping(hd.ip[0]);
			if(ping.isDone){
				if(ping.time <= 0) {
					server.ping = "0";
				} else {
					server.ping = ping.time.ToString();
				}
			} else {
				server.ping = "N/A";
			}
			
			
			serverList.Add (server);
		}

	}

	private void RefreshFilter(){
		switch(filter){
		case Filter.Servername_Asc:
			serverList = serverList.OrderBy(o=>o.hostData.gameName).ToList();
			break;
			
		case Filter.Servername_Desc:
			serverList = serverList.OrderByDescending(o=>o.hostData.gameName).ToList();
			break;
			
		case Filter.Players_Asc:
			serverList = serverList.OrderBy(o=>o.hostData.connectedPlayers).ToList();
			break;
			
		case Filter.Players_Desc:
			serverList = serverList.OrderByDescending(o=>o.hostData.connectedPlayers).ToList();
			break;
			
		case Filter.Ping_Asc:
			serverList = serverList.OrderBy(o=>o.ping).ToList();
			break;
			
		case Filter.Ping_Desc:
			serverList = serverList.OrderByDescending(o=>o.ping).ToList();
			break;
			
		case Filter.Gamemode_All:
			break;
			
		case Filter.Gamemode_Coop:

			break;
			
		case Filter.Gamemode_Normal:

			break;
		default:
			break;
		}
	}
}
