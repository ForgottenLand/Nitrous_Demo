var count:int = 0;
var wins:int = 0;
var style:GUIStyle;

function OnTriggerExit (myTrigger : Collider) {
	
	if(myTrigger.gameObject.name == "FinishLineDetection"){
		if(count >= 5){
			wins++;
			count = 0;
		} else {
			count++;
		}
	}
	
}

function OnGUI(){
	style.fontSize = 40;
	style.normal.textColor = Color.white;
	GUI.Label(Rect(30, 20, 1000, 100),"STATS",style);
	style.normal.textColor = Color.red;
	GUI.Label(Rect(30, 70, 1000, 100),"You: " + wins + " out of 3",style);
	if (wins >= 3) {
		Application.LoadLevel("Credits");
	}
}