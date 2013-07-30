var splashCamera:Camera;

function Start () {
	camera.enabled = true;
}

function OnGUI () {
	if (transform.Find("MoonCar").GetComponent(Camera).active){
		camera.enabled = false;
		enabled = false;
	}
}