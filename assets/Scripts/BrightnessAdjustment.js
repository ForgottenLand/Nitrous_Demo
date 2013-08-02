#pragma strict

function Start () {
	if(Application.platform == RuntimePlatform.Android){
		light.intensity = 0.5;
	} else if(Application.platform == RuntimePlatform.WindowsEditor){
		light.intensity = 0.3;
	}
}

function Update () {

}