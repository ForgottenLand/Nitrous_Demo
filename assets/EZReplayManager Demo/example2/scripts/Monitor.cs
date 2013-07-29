using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EZReplayManagerDemo;

public class Monitor : MonoBehaviour {
	public GameObject stone;
	public int wallMaxX;
	public int wallMaxY;
	public Transform player;
	//is anyone able to move and play?
	private bool gameActive;
	
	public bool isActive() {
		return gameActive;
	}
	
	public void setActive(bool newValue) {
		gameActive = newValue;
	}
	
	void Awake () {
		createWall();
	}

	// Use this for initialization
	void Start () {		
		Application.runInBackground = false;
		gameActive = true;
		StartCoroutine(startRecording());
		//Screen.showCursor = false; 
		
	}
	
	private IEnumerator startRecording() {
		yield return new WaitForSeconds(1f);
		EZReplayManager.record();
	}
	
	private void createWall()
	{
		GameObject goStones = new GameObject("Stones");
		for (int y = 0; y < wallMaxY; y++) 
		{
			for (int x = 0; x < wallMaxX; x++) 
			 {
				 if (y == wallMaxY-1 && (x == wallMaxX-1))
					 continue;
				 
				 float addition = 0f;
				 if (y % 2 == 0)
					addition = stone.transform.localScale.x / 2;				 
				 
				GameObject thisStone=(GameObject)Instantiate (stone,new Vector3 ((transform.position.x-x*stone.transform.localScale.x)+addition, y*stone.transform.localScale.y, transform.position.z), Quaternion.identity);
				thisStone.transform.parent = goStones.transform;
			}
		}
	}

	void  OnGUI (){
		if (EZReplayManager.getCurrentMode() == EZReplayManager.MODE_LIVE)
			if (GUI.Button ( new Rect(20,250,130,20), "New Game")) {
				//EZReplayManager.stop ();
				//EZReplayManager.clearObjects();
				Application.LoadLevel(0);
			}
	}
	
}
