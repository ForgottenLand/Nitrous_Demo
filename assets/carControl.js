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

        var turnAmount = touchDelta.x;
    
        transform.Rotate(0,turnAmount,0); 
        
        rigidbody.AddRelativeForce(forwardSpeed,0,0);
    }
        
}