using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitElement : MonoBehaviour {

	public List<Terminal> terminals = new List<Terminal> ();
	public Rating voltage;
	public Rating current;
	public Rating resistance;

	// Use this for initialization
	void Start () {
		// add references to terminal
		terminals.AddRange(GetComponentsInChildren<Terminal> ());
		foreach(Terminal t in terminals) {
			t.circuitElement = this;
		}

		// add references to ratings (probably a better way to do this)
		var ratings = transform.GetComponentsInChildren<Rating> ();
		foreach(Rating r in ratings) {
			r.circuitElement = this;
			
			switch (r.ratingType)
			{
				case Rating.RatingType.Voltage:
					voltage = r;
					break;
				case Rating.RatingType.Current:
					current = r;
					break;
				case Rating.RatingType.Resistance:
					resistance = r;
					break;
				default:
					Debug.Log("bad rating card");
					break;   
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
