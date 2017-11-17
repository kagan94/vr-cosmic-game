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
	public GameObject speedValue;
	public GameObject positionX;
	public GameObject positionY;
	public GameObject positionZ;
	public float oxygenVolumn = 100f;
	
	private GameManager gameManager;
	private Rigidbody rb;
	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private Quaternion initialCameraRotation;
	private float speed = 0f;
	private Vector3 speedAccumulated = new Vector3(0, 0, 0);
	public float speedNumber = 0f;
	private float acceleration = 0.5f;
	private float oxygenConsumingSpeedByAirInjection = 3f;
	private float oxygenConsumingSpeedByBreath = 0.002f;
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
		//show oxygen alarm screen
		if (gameManager.IsPlayingState ()) {
			if (oxygenVolumn < 25 && oxygenVolumn > 0)
				showAlarmScreen (oxygenAlarmScreen, 1f);
			else
				oxygenAlarmScreen.SetActive (false);

			//show fire alarm screen
			if (Vector3.Distance (departure.position, transform.position) < 30)
				showAlarmScreen(fireAlarmScreen, 1f);
			else
				fireAlarmScreen.SetActive (false);
		}

		speedNumber = speedAccumulated.magnitude / 20;
		speedValue.GetComponent<Text> ().text = speedNumber.ToString("F2");
		positionX.GetComponent<Text> ().text = transform.position.x.ToString("F2");
		positionY.GetComponent<Text> ().text = transform.position.y.ToString("F2");
		positionZ.GetComponent<Text> ().text = transform.position.z.ToString("F2");
	}

	private void FixedUpdate() {
		
		if (gameManager.IsPlayingState()) {
			// Speed up by clicking on TouchPad
			if (GvrControllerInput.ClickButton) {
				speed += acceleration;

				Vector3 targetPos = GvrControllerInput.Orientation * Vector3.forward;
				Vector3 direction = targetPos;
				rb.AddRelativeForce(direction.normalized * speed, ForceMode.Force);

				speedAccumulated = direction.normalized * speed + speedAccumulated;
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
		if (!other.gameObject.CompareTag ("Asteroid")) {
			Debug.Log (">>>>>>>>>>>>>collision!>>>>>>>>>>>>>>>>>" + other.gameObject.tag + ", " + other.gameObject.name);
			gameManager.SwitchToFailureState();	
		}

	}

	public float GetDistance() {
		return Vector3.Distance (transform.position, destination.position);
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
}