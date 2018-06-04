using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public enum Mode {
		LevelEditor, Play
	}

	public Mode mode = Mode.LevelEditor;

	public bool dragging = false;
	public Terminal dragTarget;

	public GameObject wirePrefab;

	// Use this for initialization
	void Start () {
		
	}
	

	// Update is called once per frame
	void Update () {
		if (mode==Mode.LevelEditor) {
			if (Input.GetMouseButtonDown(0)) {

				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hit.collider != null) {
					Transform objectHit = hit.transform;
					
					Terminal terminalHit = objectHit.GetComponent<Terminal>();

					// Do something with the object that was hit by the raycast.
					if (terminalHit != null) {
						dragging = true;
						dragTarget = terminalHit;
					}
				}
			}

			if (Input.GetMouseButtonUp(0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hit.collider != null) {
					Transform objectHit = hit.transform;
					
					Terminal terminalHit = objectHit.GetComponent<Terminal>();

					// Do something with the object that was hit by the raycast.
					if (dragTarget != terminalHit) {
						//if legal
						Debug.Log("Good connection");
						Wire newWire = GameObject.Instantiate(wirePrefab).GetComponent<Wire> ();
						newWire.terminalA = dragTarget;
						newWire.terminalB = terminalHit;

					} else {
						//illegal connection
						Debug.Log("Illegal connection");
					}
					dragging = false;
				} else {
					Debug.Log("No connection");
				}
			}
		}
	}
}


//functions
// 1 function per mode, so you don't make one giant update function
// mode: level editor: add elements, move elements
// mode: play mode: click on cards, implement rule choice, implement rule execution