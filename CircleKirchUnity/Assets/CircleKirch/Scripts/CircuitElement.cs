using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitElement : MonoBehaviour {

	public enum Voltage {known, unknwon, NA};
	public enum Current {known, unknwon, NA};
	public enum Resistance {known, unknwon, NA};

	public List<Terminal> terminals = new List<Terminal> ();

	// Use this for initialization
	void Start () {
		terminals.AddRange(GetComponentsInChildren<Terminal> ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
