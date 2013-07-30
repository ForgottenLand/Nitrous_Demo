var wins:int = 0;
var style:GUIStyle;
var count:int = 0;

function OnTriggerExit (myTrigger : Collider) {
	
	if(myTrigger.gameObject.name == "FinishLineDetection"){
		count++;
	}
	
	if(count >= 5){
		wins++;
		count = 0;
	}
}

function OnGUI(){
	style.fontSize = 40;
	style.normal.textColor = Color.green;
	GUI.Label(Rect(30, 170, 1000, 100),"Charger: " + wins + " out of 3",style);
	if (wins >= 3) {
		Application.LoadLevel("Credits");
	}
}