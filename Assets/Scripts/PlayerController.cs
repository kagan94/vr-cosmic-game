using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {
	
//	public GameObject deadScreen;
	public Transform destination;
	public Transform departure;
	public GameObject oxygenValue;
	public GameObject oxygenAlarmScreen;
	public GameObject fireAlarmScreen;
	public float oxygenVolumn = 100;
	
	private GameManager gameManager;
	private Rigidbody rb;
	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private Quaternion initialCameraRotation;
	private float speed = 0;
	private float maxSpeed = 150f;
	private float acceleration = 0.5f;
	private float oxygenConsumingSpeedByAirInjection = 2f;
	private float oxygenConsumingSpeedByBreath = 0.002f;
	private float timestampOfAppBtnDown = 0.0f;
	private float timestampOfClickBtnDown = 0.0f;
	private float timeElapsed = 0f;

	void Start() {
		rb = GetComponent<Rigidbody>();
		initialPosition = transform.position;
		initialRotation = transform.rotation;
		initialCameraRotation = Camera.main.transform.rotation;
		gameManager = (GameManager) Camera.main.GetComponent(typeof(GameManager));
	}

	void Update() {
		// Reset the scene
		if (isAppBtnLongPressed(5)) {
////			deadScreen.SetActive(false);
//			transform.position = initialPosition;
//			rb.velocity = Vector3.zero;
//            
//			float rotationResetSpeed = 50f * Time.deltaTime;
//			 transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, rotationResetSpeed);
//			// transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, rotationResetSpeed);
//			// Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, initialCameraRotation, rotationResetSpeed);
			gameManager.SwitchToInitState ();
		}

		//show oxygen alarm screen
		if (oxygenVolumn < 25 && oxygenVolumn > 0)
			showAlarmScreen (oxygenAlarmScreen, 1f);
		else
			oxygenAlarmScreen.SetActive (false);

		//die for lacking of oxygen
		if (oxygenVolumn <= 0)
			gameManager.SwitchToFailureState ();

		//show fire alarm screen
		if (Vector3.Distance (departure.position, transform.position) < 30)
			showAlarmScreen(fireAlarmScreen, 1f);
		else
			fireAlarmScreen.SetActive (false);
	}

	private void showAlarmScreen(GameObject alarmScreenObject, float interval) {
		timeElapsed += Time.deltaTime;
		if(timeElapsed > interval) {
			if(alarmScreenObject.activeSelf) {
				alarmScreenObject.SetActive (false);
			} else {
				alarmScreenObject.SetActive (true);
			}
			timeElapsed = 0f;
		}
	}

	private bool isAppBtnLongPressed(float lastingInSeconds) {
		if (GvrControllerInput.AppButtonDown) {
			timestampOfAppBtnDown = Time.time;			
		}
		if (GvrControllerInput.AppButtonUp) {
			float timePassed = Time.time - timestampOfAppBtnDown;
			return timePassed >= lastingInSeconds;
		}
		return false;
	}

	private void FixedUpdate() {
		if (gameManager.IsPlayingState()) {
			// Speed up by clicking on TouchPad
			if (GvrControllerInput.ClickButton) {
				speed += acceleration;
				Vector3 targetPos = GvrControllerInput.Orientation * Vector3.forward;
				Vector3 direction = targetPos;
				rb.AddRelativeForce(direction.normalized * speed, ForceMode.Force);
			} else {
				speed = 0;
			}

			// Oxygen consuming calculation and display
			oxygenVolumn -= oxygenConsumingSpeedByBreath; //for breath
			if (GvrControllerInput.ClickButtonDown)
				timestampOfClickBtnDown = Time.time;
			if (GvrControllerInput.ClickButtonUp) {
				float timePassed = Time.time - timestampOfClickBtnDown;
				oxygenVolumn = oxygenVolumn - timePassed * oxygenConsumingSpeedByAirInjection;
			}
			oxygenValue.GetComponent<Text> ().text = oxygenVolumn.ToString("F2");
		}
	}

	private void OnCollisionEnter(Collision other) {
//		if (other.gameObject.CompareTag("Asteroid")) {
//			deadScreen.SetActive(true);
//		}
		gameManager.SwitchToFailureState();
	}

	public float GetDistance() {
		return Vector3.Distance (transform.position, destination.position);
	}
}