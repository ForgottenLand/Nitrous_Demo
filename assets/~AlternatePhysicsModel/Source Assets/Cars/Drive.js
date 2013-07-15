#pragma strict

function Start () {
    // Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
}

var forwardSpeed : float = 200;
var upSpeed : float = 200;
var turnSpeed : float = 2;
var spaceDown : boolean = false;
var touchSpeed : float = 0.1;

function Update () {
    
	var turnAmount = touchSpeed;
	
	if(Input.GetButtonDown)
	{
		
	}
	
	
        
    
}


//Back up code
/*
#pragma strict

function Start () { 
    
}

var forwardSpeed : float = 200;
var upSpeed : float = 200;
var turnSpeed : float = 2;
var spaceDown : boolean = false;
var touchSpeed : float = 0.1;

function Update () {
    
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
        
        var touchDelta:Vector2 = Input.GetTouch(0).deltaPosition;

        var turnAmount = touchDelta.x * turnSpeed;
    
        transform.Rotate(0,turnAmount,0); 
        
        rigidbody.AddRelativeForce(forwardSpeed,0,0);
        
    }
    
}
*/