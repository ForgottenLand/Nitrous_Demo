var style:GUIStyle;
var NotYetDisplayed : boolean;

function Start(){
	NotYetDisplayed = true;
}

function OnGUI () {
	if(NotYetDisplayed){
		AdBuddizBinding.showAd();
		NotYetDisplayed = false;		
	}
	style.fontSize = 80;
	style.normal.textColor = Color.cyan;
	GUI.Label(Rect(430, 30, 1000, 100),"Waterlooers I",style);
	style.fontSize = 40;
	GUI.Label(Rect(360, 150, 1000, 100),"presented by Forgottenland Studio",style);
	style.normal.textColor = Color.yellow;
	style.fontSize = 40;
	GUI.Label(Rect(570, 250, 1000, 100),"Instruction:",style);
	style.fontSize = 30;
	GUI.Label(Rect(120, 330, 1000, 100),"Touch screen to accelerate, tilt phone to turn and multi-finger touch to GEAR up",style);
	style.normal.textColor = Color.red;
	style.fontSize = 50;
	GUI.Label(Rect(220, 460, 1000, 100),"Start practice map with a screen touch",style);
	if(Input.touches.Length > 0){
		GUI.Label(Rect(400, 540, 1000, 100),"Loading practice map...",style);
		Application.LoadLevel("level2Final");
	}
}