using UnityEngine;
using System.Collections;

public class LoginButtonEvent : MonoBehaviour {
	
	public GameObject usernameObj, passwordObj, authObj, messageObj;
	
	void OnClick ()
	{
		Authentication authScript = (Authentication) authObj.GetComponent(typeof(Authentication));
		UILabel username = (UILabel) usernameObj.GetComponentInChildren(typeof(UILabel));
		UILabel password = (UILabel) passwordObj.GetComponentInChildren(typeof(UILabel));
		UILabel message = (UILabel) messageObj.GetComponent(typeof(UILabel));
		if(authScript.Authenticate(username.text,password.text)){
			Application.LoadLevel("NGUIPreRace");
		} else {
			message.text = "Sorry, your username or password is incorrect.";
			username.text = "";
			password.text = "";
		}
	}
	
	void OnAwake()
	{
		DontDestroyOnLoad(usernameObj);
		DontDestroyOnLoad(passwordObj);
		DontDestroyOnLoad(authObj);
		DontDestroyOnLoad(messageObj);	
	}
	
}

