var gameType = "Multiplayer Testing";
var gameName = "Multiplayer Testing";
var MasterIp : String;
var RemotePort : int;
var isMasterServer : boolean;

var btnX:float;
var btnY:float;
var btnW:float;
var btnH:float;

var stringId : String;
public static var MasterServerClicked : boolean;
var client : ClientNetwork;

var oldLog = new Array();
var newLog = new Array();

function Start () {
	btnX = Screen.width * 0.01;
	btnY = Screen.width * 0.01;
	btnW = Screen.width * 0.3;
	btnH = Screen.width * 0.05;
	
	MasterIp = "192.168.0.100";
//	MasterIp = "172.20.1.229";
	RemotePort = 25003;
	if(Network.player.ipAddress == MasterIp){
		isMasterServer = true;
	} else {
		isMasterServer = false;
	}
	
//	isMasterServer = true;
	MasterServerClicked = false;
	
}

function OnGUI () {
	if(MasterServerClicked){
		gameName = GUI.TextField(Rect(btnX, btnY, btnW, btnH),gameName);
		RemotePort = int.Parse(GUI.TextField(Rect(btnX + btnW * 1.04, btnY, btnW, btnH),RemotePort.ToString()));
		
		if(GUI.Button(Rect(btnX + btnW * 2.08, btnY, btnW, btnH), "Add Host")){
			if(isMasterServer){
				AddHost();
			} else {
				Debug.Log("Not eligible to add host");
			}
		}
		
		stringId = GUI.TextField(Rect(btnX, btnY * 7, btnW, btnH),stringId);
		
		if(GUI.Button(Rect(btnX + btnW * 1.04, btnY * 7, btnW, btnH), "Delete Host")){
			if(isMasterServer){
				Debug.Log("Deleting a host");
				DeleteHost(int.Parse(stringId));
			} else {
				Debug.Log("Not eligible to delete host");
			}
		}
		
		if(GUI.Button(Rect(btnX, btnY * 13, btnW, btnH), "Return")){
			Debug.Log("Returning to main menu");
			MasterServerClicked = false;
			Application.LoadLevel(0);
		}
	}
}

function AddHost () {
	try{
		Network.InitializeServer(32,RemotePort,!Network.HavePublicAddress); 
		MasterServer.RegisterHost(gameType, gameName, "This is a test");
		for (var go : GameObject in FindObjectsOfType(GameObject)){
	 		go.SendMessage("OnLoaded", SendMessageOptions.DontRequireReceiver);	
		}
		Debug.Log("Server is initialized");
	}
	catch(UnityException){
		Debug.Log("Port is already in used, please select a different one");
	}
}

function Update () {
	if(isMasterServer){
		newLog = client.newLog;
		if(newLog.length != oldLog.length){
			Debug.Log("Receive new request!");
			RunAutoIt();
			Debug.Log(oldLog.length);
			Debug.Log(newLog.length);
			oldLog = newLog;
		}
	}
}

function DeleteHost (Id : int) {
	
}

function OnMasterServerEvent(mse:MasterServerEvent){
	if(mse == MasterServerEvent.RegistrationSucceeded){
		Debug.Log("Registered Server!");
	}
}

function OnApplicationQuit(){
	if(isMasterServer){
		MasterServer.UnregisterHost();
		Network.Disconnect();
	}
}

function RunAutoIt(){
	if(Application.platform != RuntimePlatform.WindowsEditor){
		var fileLocation = "";
		if(MasterIp == "192.168.0.100")
			fileLocation = "C:/MultiplayerProject/Assets/Scripts/IdeaPad.au3";
		else if(MasterIp == "172.20.1.229")
			fileLocation = "C:/MultiplayerProject/Assets/Scripts/Kobo.au3";
		System.Diagnostics.Process.Start(fileLocation);
		Debug.Log("Started: " + fileLocation);
	}
}