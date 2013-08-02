var carNum : float;
var selected : boolean;
var targetTranform : Transform;
var camIsMoved : boolean;
var touchPosition : Vector3;
var leftChevron : GUITexture;
var rightChevron : GUITexture;

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
			MoveCam("left");
		}else if(Input.GetKeyDown("d")){
			Debug.Log("d is down");
			MoveCam("right");
		}else if (Input.GetKeyDown("space")){
			Debug.Log("space is down");
			selected = true;
		}
		
		//Play Animation
	
		//Debug.Log(camera.cullingMask.ToString());
		switch(carNum){
			case 0:
				MoveCamTransform(GameObject.Find("Pgt").transform);
				camera.cullingMask = -1281;
			break;
			case 1:
				MoveCamTransform(GameObject.Find("Charger").transform);
				camera.cullingMask = -8705;
			break;
			case 2:
				MoveCamTransform(GameObject.Find("Colt").transform);
				camera.cullingMask = -8705;
			break;
			case 3:
				MoveCamTransform(GameObject.Find("Nitrous").transform);
				camera.cullingMask = -8705;
			break;
			case 4:
				MoveCamTransform(GameObject.Find("Lambo").transform);
				camera.cullingMask = -10753;
			break;
		}
		
		if(Application.platform == RuntimePlatform.Android && Input.touchCount > 0){
			touchPosition = Input.GetTouch(0).position;
			TouchEventHandler(touchPosition.x, touchPosition.y);
		}
		else if(Application.platform == RuntimePlatform.WindowsEditor && Input.GetMouseButtonDown(0)){
			touchPosition = Input.mousePosition;
			TouchEventHandler(touchPosition.x, touchPosition.y);
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

function TouchEventHandler(xPos : int, yPos : int){
	Debug.Log("x: " + touchPosition.x);
	Debug.Log("y: " + touchPosition.y);
	//Left chevron is touched/clicked
//	Debug.Log("leftChevron.pixelInset.x: " + leftChevron.pixelInset.x);
//	Debug.Log("leftChevron.pixelInset.width: " + leftChevron.pixelInset.width);
//	Debug.Log("leftChevron.pixelInset.y: " + leftChevron.pixelInset.y);
//	Debug.Log("leftChevron.pixelInset.height: " + leftChevron.pixelInset.height);
	if((xPos > leftChevron.pixelInset.x && xPos < leftChevron.pixelInset.x + leftChevron.pixelInset.width) && (yPos > leftChevron.pixelInset.y && yPos < leftChevron.pixelInset.y + leftChevron.pixelInset.height)){
		MoveCam("left");
	}
	//Right chevron is touched/clicked
	if((xPos > rightChevron.pixelInset.x && xPos < rightChevron.pixelInset.x + rightChevron.pixelInset.width) && (yPos > rightChevron.pixelInset.y && yPos < rightChevron.pixelInset.y + rightChevron.pixelInset.height)){
		MoveCam("right");
	}
}

function MoveCam(dir : String){
	//Check if cam can move left by one
	
	if(dir == "left" && carNum >= 1){
		//if it can, move one
		carNum -= 1;
		camIsMoved = false;
		Debug.Log("Move left");
	}

	//Check if cam can move right by one
	if(dir == "right" && carNum <= 3){
	//if it can, move one
		carNum += 1;
		camIsMoved = false;
		Debug.Log("Move right");
	}
		
}