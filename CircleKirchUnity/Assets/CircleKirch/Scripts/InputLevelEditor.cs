using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLevelEditor : MonoBehaviour {

    public bool draggingWire = false;
    public Terminal dragTarget = null;
    public GameObject wirePrefab;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null) {
                Transform objectHit = hit.transform;

                // Clicked terminal
                Terminal terminalHit = objectHit.GetComponent<Terminal>();
                if (terminalHit != null) {
                    bool eltIsJunction = terminalHit.circuitElement.circuitElementType == CircuitElement.CircuitElementType.Junction;
                    if (eltIsJunction) {
                        dragTarget = terminalHit;
                        draggingWire = true;
                    } else if (terminalHit.wires.Count == 0) {
                        dragTarget = terminalHit;
                        draggingWire = true;
                    } else {
                        Debug.Log("Non-junction terminals can only have one wire");
                    }
                }

                // Clicked rating
                Rating ratingHit = objectHit.GetComponent<Rating>();
                if (ratingHit != null) {
                    ratingHit.known = !ratingHit.known;
                }

                // Clicked wire
                Wire wireHit = objectHit.GetComponent<Wire>();
                if (wireHit != null) {
                    Debug.Log("Wire clicked");
                }
            }
        }

        // Dragging wire
        if (draggingWire && Input.GetMouseButtonUp(0)) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null) {
                Transform objectHit = hit.transform;

                // Landed on terminal
                Terminal terminalHit = objectHit.GetComponent<Terminal>();
                if (terminalHit != null) {
                    if (dragTarget.transform.parent != terminalHit.transform.parent) {
                        bool eltIsJunction = terminalHit.circuitElement.circuitElementType == CircuitElement.CircuitElementType.Junction;
                        if (eltIsJunction) {
                            Debug.Log("Good connection");
                            Wire newWire = GameObject.Instantiate(wirePrefab).GetComponent<Wire>();
                            newWire.terminalA = dragTarget;
                            newWire.terminalB = terminalHit;
                            dragTarget.wires.Add(newWire);
                            terminalHit.wires.Add(newWire);
                        } else if (terminalHit.wires.Count == 0) {
                            Debug.Log("Good connection");
                            Wire newWire = GameObject.Instantiate(wirePrefab).GetComponent<Wire>();
                            newWire.terminalA = dragTarget;
                            newWire.terminalB = terminalHit;
                            dragTarget.wires.Add(newWire);
                            terminalHit.wires.Add(newWire);
                        } else {
                            Debug.Log("Non-junction terminals can only have one wire");
                        }
                    } else {
                        Debug.Log("Illegal connection");
                    }
                } else {
                    Debug.Log("No connection");
                }
				
                draggingWire = false;
                dragTarget = null;
            }
        }
    }
}
