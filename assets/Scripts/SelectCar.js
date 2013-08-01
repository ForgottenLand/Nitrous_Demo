var carNum : float;
var selected : boolean;
var targetTranform : Transform;
var camIsMoved : boolean;

function Start () {
	carNum = 0;
	selected = false;
	camIsMoved = false;
}

function Update () {
	if(camera.enabled){
		//Input processing
		if(Input.GetKeyDown("a")){
			Debug.Log("a is down");
			//Check if cam can move left by one
			if(carNum >= 1){
			//if it can, move one
				carNum -= 1;
				camIsMoved = false;
				Debug.Log("carNum is:" + carNum);
			}	
		}else if(Input.GetKeyDown("d")){
			Debug.Log("d is down");
			//Check if cam can move right by one
			if(carNum <= 3){
			//if it can, move one
				carNum += 1;
				camIsMoved = false;
				Debug.Log("carNum is:" + carNum);
			}		
		}else if (Input.GetKeyDown("space")){
			Debug.Log("space is down");
			selected = true;
		}
		
		//Play Animation
	
		switch(carNum){
			case 0:
				MoveCamTransform(GameObject.Find("Pgt").transform);
				camera.cullingMask = -257;
			break;
			case 1:
				MoveCamTransform(GameObject.Find("Charger").transform);
				camera.cullingMask = -513;
			break;
			case 2:
				MoveCamTransform(GameObject.Find("Colt").transform);
				camera.cullingMask = -513;
			break;
			case 3:
				MoveCamTransform(GameObject.Find("Nitrous").transform);
				camera.cullingMask = -513;
			break;
			case 4:
				MoveCamTransform(GameObject.Find("Lambo").transform);
				camera.cullingMask = -513;
			break;
		}
	}
}

function MoveCamTransform(targetTranform : Transform){
	if(!camIsMoved){
		camIsMoved = true;
		transform.position = targetTranform.position;
		transform.position.x -= 3.3;
		transform.position.z -= 3.3;
		transform.position.y += 0.8;
	}
	transform.LookAt(targetTranform);
	transform.Translate(Vector3.right * Time.deltaTime * 2);	
}