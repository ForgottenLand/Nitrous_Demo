var forwardSpeed : float = 180000;
var turnSpeed : float = 5;
var style:GUIStyle;

function OnGUI () {

	if (GUI.Button(Rect(Screen.width - 100, Screen.height - 50, 100, 50),"Restart level"))
	{
		Application.LoadLevel("Gears");
	}
	if (GUI.Button(Rect(0, Screen.height - 50, 100,50),"See Credits"))
	{
		Application.LoadLevel("Credits");
	}
	style.fontStyle = FontStyle.Italic;
	style.normal.textColor = Color.white;
	style.fontSize = 40;
	GUI.Label(Rect(Screen.width - 370, 80, 100, 100),"Speed: ", style);
	style.fontSize = 110;
	style.fontStyle = FontStyle.BoldAndItalic;
	if (Input.touches.Length == 0) {
		
		style.normal.textColor = Color.white;
		GUI.Label(Rect(Screen.width - 220, 40, 100, 100),"X 0", style);
		if(Input.acceleration.x < -0.05){
			transform.Rotate(0,Input.acceleration.x * turnSpeed,0); 
		} else if(Input.acceleration.x > 0.05){
			transform.Rotate(0,Input.acceleration.x * turnSpeed,0); 
		}
		rigidbody.AddRelativeForce(0,-Physics.gravity.magnitude * 4000,0);
		
			
	} else if(Input.touches.Length == 1){
		
		style.normal.textColor = Color.green;
		GUI.Label(Rect(Screen.width - 220, 40, 100, 100),"X 1", style);
		rigidbody.AddRelativeForce(forwardSpeed,0,0);
		if(Input.acceleration.x < -0.05){
			transform.Rotate(0,Input.acceleration.x * (turnSpeed-1),0); 
		} else if(Input.acceleration.x > 0.05){
			transform.Rotate(0,Input.acceleration.x * (turnSpeed-1),0); 
		}
		rigidbody.AddRelativeForce(0,-Physics.gravity.magnitude * 4000,0);
	
	} else if(Input.touches.Length == 2){
		
		style.normal.textColor = Color.blue;
		GUI.Label(Rect(Screen.width - 220, 40, 100, 100),"X 2", style);
		rigidbody.AddRelativeForce(forwardSpeed * 2,0,0);
		if(Input.acceleration.x < -0.05){
			transform.Rotate(0,Input.acceleration.x * (turnSpeed-2),0); 
		} else if(Input.acceleration.x > 0.05){
			transform.Rotate(0,Input.acceleration.x * (turnSpeed-2),0); 
		}
		rigidbody.AddRelativeForce(0,-Physics.gravity.magnitude * 8000,0);
		
	} else if(Input.touches.Length == 3){
		
		style.normal.textColor = Color.yellow;
		GUI.Label(Rect(Screen.width - 220, 40, 100, 100),"X 3", style);
		rigidbody.AddRelativeForce(forwardSpeed * 3,0,0);
		if(Input.acceleration.x < -0.05){
			transform.Rotate(0,Input.acceleration.x * (turnSpeed-3),0); 
		} else if(Input.acceleration.x > 0.05){
			transform.Rotate(0,Input.acceleration.x * (turnSpeed-3),0); 
		}
		rigidbody.AddRelativeForce(0,-Physics.gravity.magnitude * 16000,0);
		
	} else if(Input.touches.Length > 3){
		
		style.normal.textColor = Color.magenta;
		GUI.Label(Rect(Screen.width - 220, 40, 100, 100),"X 4", style);
		rigidbody.AddRelativeForce(forwardSpeed * 4,0,0);
		if(Input.acceleration.x < -0.05){
			transform.Rotate(0,Input.acceleration.x * (turnSpeed-4),0); 
		} else if(Input.acceleration.x > 0.05){
			transform.Rotate(0,Input.acceleration.x * (turnSpeed-4),0); 
		}
		rigidbody.AddRelativeForce(0,-Physics.gravity.magnitude * 24000,0);
	}
	
}