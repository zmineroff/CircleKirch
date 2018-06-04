using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {

	public Terminal terminalA;
	public Terminal terminalB;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//where drawing happens
		Debug.DrawLine(terminalA.transform.position, terminalB.transform.position);
	}
}
