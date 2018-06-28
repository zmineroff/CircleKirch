using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLevelEditor : MonoBehaviour {

	public bool draggingWire = false;
	public Terminal dragTarget;
	public GameObject wirePrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null) {
				Transform objectHit = hit.transform;
				
				// Clicked terminal
				Terminal terminalHit = objectHit.GetComponent<Terminal>();
				if (terminalHit != null) {
					draggingWire = true;
					dragTarget = terminalHit;
				}

				// Clicked rating
				Rating ratingHit = objectHit.GetComponent<Rating>();
				if (ratingHit != null) {
					ratingHit.known = !ratingHit.known;
				}
			}
		}

		// Dragging wire
		if (draggingWire && Input.GetMouseButtonUp(0)) {
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null) {
				Transform objectHit = hit.transform;

				Terminal terminalHit = objectHit.GetComponent<Terminal>();

				// Do something with the object that was hit by the raycast.
				if (dragTarget.transform.parent != terminalHit.transform.parent) {
					// legal connection
					Debug.Log("Good connection");
					Wire newWire = GameObject.Instantiate(wirePrefab).GetComponent<Wire> ();
					newWire.terminalA = dragTarget;
					newWire.terminalB = terminalHit;

				} else {
					// illegal connection
					Debug.Log("Illegal connection");
				}
			} else {
				// no connection
				Debug.Log("No connection");
			}
			draggingWire = false;
		}
	}
}
