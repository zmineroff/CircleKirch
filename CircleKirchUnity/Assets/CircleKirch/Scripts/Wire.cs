using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {

	public Terminal terminalA;
	public Terminal terminalB;

	private LineRenderer line;

	// Use this for initialization
	void Start () {
		// Add a Line Renderer to the GameObject
		line = this.gameObject.AddComponent<LineRenderer>();

		line.positionCount = 2;
		
		line.startWidth = 0.05F;
		line.endWidth = 0.05F;
		line.material.color = Color.black;

		// Must have a material with a shader "Sprite/*" to use "sortingLayerName" 
		line.material.shader = Shader.Find("Sprites/Default");
		line.sortingLayerName = "Foreground";
	}
	
	// Update is called once per frame
	void Update () {
		// Update position of the two vertices of the LineRenderer
		line.SetPosition(0, terminalA.transform.position);
		line.SetPosition(1, terminalB.transform.position);
	}
}
