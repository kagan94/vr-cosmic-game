using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public GameObject DeadScreen;

	private float speed = 0;
	private float minSpeed = 0;
	private float maxSpeed = 100f;
	private float acceleration = 0.5f;
	private float deceleration = 0.1f;
	private Rigidbody rb;
	private Quaternion lastTouchPadOrientation;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update() {
		if (GvrControllerInput.AppButton) {
			DeadScreen.SetActive(false);
		}
	}

	private void FixedUpdate() {
		// Speed up by clicking on TouchPad
		if (GvrControllerInput.ClickButton) {
			speed += acceleration;
			Vector3 targetPos = GvrControllerInput.Orientation * Vector3.forward;
			Vector3 direction = targetPos;
			rb.AddRelativeForce(direction.normalized * speed, ForceMode.Force);
		} else {
			speed = 0;
		}
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Asteroid")) {
			DeadScreen.SetActive(true);
		}
	}
}