﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rating : MonoBehaviour {

    public enum RatingType { Voltage, Resistance, Current }
    public RatingType ratingType;
    public CircuitElement circuitElement;
    public bool known = false;
    public bool isTarget = false;
    public bool isArgument = false;

    public Sprite knownSprite;
    public Sprite unknownSprite;

	public GameObject targetBorder;
    public GameObject argumentBorder;

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = unknownSprite;

        targetBorder = this.transform.Find("TargetBorder").gameObject;
        argumentBorder = this.transform.Find("ArgumentBorder").gameObject;
    }

    // Update is called once per frame
    void Update() {
        if (known) {
            spriteRenderer.sprite = knownSprite;
        } else {
            spriteRenderer.sprite = unknownSprite;
        }

        if (isTarget) {
            targetBorder.SetActive(true);
        } else {
            targetBorder.SetActive(false);
		}

        if (isArgument) {
            argumentBorder.SetActive(true);
        } else {
            argumentBorder.SetActive(false);
        }
    }
}
