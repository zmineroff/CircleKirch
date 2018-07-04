using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public enum Mode {LevelEditor, Play}
	public Mode mode = Mode.LevelEditor;


	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E)) {
			mode = Mode.LevelEditor;
			Debug.Log("Edit mode");
			this.GetComponent<InputPlay>().enabled = false;
			this.GetComponent<InputLevelEditor>().enabled = true;
		} else if (Input.GetKeyDown(KeyCode.P)) {
			mode = Mode.Play;
			Debug.Log("Play mode");
			this.GetComponent<InputPlay>().enabled = true;
			this.GetComponent<InputLevelEditor>().enabled = false;
		}
	}
}

