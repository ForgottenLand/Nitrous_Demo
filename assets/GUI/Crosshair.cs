using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour
{
	public Texture crosshairTexture;
	Rect position;
    
	// Use this for initialization
	void Start ()
	{
		position = new  Rect ((Screen.width - crosshairTexture.width) / 2, (Screen.height - crosshairTexture.height) / 2, crosshairTexture.width, crosshairTexture.height);
	
	}
	
	// Update is called once per frame
	void OnGUI() 
	{
		GUI.DrawTexture (position, crosshairTexture);
	}
	
	
}
