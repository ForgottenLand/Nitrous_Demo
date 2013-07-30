//CarController1.js

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
var style : GUIStyle;

function Awake(){
    
    rigidbody.centerOfMass=Vector3(0,-0.9,0.3);
    pos = transform.position;
    prevPos = pos;
    speed = 0;
}

function Update () {

	
	style.fontStyle = FontStyle.Italic;
    style.normal.textColor = Color.white;
    style.fontSize = 40;
    GUI.Label(Rect(Screen.width - 370, 80, 100, 100),"Speed: ", style);
    style.fontSize = 40;
    style.fontStyle = FontStyle.BoldAndItalic;
    style.normal.textColor = Color.white;
	
	speed=rigidbody.velocity.magnitude * 3.6;
	
	
	
    power=Input.GetAxis("Vertical") * enginePower * Time.deltaTime * 250.0;
    steer=Input.GetAxis("Horizontal") * maxSteer * Mathf.Clamp(speedTurn/speed, 0, 1);
    brake=Input.GetButton("Jump") ? brakePower: 0.0;   
	
	
	rigidbodyAngVel = this.rigidbody.angularVelocity.y;
	
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
    
    GUI.Label(Rect(Screen.width - 220, 80, 100, 100), speed.ToString(), style);
    
    
}
