using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {

	public Terminal terminalA;
	public Terminal terminalB;

    public float lineWidth;

    public bool isArgument = false;

	private LineRenderer line;
    private BoxCollider2D lineCollider;

	// Use this for initialization
	void Start () {
		// Add a Line Renderer to the GameObject
		line = this.gameObject.AddComponent<LineRenderer>();

		line.positionCount = 2;

        lineWidth = 0.1F;
		line.startWidth = lineWidth;
		line.endWidth = lineWidth;
		line.material.color = Color.black;

		// Must have a material with a shader "Sprite/*" to use "sortingLayerName" 
		line.material.shader = Shader.Find("Sprites/Default");
		line.sortingLayerName = "Foreground";


        //Collider
        lineCollider = this.gameObject.AddComponent<BoxCollider2D>();
		lineCollider.offset = new Vector2(0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		// Update position of the two vertices of the LineRenderer
		line.SetPosition(0, terminalA.transform.position);
		line.SetPosition(1, terminalB.transform.position);


		//Update collider
        float lineLength = Vector2.Distance(terminalA.transform.position, terminalB.transform.position);
        lineCollider.size = new Vector2(lineLength, lineWidth*6);
        Vector2 midPoint = (terminalA.transform.position + terminalB.transform.position) / 2;
        lineCollider.transform.position = midPoint;

        float angle = Mathf.Atan2((terminalB.transform.position.y - terminalA.transform.position.y), (terminalB.transform.position.x - terminalA.transform.position.x));
        angle *= Mathf.Rad2Deg;
        // angle *= -1;
        lineCollider.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

		if (this.isArgument) {
			line.material.color = Color.blue;
		} else {
            line.material.color = Color.black;
		}
	}
}
