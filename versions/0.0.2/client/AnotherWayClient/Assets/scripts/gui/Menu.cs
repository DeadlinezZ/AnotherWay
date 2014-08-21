using UnityEngine;
using System.Collections;

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

	private DBManager DBManager;

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

	public string registerMessage = "";
	public string loginMessage = "";

	private float textfieldIntWidth;
	
	public GUIStyle headerText;
	public GUIStyle labelText;
	public GUIStyle	registerAndLoginMessage;
	private GUIStyle buttonText;

	public string lusername = "";
	public string lpassword = "";
	public string rusername = "";
	public string rpassword = "";
	public string rpassword2 = "";
	public string remail = "";

	// Use this for initialization
	void Start () {
		curMenu = MenuScreen.Main;

		DBManager = GameObject.Find ("DBManager").GetComponent<DBManager>();

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

		//textfieldIntWidth = 60f;

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
		if(curMenu == MenuScreen.Multiplayer && !DBManager.loggedIn){
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
					MenuButtons(new string[]{ "Multiplayer", "Singleplayer", "Settings", "Credits"}, 
								new MenuScreen[]{MenuScreen.Multiplayer, MenuScreen.Singleplayer, MenuScreen.SettingsAudio, MenuScreen.Credits});
					ExitButton();
				break;

			case MenuScreen.Multiplayer:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Overview", headerText);
					MenuButtons(new string[]{ "Overview", "Lobby", "Direct Join"}, 
								new MenuScreen[]{MenuScreen.Multiplayer, MenuScreen.MultiplayerLobby, MenuScreen.MultiplayerDirect});
					BackButton(MenuScreen.Main);
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
					lpassword = GUI.PasswordField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 270, 200, 20), lpassword, "*"[0], 30);                
			        
					if(GUI.Button (new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 310, 200, 35),"Login")){
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
					rusername = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 220, 200, 20), rusername);

					//Email
					GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 250, 200, 20), "E-Mail", labelText);
					GUI.SetNextControlName("Email");
					remail = GUI.TextField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 270, 200, 20), remail);

					//Password 1
					GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 300, 200, 20), "Password", labelText);
					GUI.SetNextControlName("Password1");		
					rpassword = GUI.PasswordField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 320, 200, 20), rpassword, "*"[0], 30);

					//Password 2
					GUI.Label(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth + 3, settingsTopIndent + 350, 200, 20), "Repeat Password", labelText);
					GUI.SetNextControlName("Password2");		
					rpassword2 = GUI.PasswordField(new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 370, 200, 20), rpassword2, "*"[0], 30);
		
					if(GUI.Button (new Rect(settingsLeftIndent + settingsWidth - settingsWidth / 2 - labelWidth, settingsTopIndent + 410, 200, 35),"Register")){
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
					MenuButtons(new string[]{"Overview", "Lobby", "Direct Join"}, 
								new MenuScreen[]{MenuScreen.Multiplayer, MenuScreen.MultiplayerLobby, MenuScreen.MultiplayerDirect});				
					BackButton(MenuScreen.Main);
				break;

			case MenuScreen.MultiplayerDirect:
					GUI.Label(new Rect(headerLeftIndent, headerTopIndent, labelWidth, labelHeight), "Direct Join", headerText);
					MenuButtons(new string[]{"Overview", "Lobby", "Direct Join"}, 
								new MenuScreen[]{MenuScreen.Multiplayer, MenuScreen.MultiplayerLobby, MenuScreen.MultiplayerDirect});
					
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
