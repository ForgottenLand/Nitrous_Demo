var timer : float;
var thirdCamera : Camera;

function Start()
{
	timer = 13;
	thirdCamera.enabled = false;
}

function Update()
{
   timer -= Time.deltaTime;
   if(timer < 0){
    camera.enabled = false;
    thirdCamera.enabled = true;
   }
}