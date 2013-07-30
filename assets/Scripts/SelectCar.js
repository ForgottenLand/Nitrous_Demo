var carNum : float;

function Start () {
	carNum = 0;
}

function Update () {
	if(Input.GetKeyDown("a")){
		Debug.Log("a is down");
		//Check if cam can move left by one
		
		//if it can, move one
		
	}else if(Input.GetKeyDown("d")){
		Debug.Log("d is down");
		//Check if cam can move right by one
		
		//if it can, move one
		
	}else if (Input.GetKeyDown("Enter")){
		Debug.Log("enter is down");
		
	}
}