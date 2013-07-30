var playerPrefab:GameObject;
var spawnObject:Transform;
var gameName = "CGCookie_Tutorial_Networking";

private var refreshing:boolean;
private var hostData:HostData[];

private var btnX:float;
private var btnY:float;
private var btnW:float;
private var btnH:float;


function Start() {
	btnX = Screen.width * 0.01;
	btnY = Screen.width * 0.01;
	btnW = Screen.width * 0.1;
	btnH = Screen.width * 0.05;
}

function startServer(){
	Network.InitializeServer(32,25002, !Network.HavePublicAddress);
	MasterServer.RegisterHost(gameName,"Tutorial Game Name","This is a tutorial game");
	camera.enabled = false;
}

function refreshHostList(){
	MasterServer.RequestHostList(gameName);
	refreshing = true;
}

function Update(){
	if(refreshing){
		if(MasterServer.PollHostList().Length > 0){
			refreshing = false;
			Debug.Log(MasterServer.PollHostList().Length);
			hostData = MasterServer.PollHostList();
		}
	}
}



function spawnPlayer(){
	Network.Instantiate(playerPrefab, spawnObject.position, Quaternion.identity, 0);
}

//Message
function OnServerInitialized(){
	Debug.Log("Server initialized!");
	spawnPlayer();
}

function OnConnectedToServer(){
	spawnPlayer();
}

function OnMasterServerEvent(mse:MasterServerEvent){
	if(mse == MasterServerEvent.RegistrationSucceeded){
		Debug.Log("Registered Server!");
	}
}

function OnGUI() {
	if(!Network.isClient && !Network.isServer){
		if(GUI.Button(Rect(btnX, btnY, btnW, btnH), "Start Server")){
			Debug.Log("Starting Server");
			startServer();
		}
	
			if(GUI.Button(Rect(btnX, btnY * 7, btnW, btnH), "Refresh Hosts")){
			Debug.Log("Refreshing");
			refreshHostList();
		}
		if(hostData){
			for(var i = 0; i < hostData.length; i++){
				if(GUI.Button(Rect(btnX  * 2 + btnW, btnY + (btnH*i), btnW * 3, btnH * 0.5),hostData[i].gameName)){
					Network.Connect(hostData[i]);
				}
			}
		}
	}
}