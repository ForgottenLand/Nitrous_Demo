var count:int = 0;
var wins:int = 0;
var style:GUIStyle;

function OnCollisionEnter(myCollision : Collision){
	
	if(myCollision.gameObject.name == "Group1"){
		wins++;
	}
	
}

function OnGUI(){
	style.fontSize = 30;
	style.normal.textColor = Color.red;
	GUI.Label(Rect(30, 30, 1000, 100),"Let's practice driving!",style);
	GUI.Label(Rect(30, 80, 1000, 100),"Objective: Follow the path and find your friend Johnny.",style);
	GUI.Label(Rect(30, 130, 1000, 100),"He invited you to a car race named 'GEARS'.",style);
	if (wins > 0) {
		GUI.Label(Rect(30, 180, 1000, 100),"After some chats, Johnny brought you to the race.",style);
		GUI.Label(Rect(30, 230, 1000, 100),"Loading the racing map...",style);
		Application.LoadLevel("Gears");
	}
}