#pragma strict
var timer : float;
var secondCamera : Camera;

function Start () {
	timer = 14;
	secondCamera.enabled = false;
}

function Update () {
	timer -= Time.deltaTime;
    if (timer < 0){
    	camera.enabled = false;
    	secondCamera.enabled = true;
    }
}