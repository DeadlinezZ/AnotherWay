using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

	public enum MenuScreen {
		Main,
		Multiplayer,
		MultiplayerLogin,
		MultiplayerRegister,
		MultiplayerLobby,
		MultiplayerDirect,
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
	private List<Ping> serverPingList = new List<Ping>();
	private Vector2 scrollPosition = Vector2.zero;
	private int lobbyRowHeight = 25;
	private int lobbyRowWidth = 120;
	public GUIStyle lobbyHeaderStyle = new GUIStyle();
	public GUIStyle lobbyRowStyle = new GUIStyle();
	private GUIStyle lobbyButton;
	private int lobbyFontSize = 13;
	private GUIStyle lobbyBoxStyle;
	private GUIStyle guiElementStyle;
	
	private bool enterPressed = false;

	// Use this for initialization
	void Start () {
		curMenu = MenuScreen.Main;
		serverSearch = ServerSearch.Searching;

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


		GUI.depth = 0;
		//Background Boxes
		GUI.DrawTexture(new Rect(0, menuTopIndent, menuWidth, menuHeight), backgroundTexMenu); // menu box
		GUI.DrawTexture(new Rect(settingsLeftIndent, settingsTopIndent, settingsWidth, settingsHeight), backgroundTexSettings); // settings box
		GUI.DrawTexture(new Rect(settingsLeftIndent - 100, settingsTopIndent, settingsWidth, settingsHeight), backgroundBackground); // background image
		GUI.DrawTexture(new Rect(settingsLeftIndent - middleSpacer, 0, middleSpacer, settingsHeight), lobbyBar); //middle bar
		GUI.Box (new Rect(0,0, Screen.width, Screen.height), ""); //dark overlay


		switch(curMenu){
			case MenuScreen.Main:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Main Menu", headerText);
					MenuButtons(new string[]{ "Multiplayer", "Singleplayer", "Settings", "Credits"}, 
								new MenuScreen[]{MenuScreen.Multiplayer, MenuScreen.Singleplayer, MenuScreen.SettingsAudio, MenuScreen.Credits});
					ExitButton();
				break;

			case MenuScreen.Multiplayer:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Overview", headerText);
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Overview", buttonText)){
						curMenu = MenuScreen.Multiplayer;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Lobby", buttonText)){
						curMenu = MenuScreen.MultiplayerLobby;
						StartCoroutine(TalkToMasterServer());
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Direct Join", buttonText)){
						curMenu = MenuScreen.MultiplayerDirect;
					}
					BackButton(MenuScreen.Main);
				break;

			case MenuScreen.MultiplayerLogin:
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
				break;

			case MenuScreen.MultiplayerRegister:
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
				break;
			
			case MenuScreen.MultiplayerLobby:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Lobby", headerText);
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Overview", buttonText)){
						curMenu = MenuScreen.Multiplayer;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Lobby", buttonText)){
						curMenu = MenuScreen.MultiplayerLobby;
						StartCoroutine(TalkToMasterServer());
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Direct Join", buttonText)){
						curMenu = MenuScreen.MultiplayerDirect;
					}
					BackButton(MenuScreen.Main);
					

					MultiplayerLobby();
				break;

			case MenuScreen.MultiplayerDirect:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Direct Join", headerText);
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Overview", buttonText)){
						curMenu = MenuScreen.Multiplayer;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Lobby", buttonText)){
						curMenu = MenuScreen.MultiplayerLobby;
						StartCoroutine(TalkToMasterServer());
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Direct Join", buttonText)){
						curMenu = MenuScreen.MultiplayerDirect;
					}
					BackButton(MenuScreen.Main);
				break;

			case MenuScreen.Singleplayer:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Singleplayer", headerText);
					MenuButtons(new string[]{ "New Game", "Load Game"}, 
								new MenuScreen[]{MenuScreen.SingleplayerNewGame, MenuScreen.SingleplayerLoadGame});
					BackButton(MenuScreen.Main);
				break;

			case MenuScreen.SingleplayerLoadGame:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Load Singleplayer Game", headerText);				
					MenuButtons(new string[]{ "New Game", "Load Game"}, 
								new MenuScreen[]{MenuScreen.SingleplayerNewGame, MenuScreen.SingleplayerLoadGame});				
					BackButton(MenuScreen.Main);	
				break;

			case MenuScreen.SingleplayerNewGame:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "New Singleplayer Game", headerText);				
					MenuButtons(new string[]{ "New Game", "Load Game"}, 
								new MenuScreen[]{MenuScreen.SingleplayerNewGame, MenuScreen.SingleplayerLoadGame});					
					BackButton(MenuScreen.Main);
				break;
			
			case MenuScreen.SettingsAudio:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Audio Settings", headerText);
					MenuButtons(new string[]{ "Audio", "Graphics", "Keys & Mouse"}, 
								new MenuScreen[]{MenuScreen.SettingsAudio, MenuScreen.SettingsGraphics, MenuScreen.SettingsKeys});					
					BackButton(MenuScreen.Main);
				break;

			case MenuScreen.SettingsGraphics:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Graphics Settings", headerText);
					MenuButtons(new string[]{ "Audio", "Graphics", "Keys & Mouse"}, 
								new MenuScreen[]{MenuScreen.SettingsAudio, MenuScreen.SettingsGraphics, MenuScreen.SettingsKeys});					
					BackButton(MenuScreen.Main);
				break;

			case MenuScreen.SettingsKeys:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Keys & Mouse Settings", headerText);
					MenuButtons(new string[]{ "Audio", "Graphics", "Keys & Mouse"}, 
								new MenuScreen[]{MenuScreen.SettingsAudio, MenuScreen.SettingsGraphics, MenuScreen.SettingsKeys});					
					BackButton(MenuScreen.Main);
				break;

			case MenuScreen.Credits:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Credits", headerText);
					BackButton(MenuScreen.Main);
				break;
		}
	}


	private void MultiplayerLobby(){

		GUILayout.BeginArea(new Rect(settingsLeftIndent + 50, headerTopIndent + 50, settingsWidth - 100, settingsHeight - headerTopIndent - 100), "");

		GUILayout.Box("", lobbyBoxStyle, GUILayout.Height(2));
		GUILayout.Space (15);

		//If at least one server was found
		if(serverSearch == ServerSearch.ServerFound){
			//Headers
			GUILayout.BeginHorizontal();

			GUILayout.Label ("Server", lobbyHeaderStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));

			GUILayout.Space(10);

			GUILayout.Label ("Players", lobbyHeaderStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));

			GUILayout.Space(10);

			GUILayout.Label ("Ping", lobbyHeaderStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));

			GUILayout.Space(10);

			GUILayout.Label ("", lobbyHeaderStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));

			GUILayout.EndHorizontal();

			//Hosts
			scrollPosition = GUILayout.BeginScrollView(scrollPosition,false,false);
			
			//Go through the hostlist and set them on the list
			for(int i = 0; i < hostData.Length; i++){
				GUILayout.BeginHorizontal();
				
				//ServerName
				GUILayout.Label(hostData[i].gameName, lobbyRowStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));
				GUILayout.Space(10);

				//Connected Players
				GUILayout.Label ((hostData[i].connectedPlayers -1) + "/" + (hostData[i].playerLimit - 1), lobbyRowStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));
				GUILayout.Space(10);

				//Latency
				if(serverPingList[i].isDone){
					if(serverPingList[i].time <= 0){
						GUILayout.Label("0",lobbyRowStyle, GUILayout.Height(lobbyRowHeight), GUILayout.Width (lobbyRowWidth));
					} else {
						GUILayout.Label(serverPingList[i].time.ToString(), lobbyRowStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width (lobbyRowWidth));
					}
				} else {
					GUILayout.Label("N/A", lobbyRowStyle, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth));
				}
				GUILayout.Space(10);


				//A Button for each host to connect
				if(GUILayout.Button("Connect", lobbyButton, GUILayout.Height (lobbyRowHeight), GUILayout.Width(lobbyRowWidth))){			
					//Connect to the server
					Network.Connect(hostData[i]);
				}

				GUILayout.EndHorizontal();
				GUILayout.Space(10);
			}
			
			GUILayout.EndScrollView();
			scrollPosition = Vector2.zero;
		}
		
		else if(serverSearch == ServerSearch.NoServerFound){
			scrollPosition = GUILayout.BeginScrollView(scrollPosition,false,false);
			GUILayout.Label("No servers found..");
			GUILayout.EndScrollView();
			scrollPosition = Vector2.zero;

		}
		
		//If there was no server found yet
		else if(serverSearch == ServerSearch.Searching){
			scrollPosition = GUILayout.BeginScrollView(scrollPosition,false,false);
			GUILayout.Label("Searching for servers..");
			GUILayout.EndScrollView();
			scrollPosition = Vector2.zero;
		}

		GUILayout.Space(15);
		GUILayout.Box ("", lobbyBoxStyle, GUILayout.Height (2));
		GUILayout.Space (10);
		//Refresh Button
		if(GUILayout.Button("Refresh", lobbyButton, GUILayout.Height(lobbyRowHeight), GUILayout.Width (lobbyRowWidth))){
			StartCoroutine(TalkToMasterServer());
		}


		GUILayout.EndArea();
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
		
		if(hostData.Length == 0){
			serverSearch = ServerSearch.NoServerFound;
		} 
		//Clear the serverPing list used to show the different servers ping later
		serverPingList.Clear();
		serverPingList.TrimExcess();
		//Fill the list with all server pings
		if(hostData.Length != 0){
			foreach(HostData hd in hostData){
				serverPingList.Add(new Ping(hd.ip[0]));
			}
			serverSearch = ServerSearch.ServerFound;
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


}
