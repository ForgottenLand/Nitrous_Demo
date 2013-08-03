//John Variables
//The car transforms
var avatar0 : Transform;
//var avatar1 : Transform;
//var avatar2 : Transform;
//var avatar3 : Transform;
//var avatar4 : Transform;

//the button dimensions
//var btnX:float;
//var btnY:float;
//var btnW:float;
//var btnH:float;

//Which car and whether you've picked it yet.
//var selectNumber : int;
//static var selected : boolean;
//var first : boolean;
//var second : boolean;
//var third : boolean;
//var fourth : boolean;
//var fifth : boolean;


//The Player GameObject and its name
var Player : GameObject;
var playerClone: String;
var style:GUIStyle;

//var nView:NetworkView;
//var networkGroup : int;
//var adminPanel : AdminPanel;

//Luke Variables---------------
var frLeft : WheelCollider;
var frRight : WheelCollider; 
var reLeft : WheelCollider; 
var reRight : WheelCollider;

var enginePower :float;
var maxSteer : float;

var power : float;
var brake : float;
var steer : float;
var speedTurn: float;
var speed : float;
var brakePower: float;

var drifting = false;
var driftingTimer : float;
var driftingTimeout : float;

var regFriction : float;
var driftFriction : float;
var driftFwdFriction : float;
var frictionRatio : float;

var startDrift;
var endDrift;
var pos;
var prevPos;

var startDriftAngVel : float;
var endDriftAngVel : float;

var rigidbodyAngVel : float;

var steerMultiplier : float;

var maxSpeed : float;
var minDriftSpeed : float;
//-----------------------------


function OnLoaded() {
//    btnX = Screen.width * 0.01;
//    btnY = Screen.width * 0.01;
//    btnW = Screen.width * 0.3;
//    btnH = Screen.width * 0.05;
     //reverse = false;
//    networkGroup = 0;
	
	enginePower = 60;
    maxSteer = 12;
    power = 0;
    brake = 0;
    steer = 0;
    speedTurn = 30;
    speed = 0;
    brakePower = 50;
    driftingTimeout = 0.15;
    regFriction = 0.3;
    driftFriction = 0.03;
    driftFwdFriction = 0.5;
    frictionRatio = 1.25;
    startDriftAngVel = 0.6;
    endDriftAngVel = 0.3;
    rigidbodyAngVel = 0;
    steerMultiplier = 1.75;
    maxSpeed = 280;
    minDriftSpeed = 130;
//  Network.SetReceivingEnabled(networkGroup,true);
//	Network.SetSendingEnabled(networkGroup,true);
}

function FindAvatar0(){ 
   
//    switch(selectNumber){
//        case 0:
//            Network.Instantiate(avatar0, transform.position, transform.rotation, networkGroup);
            playerClone = avatar0.name; 
            
//        break;
//        case 1:
//            Network.Instantiate(avatar1, transform.position, transform.rotation, networkGroup);
//            playerClone = avatar1.name+"(Clone)"; 
//
//        break;
//        case 2:
//            Network.Instantiate(avatar2, transform.position, transform.rotation, networkGroup);
//            playerClone = avatar2.name+"(Clone)"; 
//
//        break;
//        case 3:
//            Network.Instantiate(avatar3, transform.position, transform.rotation, networkGroup);
//            playerClone = avatar3.name+"(Clone)";
//
//        break;
//        case 4:
//            Network.Instantiate(avatar4, transform.position, transform.rotation, networkGroup);
//            playerClone = avatar4.name+"(Clone)"; 
//
//        break;
//    }
 	
  	Player = GameObject.Find(playerClone);
//  	nView = Player.networkView;
    Player.rigidbody.centerOfMass=Vector3(0,-0.9,0.3);
    pos = Player.transform.position;
    prevPos = pos;
    
    var carWheels = Player.GetComponentsInChildren(WheelCollider);
    
    for(var count = 0; count < 4; count++){
    	if(carWheels[count].ToString().Contains("FrontLeft")){
    		frLeft = carWheels[count];
    	} else if (carWheels[count].ToString().Contains("FrontRight")){
    		frRight = carWheels[count];
    	} else if (carWheels[count].ToString().Contains("RearLeft")){
    		reLeft = carWheels[count];
    	} else if (carWheels[count].ToString().Contains("RearRight")){
    		reRight = carWheels[count];
    	}
    	
    }
}

//function OnGUI(){
//    if(!adminPanel.adminPanelClicked){
//		if(!selected){
//			     if(GUI.Button(Rect(btnX, btnY * 3, btnW, btnH), avatar0.name)){
//			         selectNumber = 0;
//			         selected = true;
//			         SpawnCar();
//			     }
//			     if(GUI.Button(Rect(btnX, btnY * 9, btnW, btnH), avatar1.name)){
//			         selectNumber = 1;
//			         selected = true;
//			         SpawnCar();
//			     }
//			     if(GUI.Button(Rect(btnX, btnY * 15, btnW, btnH), avatar2.name)){
//			         selectNumber = 2;
//			         selected = true;
//			         SpawnCar();
//			     }
//			     if(GUI.Button(Rect(btnX, btnY * 21, btnW, btnH), avatar3.name)){
//			         selectNumber = 3;
//			         selected = true;
//			         SpawnCar();
//			     }
//			     if(GUI.Button(Rect(btnX, btnY * 27, btnW, btnH), avatar4.name)){
//			         selectNumber = 4;
//			         selected = true;
//			         SpawnCar();
//			     }  
//		}
//	}
//     
//     
//}
    

function Update()
{
	try{
//		style.fontStyle = FontStyle.Italic;
//	    style.normal.textColor = Color.white;
//	    style.fontSize = 40;
//	    GUI.Label(Rect(Screen.width - 370, 80, 100, 100),"Speed: ", style);
//	    style.fontSize = 40;
//	    style.fontStyle = FontStyle.BoldAndItalic;
//	    style.normal.textColor = Color.white;
		
		speed=Player.rigidbody.velocity.magnitude * 3.6;
		
	    power=Input.GetAxis("Vertical") * enginePower * Time.deltaTime * 250.0;
	    steer=Input.GetAxis("Horizontal") * maxSteer * Mathf.Clamp(speedTurn/speed, 0, 1);
	    brake=Input.GetButton("Jump") ? brakePower: 0.0;   
		
		
		rigidbodyAngVel = Player.rigidbody.angularVelocity.y;
		
		if((rigidbodyAngVel >= startDriftAngVel || rigidbodyAngVel <= - startDriftAngVel) && speed >= minDriftSpeed)
		{
			driftingTimer += Time.deltaTime;
		}
		if(driftingTimer >= driftingTimeout)
		{
			startDrift = true;
			driftingTimer = 0;
		}
		
		if(rigidbodyAngVel > 0)
		{
			if(rigidbodyAngVel <= endDriftAngVel || speed <= minDriftSpeed/3)
			{
				endDrift = true;
			}
		}
		else
		{
			if(rigidbodyAngVel >= -endDriftAngVel || speed <= minDriftSpeed/3)
			{
				endDrift = true;
			}
		}
	
		//determine this based on forward speed and (angularVelocity?) or another method of determining sideways force.
		if(startDrift)
		{
			drifting = true;
			startDrift = false;
		}
		else if(endDrift)
		{
			drifting = false;
			endDrift = false;
			driftingTimer = 0;
		}
	
	    
	    if(speed < maxSpeed)
	    {
	        frLeft.brakeTorque=power/10;
			frRight.brakeTorque=power/10;
	        reLeft.brakeTorque=power/10;
	        reRight.brakeTorque=power/10;
	        reLeft.motorTorque=power;
	        reRight.motorTorque=power;
	    }
	    else
	    {
	    	frLeft.brakeTorque=power/10;
			frRight.brakeTorque=power/10;
	        reLeft.brakeTorque=power/10;
	        reRight.brakeTorque=power/10;
	        reLeft.motorTorque=0;
	        reRight.motorTorque=0;
	    }
	    
	    if(drifting)
	    {
	    	if(rigidbodyAngVel > 0)
	    	{
	    		frLeft.sidewaysFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFriction, regFriction, rigidbodyAngVel/endDriftAngVel), 2), driftFriction, regFriction);
	        	frRight.sidewaysFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFriction, regFriction, rigidbodyAngVel/endDriftAngVel), 2), driftFriction, regFriction);
	        	reLeft.sidewaysFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFriction/frictionRatio, regFriction, rigidbodyAngVel/endDriftAngVel), 2), driftFriction/frictionRatio, regFriction);
	        	reRight.sidewaysFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFriction/frictionRatio, regFriction, rigidbodyAngVel/endDriftAngVel), 2), driftFriction/frictionRatio, regFriction);
	        }
	        else
	        {
	       		frLeft.sidewaysFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFriction, regFriction, -rigidbodyAngVel/endDriftAngVel), 2), driftFriction, regFriction);
	        	frRight.sidewaysFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFriction, regFriction, -rigidbodyAngVel/endDriftAngVel), 2), driftFriction, regFriction);
	        	reLeft.sidewaysFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFriction/frictionRatio, regFriction, -rigidbodyAngVel/endDriftAngVel), 2), driftFriction/frictionRatio, regFriction);
	        	reRight.sidewaysFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFriction/frictionRatio, regFriction, -rigidbodyAngVel/endDriftAngVel), 2), driftFriction/frictionRatio, regFriction);
	        }
	        
	        frLeft.forwardFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFwdFriction, 1, rigidbodyAngVel/endDriftAngVel), 2), driftFwdFriction, 1);
	        frRight.forwardFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFwdFriction, 1, rigidbodyAngVel/endDriftAngVel), 2), driftFwdFriction, 1);
	        reLeft.forwardFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFwdFriction, 1, rigidbodyAngVel/endDriftAngVel), 2), driftFwdFriction, 1);
	        reRight.forwardFriction.stiffness = Mathf.Clamp(Mathf.Pow(Mathf.Lerp(driftFwdFriction, 1, rigidbodyAngVel/endDriftAngVel), 2), driftFwdFriction, 1);
	    	
	    	//driftingTimer += Time.deltaTime;
	    	
	    	steer *= steerMultiplier;
	    	
	    }
	    else
	    {
	    	frLeft.sidewaysFriction.stiffness = regFriction;
	        frRight.sidewaysFriction.stiffness = regFriction;
	        reLeft.sidewaysFriction.stiffness = regFriction;
	        reRight.sidewaysFriction.stiffness = regFriction;
	        
	        frLeft.forwardFriction.stiffness = 1;
	        frRight.forwardFriction.stiffness = 1;
	        reLeft.forwardFriction.stiffness = 1;
	        reRight.forwardFriction.stiffness = 1;
	    }
	    
	//    if(driftingTimer >= driftingTimeout)
	//    {
	//    	drifting = false;
	//    	driftingTimer = 0.0f;
	//    }
	    
	    if(brake > 0.0)
	    {
	        frLeft.brakeTorque=50;
	        frRight.brakeTorque=50;
	        reLeft.brakeTorque=50;
	        reRight.brakeTorque=50;
	
	        reLeft.motorTorque=0.0;
	        reRight.motorTorque=0.0;
	    } 
	    
	    
	    frLeft.steerAngle=steer;
	    frRight.steerAngle=steer;
	    
	    //GUI.Label(Rect(Screen.width - 220, 80, 100, 100), speed.ToString(), style);
	    }
	catch(UnityException)
	{}
}

//function DestroyPlayerInNetwork(){
//	try{
//		Debug.Log("Destroy player in network");
//		Network.RemoveRPCs(Network.player, networkGroup);
//		Network.Destroy(nView.viewID);
//	}
//	catch (UnityException)
//	{}	
//}
//
//function OnApplicationQuit(){
//	DestroyPlayerInNetwork();
//}