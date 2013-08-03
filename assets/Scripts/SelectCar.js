var carNum : float;
var selected : boolean;
var targetTranform : Transform;
var camIsMoved : boolean;
var touchPosition : Vector2;
var leftChevron : GUITexture;
var rightChevron : GUITexture;
var select : GUITexture;
var pgt : GameObject;
var charger : GameObject;
var colt : GameObject;
var nitrous : GameObject;
var lambo : GameObject;
var gameCamera : Camera;
var pgtAdminControl : PgtAdminControl;

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
				camera.cullingMask = -17665;
			break;
			case 1:
				MoveCamTransform(GameObject.Find("Charger").transform);
				camera.cullingMask = -25089;
			break;
			case 2:
				MoveCamTransform(GameObject.Find("Colt").transform);
				camera.cullingMask = -8961;
			break;
			case 3:
				MoveCamTransform(GameObject.Find("Nitrous").transform);
				camera.cullingMask = -8961;
			break;
			case 4:
				MoveCamTransform(GameObject.Find("Lambo").transform);
				camera.cullingMask = -11009;
			break;
		}
		
		if(Application.platform == RuntimePlatform.Android && Input.GetTouch(0).phase == TouchPhase.Began && Input.touchCount == 1){
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
		if(Application.platform == RuntimePlatform.Android){
			transform.position.x -= 2.8;
			transform.position.z -= 2.8;
		} else {
			transform.position.x -= 3.3;
			transform.position.z -= 3.3;
		}
		transform.position.y += 0.8;
	}
	transform.LookAt(targetTranform);
	transform.Translate(Vector3.right * Time.deltaTime * 2);	
}

function TouchEventHandler(xPos : int, yPos : int){
//	Debug.Log("x: " + touchPosition.x);
//	Debug.Log("y: " + touchPosition.y);
//	Debug.Log("leftChevron.pixelInset.x: " + leftChevron.pixelInset.x);
//	Debug.Log("leftChevron.pixelInset.width: " + leftChevron.pixelInset.width);
//	Debug.Log("leftChevron.pixelInset.y: " + leftChevron.pixelInset.y);
//	Debug.Log("leftChevron.pixelInset.height: " + leftChevron.pixelInset.height);
	
	//Left chevron is touched/clicked
	if((xPos > leftChevron.pixelInset.x && xPos < leftChevron.pixelInset.x + leftChevron.pixelInset.width) && (yPos > leftChevron.pixelInset.y && yPos < leftChevron.pixelInset.y + leftChevron.pixelInset.height)){
		MoveCam("left");
	}
	//Right chevron is touched/clicked
	if((xPos > rightChevron.pixelInset.x && xPos < rightChevron.pixelInset.x + rightChevron.pixelInset.width) && (yPos > rightChevron.pixelInset.y && yPos < rightChevron.pixelInset.y + rightChevron.pixelInset.height)){
		MoveCam("right");
	}
	
	//Select texture is touched/clicked
	if((xPos > select.pixelInset.x && xPos < select.pixelInset.x + select.pixelInset.width) && (yPos > select.pixelInset.y && yPos < select.pixelInset.y + select.pixelInset.height)){
		StartGame();
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

function StartGame(){
	if(carNum == 0){
		Debug.Log("Start Game!");
		nitrous.active = false;
		colt.active = false;
		lambo.active = false;
		gameCamera.enabled = true;
		camera.enabled = false;
		pgtAdminControl.FindAvatar0();
		pgt.rigidbody.constraints.value__ = 0;
	}
}