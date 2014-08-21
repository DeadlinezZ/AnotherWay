using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	private enum MenuScreen {
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

	private MenuScreen curMenu;

	private ConnectionManager connectionManager;

	private int windowWidth;
	private int windowHeight;
	
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
	
	private float textfieldIntWidth;
	
	public GUIStyle headerText;
	public GUIStyle labelText;
	private GUIStyle buttonText;

	private string lusername = "";
	private string lpassword = "";
	private string rusername = "";
	private string rpassword = "";
	private string rpassword2 = "";
	private string remail = "";

	// Use this for initialization
	void Start () {
		curMenu = MenuScreen.Main;

		connectionManager = GameObject.Find ("ConnectionManager").GetComponent<ConnectionManager>();

		windowWidth = Screen.width;
		windowHeight = Screen.height;
		
		menuTopIndent = 0;
		menuHeight = windowHeight;
		menuWidth = windowWidth / 3;
		
		settingsTopIndent = 0;
		settingsLeftIndent = windowWidth / 3 + 10;
		settingsHeight = windowHeight;
		settingsWidth = windowWidth / 3 * 2 - 10;
		
		buttonWidth = menuWidth;
		buttonHeight = 40;
		
		labelWidth = 100;
		labelHeight = 30;

		textfieldIntWidth = 60f;

		headerLeftIndent = settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth / 2;
		headerTopIndent = settingsTopIndent + 100;

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
	}
	
	// Update is called once per frame
	void Update () {
		if(curMenu == MenuScreen.Multiplayer && !connectionManager.loggedIn){
			curMenu = MenuScreen.MultiplayerLogin;
		} 
	}

	void OnGUI(){

		//Background Boxes
		GUI.Box(new Rect(0, menuTopIndent, menuWidth, menuHeight), "");
		GUI.Box(new Rect(settingsLeftIndent, settingsTopIndent, settingsWidth, settingsHeight), "");

		switch(curMenu){
			case MenuScreen.Main:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Main Menu", headerText);
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Multiplayer", buttonText)){
						curMenu = MenuScreen.Multiplayer;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Singleplayer", buttonText)){
						curMenu = MenuScreen.Singleplayer;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Settings", buttonText)){
						curMenu = MenuScreen.SettingsAudio;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 3 + 3 * 10, buttonWidth, buttonHeight), "Credits", buttonText)){
						curMenu = MenuScreen.Credits;
					}
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Exit", buttonText)){
						Application.Quit();
					}
				break;

			case MenuScreen.Multiplayer:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Multiplayer", headerText);
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Main;
					}
				break;

			case MenuScreen.MultiplayerLogin:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Login", headerText);

					//Username
					GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 200, 200, 20), "Username", labelText);
					GUI.SetNextControlName("Username");
					lusername = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 220, 200, 20), lusername);
			                          
					//Password
					GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 250, 200, 20), "Password", labelText);
					GUI.SetNextControlName("Password");
					lpassword = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 270, 200, 20), lpassword);                
			        
					if(GUI.Button (new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 310, 200, 35),"Login")){
						connectionManager.Login(lusername, lpassword);
					}
			
					if (GUI.GetNameOfFocusedControl() == string.Empty) {
						GUI.FocusControl("Username");
					}

					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Login", buttonText)){
						curMenu = MenuScreen.MultiplayerLogin;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Register", buttonText)){
						curMenu = MenuScreen.MultiplayerRegister;
					}
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Main;
					}
					
				break;

			case MenuScreen.MultiplayerRegister:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Register", headerText);

					
					//Username
					GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 200, 200, 20), "Username", labelText);
					GUI.SetNextControlName("Username");
					rusername = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 220, 200, 20), rusername);

					//Email
					GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 250, 200, 20), "E-Mail", labelText);
					GUI.SetNextControlName("Email");
					remail = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 270, 200, 20), remail);

					//Password 1
					GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 300, 200, 20), "Password", labelText);
					GUI.SetNextControlName("Password1");		
					rpassword = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 320, 200, 20), rpassword);

					//Password 2
					GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 350, 200, 20), "Repeat Password", labelText);
					GUI.SetNextControlName("Password2");		
					rpassword2 = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 370, 200, 20), rpassword2);
		
					if(GUI.Button (new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 410, 200, 35),"Register")){
					}
			                    
					if (GUI.GetNameOfFocusedControl() == string.Empty) {
						GUI.FocusControl("Username");
					}

			        if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Login", buttonText)){
						curMenu = MenuScreen.MultiplayerLogin;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Register", buttonText)){
						curMenu = MenuScreen.MultiplayerRegister;
					}
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Main;
					}
				break;
			
			case MenuScreen.MultiplayerLobby:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Lobby", headerText);
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Multiplayer;
					}
				break;

			case MenuScreen.MultiplayerDirect:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Direct Join", headerText);
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Multiplayer;
					}
				break;

			case MenuScreen.Singleplayer:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Singleplayer", headerText);
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "New Game", buttonText)){
						curMenu = MenuScreen.SingleplayerNewGame;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Load Game", buttonText)){
						curMenu = MenuScreen.SingleplayerLoadGame;
					}
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Main;
					}	
				break;

			case MenuScreen.SingleplayerLoadGame:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Load Singleplayer Game", headerText);
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "New Game", buttonText)){
						curMenu = MenuScreen.SingleplayerNewGame;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Load Game", buttonText)){
						curMenu = MenuScreen.SingleplayerLoadGame;
					}
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Main;
					}	
				break;

			case MenuScreen.SingleplayerNewGame:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "New Singleplayer Game", headerText);
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "New Game", buttonText)){
						curMenu = MenuScreen.SingleplayerNewGame;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Load Game", buttonText)){
						curMenu = MenuScreen.SingleplayerLoadGame;
					}
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Main;
					}	
				break;
			
			case MenuScreen.SettingsAudio:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Audio Settings", headerText);
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Audio", buttonText)){
						curMenu = MenuScreen.SettingsAudio;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Graphics", buttonText)){
						curMenu = MenuScreen.SettingsGraphics;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Keys & Mouse", buttonText)){
						curMenu = MenuScreen.SettingsKeys;
					}
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Main;
					}
				break;

			case MenuScreen.SettingsGraphics:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Graphics Settings", headerText);
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Audio", buttonText)){
						curMenu = MenuScreen.SettingsAudio;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Graphics", buttonText)){
						curMenu = MenuScreen.SettingsGraphics;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Keys & Mouse", buttonText)){
						curMenu = MenuScreen.SettingsKeys;
					}
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Main;
					}
				break;

			case MenuScreen.SettingsKeys:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Keys & Mouse Settings", headerText);
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 0 + 0 * 10, buttonWidth, buttonHeight), "Audio", buttonText)){
						curMenu = MenuScreen.SettingsAudio;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 1 + 1 * 10, buttonWidth, buttonHeight), "Graphics", buttonText)){
						curMenu = MenuScreen.SettingsGraphics;
					}
					if(GUI.Button(new Rect(0, menuHeight / 3 + buttonHeight * 2 + 2 * 10, buttonWidth, buttonHeight), "Keys & Mouse", buttonText)){
						curMenu = MenuScreen.SettingsKeys;
					}
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Main;
					}
				break;

			case MenuScreen.Credits:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Credits", headerText);
					if(GUI.Button(new Rect(0, menuHeight -buttonHeight, buttonWidth, buttonHeight), "Back", buttonText)){
						curMenu = MenuScreen.Main;
					}	
				break;
		}
	}


}
