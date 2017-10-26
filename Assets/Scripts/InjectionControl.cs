using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectionControl : MonoBehaviour {

	public GameObject injectionVisualizatin;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Quaternion orientation = GvrControllerInput.Orientation;
		transform.rotation = orientation;


		if (GvrControllerInput.ClickButtonDown) {
			injectionVisualizatin.SetActive (true);
		}
		if (GvrControllerInput.ClickButtonUp) {
			injectionVisualizatin.SetActive (false);
		}
	}
}
