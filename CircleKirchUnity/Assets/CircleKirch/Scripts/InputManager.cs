using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public enum Mode {LevelEditor, Play}
	public Mode mode = Mode.LevelEditor;

	public GameObject levelEditorCanvas;
	public GameObject playCanvas;

	// Use this for initialization
	void Start () {
		enterEditMode();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E)) {
			enterEditMode();
		} else if (Input.GetKeyDown(KeyCode.P)) {
			enterPlayMode();
		}
	}

	void enterEditMode () {
		mode = Mode.LevelEditor;
		Debug.Log("Edit mode");
		this.GetComponent<InputPlay>().enabled = false;
		this.GetComponent<InputLevelEditor>().enabled = true;

		levelEditorCanvas.SetActive(true);
		playCanvas.SetActive(false);

		var circuitElements = FindObjectsOfType<CircuitElement>();
		foreach (CircuitElement circuitElement in circuitElements) {
			circuitElement.GetComponent<ItemDragDrop>().enabled=true;
		}
	}

	void enterPlayMode () {
		// can have a check board validity thing here
		// if not valid, stay in edit mode
		// e.g., all terminals only have one wire except junctions

		mode = Mode.Play;
		Debug.Log("Play mode");
		this.GetComponent<InputPlay>().enabled = true;
		this.GetComponent<InputLevelEditor>().enabled = false;
		
		levelEditorCanvas.SetActive(false);
		playCanvas.SetActive(true);
		
		var circuitElements = FindObjectsOfType<CircuitElement>();
		foreach (CircuitElement circuitElement in circuitElements) {
			circuitElement.GetComponent<ItemDragDrop>().enabled=false;
		}
	}
}

