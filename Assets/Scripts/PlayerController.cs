using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public GameObject deadScreen;

	private float speed = 0;
	private float maxSpeed = 150f;
	private float acceleration = 0.5f;
	private Rigidbody rb;
	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private Quaternion initialCameraRotation;

	void Start() {
		rb = GetComponent<Rigidbody>();
		initialPosition = transform.position;
		initialRotation = transform.rotation;
		initialCameraRotation = Camera.main.transform.rotation;
	}

	// Update is called once per frame
	void Update() {
		// Reset the Player position
		if (GvrControllerInput.AppButton) {
			deadScreen.SetActive(false);
			transform.position = initialPosition;
			rb.velocity = Vector3.zero;
            
			float rotationResetSpeed = 50f * Time.deltaTime;
			 transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, rotationResetSpeed);
			// transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, rotationResetSpeed);
			// Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, initialCameraRotation, rotationResetSpeed);
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
			deadScreen.SetActive(true);
		}
	}
}