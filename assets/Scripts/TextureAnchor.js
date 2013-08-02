#pragma strict
var yOffSet : int;
var xOffSet : int;
var xPercent : float;
var yPercent : float;
function Start () {
	yOffSet = 10;
	xOffSet = 10;
	xPercent = 0.5;
	yPercent = 0.5;
	if(guiTexture.texture.name == "RightChevron"){
		guiTexture.pixelInset.x = Screen.width - guiTexture.pixelInset.width - xOffSet;
		guiTexture.pixelInset.y = (Screen.height - guiTexture.pixelInset.height) * yPercent;
	} else if(guiTexture.texture.name == "LeftChevron"){
		guiTexture.pixelInset.x = xOffSet;
		guiTexture.pixelInset.y = (Screen.height - guiTexture.pixelInset.height) * yPercent;
	} else if(guiTexture.texture.name == "Lock"){
		guiTexture.pixelInset.x = (Screen.width - guiTexture.pixelInset.width) * xPercent;
		guiTexture.pixelInset.y = (Screen.height - guiTexture.pixelInset.height) * yPercent;
	} else if(guiTexture.texture.name == "openLock"){
		guiTexture.pixelInset.x = (Screen.width - guiTexture.pixelInset.width) * xPercent;
		guiTexture.pixelInset.y = Screen.height - guiTexture.pixelInset.height - yOffSet;
	} else if(guiTexture.texture.name == "select"){
		guiTexture.pixelInset.x = Screen.width - guiTexture.pixelInset.width - xOffSet;
		guiTexture.pixelInset.y = yOffSet;
	}
}

function Update () {

}