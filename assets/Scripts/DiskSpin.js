var wheel:GameObject;
var speed:float;

function Start () {

    speed = 50;
    
}

function OnGUI () {

    if(Input.GetKey("w")){
        wheel.transform.Rotate(Vector3.right * Time.deltaTime * speed * 10, Space.Self);
    } else if (Input.GetKey("s")){
        wheel.transform.Rotate(Vector3.left * Time.deltaTime * speed, Space.Self);
    }
}