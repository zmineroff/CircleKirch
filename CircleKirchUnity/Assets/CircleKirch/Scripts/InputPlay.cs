using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlay : MonoBehaviour {

	// public Rating clickedRating;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null) {
				Transform objectHit = hit.transform;
				
				Rating ratingHit = objectHit.GetComponent<Rating>();
				if (ratingHit != null) {
					ratingHit.known = !ratingHit.known;
				}
			}
		}
	}
}
