using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectionControl : MonoBehaviour {

	public GameObject injectionVisualizatin;
	
	// Update is called once per frame
	void Update () {

		Quaternion orientation = GvrControllerInput.Orientation;
		transform.rotation = orientation;

		// TODO: Redo in a way of moving the controller on 360 degrees
		// https://developers.google.com/vr/elements/arm-model
		
		if (GvrControllerInput.ClickButtonDown) {
			injectionVisualizatin.SetActive (true);
		}
		if (GvrControllerInput.ClickButtonUp) {
			injectionVisualizatin.SetActive (false);
		}
	}
}
