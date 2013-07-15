#pragma strict


function Start () {

}

function Update () {

	if (Input.GetKeyDown(KeyCode.V)) {
		
		GameObject.Find("cameraBehind").GetComponent("Camera").active = true;
  	    GameObject.Find("Main Camera").GetComponent("ThirdPersonCamera").active = false;
	}
}