using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyControl : MonoBehaviour {

	public Transform testBall;

	void Start () {
		
	}

	/// <summary>
	/// Turn the astronaunt according to the forward direction of the camera
	/// </summary>
	void Update () {
		var camForward = Camera.main.transform.forward;
		var camForwardXZPlane = new Vector3 (camForward.x, 0, camForward.z);
		var distance = Mathf.Pow (camForward.x, 2f) + Mathf.Pow (camForward.z, 2f);
		if (distance > 0.05f) {
			var forwardPosition = transform.position + camForwardXZPlane;
			testBall.position = forwardPosition;
			this.transform.LookAt (forwardPosition, transform.up);
		}
	}
}
