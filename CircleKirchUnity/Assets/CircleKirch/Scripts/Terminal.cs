using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour {

	bool isConnected;
	public List<Wire> wires = new List<Wire>(); //add to it whenever wire gets made
	//list of wires
	//have addWire function

	public CircuitElement circuitElement; //use this to help go through graph

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
