#pragma strict

var StateNumber : int;

var signInBoxWidth : int;
var signInBoxHeight : int;
var registerBoxWidth : int;
var registerBoxHeight : int;

public var UserName : String;

public var Email : String;

public var Password : String;
public var ReEnter : String;

public var FirstName : String;

public var LastName : String;

public var CardHolderFirstName : String;

public var CardHolderLastName : String;

public var Address1 : String;

public var Address2 : String;

public var City : String;

public var Province : String;

public var Country : String;

public var Postal : String;

public var Phone : String;

public var CreditNumber : String;

public var CVV : String;

public var ExpireMonth : String;

public var ExpireYear : String;

var TextboxHeight : int;

var guiStyle : GUIStyle;

var gmObj : GameObject;
var auth : Authentication;

var buttonObj : GameObject;

var LabelWidth : int;

var mask : char;

var ErrorNumber : int;

function Start() {
	signInBoxWidth = 300;
	signInBoxHeight = 120;
	registerBoxWidth = 300;
	registerBoxHeight = 200;	
	TextboxHeight = 22;	
	StateNumber = 1;
	
	ErrorNumber = 0;
	
	auth = gmObj.GetComponent(Authentication);
	mask = System.Convert.ToChar('*');
}

function OnGUI () {
	//Scale screen properly
//	var screenScale: float = Screen.width / 320.0;
//    var scaledMatrix: Matrix4x4 = Matrix4x4.identity.Scale(Vector3(screenScale,screenScale,screenScale));
//    GUI.matrix = scaledMatrix;
    
    var addHeight : int;
    addHeight = -100;
    
    if(Application.platform == RuntimePlatform.Android){
    	print("Android Platform");
    	addHeight = 85;
    }
    
    GUI.Box(Rect (10, 400 + addHeight, 300, 70), "Message window");
    
	switch (StateNumber){
		case 1:
			StateSignIn();
		break;
		case 2:
			StateReg ();
		break;
		case 3:
			StateLoggedIn();
		break;
	}
	
	switch (ErrorNumber){
		case 0:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "Empty");
		break;
		case 1:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "You are authenticated, username is found and");
			GUI.Label (Rect (20, 440 + addHeight, 300, 22), "password matches    Loading..");
		break;
		case 2:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "Password is incorrect or username does not");
			GUI.Label (Rect (20, 440 + addHeight, 300, 22), "exist");
		break;
		case 3:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "Username should not be null");
		break;
		case 4:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "Password should not be null");
		break;
		case 5:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "Password should be at least 6 digits long");
			GUI.Label (Rect (20, 440 + addHeight, 300, 22), "");
		break;
		case 6:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "Re-entered password does not match");
		break;
		case 7:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "Email should not be null");
		break;
		case 8:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "Email entered is not valid");
		break;
		case 9:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "First name should not be null");
		break;
		case 10:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "Last name should not be null");
		break;
		case 11:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "Your username is added to the database");
		break;
		case 12:
			GUI.Label (Rect (20, 420 + addHeight, 300, 22), "Username already exists, please choose a");
			GUI.Label (Rect (20, 440 + addHeight, 300, 22), "different username");
		break;
	}
}

function StateSignIn () {
//	if( Screen.width - signInBoxWidth - 10 >= 10 ) {
//		LabelWidth = 60;
//		GUI.Box(Rect (10,10,signInBoxWidth,signInBoxHeight), "Sign in window");
//		
//		GUI.Label (Rect (20, 40, LabelWidth, 22), "Username");
//		if(UserName == ""){
//			GUI.SetNextControlName("UserName");
//			GUI.FocusControl("UserName");
//		}
//		UserName = GUI.TextField (Rect (LabelWidth + 30, 40, signInBoxWidth - LabelWidth - 30, TextboxHeight),UserName,40);
//	
//		GUI.Label (Rect (20, 70, LabelWidth, 22), "Password");
//
//		Password = GUI.PasswordField (Rect (LabelWidth + 30, 70, signInBoxWidth - LabelWidth - 30, TextboxHeight),Password,mask,40);
//	
//		if( GUI.Button (Rect (20, 100, (signInBoxWidth - 40) / 2, 22), "Sign in") ) {
//			//Call StateRead
//			ReadSignIn();
//		}
//		
//		if( GUI.Button (Rect (20 + (signInBoxWidth - 40) / 2 + 20, 100, (signInBoxWidth - 40) / 2, 22), "Register") ) {
//			//Call StateReg
//			StateNumber = 2;
//		}
//	}	
}

function StateReg () {
//	if( Screen.width - signInBoxWidth - 10 >= 10 ) {
//		LabelWidth = 60;
//		
//		GUI.Box(Rect (10, 10, registerBoxWidth, 250), "Registration window");
//		
//		GUI.Label (Rect (20, 40, LabelWidth, 22), "Username");
//		UserName = GUI.TextField (Rect (LabelWidth + 30, 40, registerBoxWidth - LabelWidth - 30, TextboxHeight),UserName,40);
//		
//		GUI.Label (Rect (20, 70, LabelWidth, 22), "Password");
//		Password = GUI.PasswordField (Rect (LabelWidth + 30, 70, registerBoxWidth - LabelWidth - 30, TextboxHeight),Password,mask,40);
//		
//		GUI.Label (Rect (20, 100, LabelWidth, 22), "Re-enter");
//		ReEnter = GUI.PasswordField (Rect (LabelWidth + 30, 100, registerBoxWidth - LabelWidth - 30, TextboxHeight),ReEnter,mask,40);
//		
//		GUI.Label (Rect (20, 130, LabelWidth, 22), "Email");
//		Email = GUI.TextField (Rect (LabelWidth + 30, 130, registerBoxWidth - LabelWidth - 30, TextboxHeight),Email,40);
//		
//		GUI.Label (Rect (20, 160, LabelWidth, 22), "FirstName");
//		FirstName = GUI.TextField (Rect (LabelWidth + 30, 160, registerBoxWidth - LabelWidth - 30, TextboxHeight),FirstName,40);
//		
//		GUI.Label (Rect (20, 190, LabelWidth, 22), "LastName");
//		LastName = GUI.TextField (Rect (LabelWidth + 30, 190, registerBoxWidth - LabelWidth - 30, TextboxHeight),LastName,40);
//			
//		if( GUI.Button (Rect (20, 230, 130, 22), "Back to sign in") ) {
//			//Call StateSignIn
//			StateNumber = 1;
//		}
//			
//		if( GUI.Button (Rect ( signInBoxWidth / 2 + 20, 230, 130, 22), "Confirm") ) {
//			//Call StateReg
//			ReadRegister();
//		}
//	}
}

function ReadSignIn() {
	if(auth.Authenticate(UserName, Md5Sum(Password))){
		//User is authenticated
		ErrorNumber = 1;
		StateNumber = 3;
	}
	else {
		//User is not authenticated
		ErrorNumber = 2;
	}
}

function ReadRegister() {
	
	if(UserName == ""){
		ErrorNumber = 3;
	}
	else if(Password == ""){
		ErrorNumber = 4;
	}
	else if(Password.Length < 6){
		ErrorNumber = 5;
	}
	else if(!ReEnter.Equals(Password)){
		ErrorNumber = 6;
	}
	else if(Email == ""){
		ErrorNumber = 7;
	}
	else if(!Email.Contains("@") || !Email.Contains(".com")) {
		ErrorNumber = 8;
	}
	else if(FirstName == ""){
		ErrorNumber = 9;
	}
	else if(LastName == ""){
		ErrorNumber = 10;
	}
	else{
		if(auth.NewUser(UserName, Email, Md5Sum(Password), FirstName, LastName)){
			//New user is registered
			ErrorNumber = 11;
			StateNumber = 3;
		}
		else {
			//Not eligible
			ErrorNumber = 12;
		}
	}
	
}

function StateLoggedIn() {
	
	Application.LoadLevel("Scene1");
}

static function Md5Sum(strToEncrypt: String)
{
	var encoding = System.Text.UTF8Encoding();
	var bytes = encoding.GetBytes(strToEncrypt);
 
	// encrypt bytes
	var md5 = System.Security.Cryptography.MD5CryptoServiceProvider();
	var hashBytes:byte[] = md5.ComputeHash(bytes);
 
	// Convert the encrypted bytes back to a string (base 16)
	var hashString = "";
 
	for (var i = 0; i < hashBytes.Length; i++)
	{
		hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, "0"[0]);
	}
 
	return hashString.PadLeft(32, "0"[0]);
}