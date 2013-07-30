#pragma strict
var audioSource : AudioSource;
var timer : float;

function Start () {
	timer = 13;
}

function Update () {
	timer -= Time.deltaTime;
	if (timer < 0){
		audioSource.volume *= 100;
	}
}