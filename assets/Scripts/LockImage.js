#pragma strict

function Start () {

}

function Update () {
	guiTexture.pixelInset.x = Screen.width - guiTexture.texture.width - 20;
	guiTexture.pixelInset.y = 20;
}