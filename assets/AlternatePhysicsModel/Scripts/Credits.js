var style:GUIStyle;

function OnGUI () {
	style.fontSize = 80;
	style.normal.textColor = Color.white;
	GUI.Label(Rect(470, 30, 1000, 100),"CREDITS",style);
	
	style.fontSize = 40;
	
	GUI.Label(Rect(20, 150, 1000, 100),"This application is built entirely by Yuzhi Wang and Chinmay Amin.",style);
	GUI.Label(Rect(20, 200, 1000, 100),"Credits to Unity Technologies for its open source car models",style);
	GUI.Label(Rect(20, 250, 1000, 100),"race track and audios. For more details, please visit:",style);
	GUI.Label(Rect(20, 300, 1000, 100),"https://www.assetstore.unity3d.com/#/content/10",style);
	if (GUI.Button(Rect(Screen.width - 100, Screen.height - 50, 100, 50),"Restart level"))
	{
		Application.LoadLevel("Gears");
	}
}