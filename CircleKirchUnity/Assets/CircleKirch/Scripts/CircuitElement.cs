using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitElement : MonoBehaviour {

	public List<Terminal> terminals = new List<Terminal> ();

	// Use this for initialization
	void Start () {
		terminals.AddRange(GetComponentsInChildren<Terminal> ());
	}
	
	// Update is called once per frame
	void Update () {

	}
}
