using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

	public GameObject canvasInit;
	public GameObject canvasPlaying;
	public GameObject canvasFailure;
	public GameObject canvasSuccess;
	public const String sceneName = "Demo";

	public GameObject player;
	public GameObject distanceValue;
	public GameObject timeSpentValue;
	public GameObject oxygenConsummedValue;

	public enum GameState {InitState, PlayingState, FailureState, SuccessState};
	public GameState currentState;

	private PlayerController playerController;
	private float lastStateChange = 0.0f;
	private float maxLastingTimeAfterFailure = 5.0f;
	private float successDistance = 5.0f;
	private float timestampGameStart = 0.0f;

	void Start () {
		playerController = (PlayerController) player.GetComponent(typeof(PlayerController));
		SetState (GameState.InitState);
		timestampGameStart = Time.time;
	}

	void Update () {
		switch (currentState) {
		case GameState.InitState:
			canvasInit.SetActive (true);
			canvasPlaying.SetActive (false);
			canvasFailure.SetActive (false);
			canvasSuccess.SetActive (false);
			break;
		case GameState.PlayingState:
			canvasInit.SetActive (false);
			canvasPlaying.SetActive (true);
			canvasFailure.SetActive (false);
			canvasSuccess.SetActive (false);
			if (playerController.GetDistance () < successDistance) {
				SwitchToSuccessState ();
			}
			distanceValue.GetComponent<Text> ().text = Convert.ToString (playerController.GetDistance ());
			break;
		case GameState.FailureState:
			canvasInit.SetActive (false);
			canvasPlaying.SetActive (false);
			canvasFailure.SetActive (true);
			canvasSuccess.SetActive (false);
			Debug.Log ("GetStateElapsed=" + GetStateElapsed() + ", maxLastingTimeAfterFailure=" + maxLastingTimeAfterFailure);
			if (GetStateElapsed () >= maxLastingTimeAfterFailure) {
				SwitchToInitState ();
			}
			break;
		case GameState.SuccessState:
			canvasInit.SetActive (false);
			canvasPlaying.SetActive (false);
			canvasFailure.SetActive (false);
			canvasSuccess.SetActive (true);

			//calculate the time cost
			float timeCost = Time.time - timestampGameStart;
			timeSpentValue.GetComponent<Text> ().text = Convert.ToString (timeCost);
			oxygenConsummedValue.GetComponent<Text> ().text = Convert.ToString (100 - playerController.oxygenVolumn);

			//Pause the world! This is a trick to pause the world without actually pausing everything...
			Time.timeScale = 0.01f;
			if (GetStateElapsed () > 0.05f) {
				Time.timeScale = 1;
				SwitchToInitState ();
			}
			break;
		}
		// Debug.Log ("Current state: " + currentState);
	}
		
	private void SetState(GameState state) {
		currentState = state;
		lastStateChange = Time.time;
	}

	public void SwitchToPlayingState() {
		SetState(GameState.PlayingState);
	}

	public void SwitchToFailureState() {
		SetState(GameState.FailureState);
	}

	public void SwitchToSuccessState() {
		SetState(GameState.SuccessState);
	}

	public void SwitchToInitState() {
		SceneManager.LoadScene (GameManager.sceneName);
		timestampGameStart = Time.time;
	}

	public void Quit() {
		Application.Quit ();
	}

	private float GetStateElapsed() {
		return Time.time - lastStateChange;
	}

	public bool IsInitState() { return currentState == GameState.InitState; }
	public bool IsPlayingState() { return currentState == GameState.PlayingState; }
	public bool IsFailureState() { return currentState == GameState.FailureState; }
	public bool IsSuccessState() { return currentState == GameState.SuccessState; }
}
