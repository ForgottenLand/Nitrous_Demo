#pragma strict

function Start () {

}

var forwardSpeed : float = 20000;
var upSpeed : float = 10000;
var turnSpeed : float = 2;
var spaceDown : boolean = false;
var cDown : boolean = false;

function Update () {
//this is the forward speed that will actually happen
	var forwardMoveAmount = Input.GetAxis("Vertical") * forwardSpeed;
	
	if (Input.GetKeyDown("space")){
		spaceDown = true;
	} else if (Input.GetKeyUp("space")){
		spaceDown = false;
	}
	
	if (Input.GetKeyDown("c")){
		cDown = true;
	} else if (Input.GetKeyUp("c")){
		cDown = false;
	}
	
	
	//acutal turn amount
	var turnAmount = Input.GetAxis("Horizontal") * turnSpeed;
	
	//rotate the vehicle
	transform.Rotate(0,turnAmount,0); 
	
	//push the vehicle forward with a force
	rigidbody.AddRelativeForce(0,0,forwardMoveAmount);
	
	if (spaceDown) {
	rigidbody.AddRelativeForce(0,upSpeed,0);
	}
	if (cDown) {
	rigidbody.AddRelativeForce(0,-(upSpeed/2),0);
	}
}