using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rating : MonoBehaviour {

	public bool known = false;

	public Sprite knownSprite;
	public Sprite unknownSprite;

	private SpriteRenderer spriteRenderer; 

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = unknownSprite;
	}
	
	// Update is called once per frame
	void Update () {
		if (known) {
			spriteRenderer.sprite = knownSprite;
		} else {
			spriteRenderer.sprite = unknownSprite;
		}
	}
}
