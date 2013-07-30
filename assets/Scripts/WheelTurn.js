var rotate:float;
var wheeltype:String;
var wheel:GameObject;

function Start () {

    rotate = 0;
    wheeltype = wheel.name;
    
}

function OnGUI () {
    
    if(Input.GetKey("a")){
        rotate = -20;
    } else if(Input.GetKey("d")){
        rotate = 20;
    }
    else
    {
        rotate = 0;
    }
    
    wheel.transform.localRotation = Quaternion.Euler (0, rotate, 0);
    
}