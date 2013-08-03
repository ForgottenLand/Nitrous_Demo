// The target we are following
var target : Transform;
// The distance in the x-z plane to the target
var distance : float;
// the height we want the camera to be above the target
var height : float;
// How much we 
var heightDamping : float;
var rotationDamping : float;
var mc : PgtAdminControl;

function OnLoaded() {
    distance = 6;
    height = 1.2;
    heightDamping = 2;
    rotationDamping = 3;
    
}

function LateUpdate () {
	// Early out if we don't have a target
	try{
		target = mc.Player.transform;
		transform.position.y = target.position.y + 5.8;
		transform.position = target.transform.position;
		transform.rotation = target.transform.rotation;
	}catch(UnityException)
	{}
	
//    if(mc.selectNumber == 3)
//    height = 2;
//    distance = 8;
    
	if (!target)
		return;
	// Calculate the current rotation angles
	var wantedRotationAngle = target.eulerAngles.y;
	var wantedHeight = target.position.y + height;
		
	var currentRotationAngle = transform.eulerAngles.y;
	var currentHeight = transform.position.y;
	
	// Damp the rotation around the y-axis
	currentRotationAngle = Mathf.LerpAngle (currentRotationAngle , wantedRotationAngle , rotationDamping * Time.deltaTime);

	// Damp the height
	currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

	// Convert the angle into a rotation
	var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
	
	// Set the position of the camera on the x-z plane to:
	// distance meters behind the target
	transform.position = target.position;
	transform.position -= currentRotation * Vector3.forward * distance;

	// Set the height of the camera
	transform.position.y = currentHeight + 0.05;
	
	// Always look at the target
	//transform.LookAt(target);

}