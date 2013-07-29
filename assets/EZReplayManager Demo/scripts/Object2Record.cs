using UnityEngine;
using System.Collections;
using EZReplayManagerDemo;

public class Object2Record : MonoBehaviour {

	// Use this for initialization
	void Start () {
		EZReplayManager.mark4Recording(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
