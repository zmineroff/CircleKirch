using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCircuitElement : MonoBehaviour {

	public GameObject batteryPrefab;
	public GameObject resitorPrefab;
	public GameObject junctionPrefab;

	public void instantiateBattery () {
		Instantiate(batteryPrefab, transform.position, transform.rotation);
	}

	public void instantiateResistor () {
		Instantiate(resitorPrefab, transform.position, transform.rotation);
	}

	public void instantiateJunction () {
		Instantiate(junctionPrefab, transform.position, transform.rotation);
	}
}
