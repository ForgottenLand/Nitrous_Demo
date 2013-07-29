using UnityEngine;
using System.Collections;
using EZReplayManagerDemo;

public class Game : MonoBehaviour {
	private bool activated=true;
	
	void Awake() {
		
		
	}

	// Use this for initialization
	void Start () {
		EZReplayManager.record();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		GameObject middleHeli = GameObject.Find("Heli3");
		
		if (middleHeli != null && activated) {
			if (middleHeli.transform.position.x >180f) {
				activated = false;
				EZReplayManager.stop(true);
				EZReplayManager.play(0);
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