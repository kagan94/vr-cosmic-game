﻿using System;
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

	public GameObject player;
	public GameObject distanceValue;
	public GameObject timeSpentValue;
	public GameObject oxygenConsummedValue;

	public enum GameState {InitState, PlayingState, FailureState, SuccessState};
	public GameState currentState;

	//audio resources
	public GameObject landerObject;
	private AudioSource landerSmokeAudio;
	public GameObject backgroundMusicObject;
	private AudioSource backgroundMusicAudio;
	public GameObject breathMusicObject;
	private AudioSource breathMusicAudio;


	private PlayerController playerController;
	private float lastStateChange = 0.0f;
	private float maxLastingTimeAfterFailure = 5.0f;
	private float successDistance = 5.0f;
	private float timestampGameStart = 0.0f;
	private float longpressTimeForReset = 3.5f;
	private float timestampOfAppBtnDown = 0.0f;

	private static float scoreTimeCost = 0f;
	private static float scoreOxygenConsummed = 0f;
	private static float highScore = 10000f;
	
	private String sceneName;

	void Start () {
		playerController = (PlayerController) player.GetComponent(typeof(PlayerController));
		
		SetState (GameState.InitState);
		
		timestampGameStart = Time.time;
		landerSmokeAudio = landerObject.GetComponent<AudioSource> ();
		backgroundMusicAudio = backgroundMusicObject.GetComponent<AudioSource> ();
		breathMusicAudio = breathMusicObject.GetComponent<AudioSource> ();
		
		Scene scene = SceneManager.GetActiveScene();
		sceneName = scene.name;
	}

	void Update () {
		
		//Reset the scene
		if (isAppBtnLongPressed(longpressTimeForReset)) {
			SwitchToInitState ();
		}

		//State switching for every state
		switch (currentState) {
		case GameState.InitState:
			enableSounds (false);
			canvasInit.SetActive (true);
			canvasPlaying.SetActive (false);
			canvasFailure.SetActive (false);
			canvasSuccess.SetActive (false);

			//formula for the high score, the smaller the better
			if (scoreTimeCost * scoreOxygenConsummed < highScore) {
				timeSpentValue.GetComponent<Text> ().text = scoreTimeCost.ToString ("F2");
				oxygenConsummedValue.GetComponent<Text> ().text = scoreOxygenConsummed.ToString ("F2");
			}

			break;
		case GameState.PlayingState:
			enableSounds (true);
			canvasInit.SetActive (false);
			canvasPlaying.SetActive (true);
			canvasFailure.SetActive (false);
			canvasSuccess.SetActive (false);

			if (playerController.GetDistance () < successDistance && playerController.speedNumber < 35) {
				SwitchToSuccessState ();
			}
			distanceValue.GetComponent<Text> ().text = playerController.GetDistance ().ToString ("F2");
			//die for lacking of oxygen
			if (playerController.oxygenVolumn <= 0) {
				SwitchToFailureState ();
			}
			break;
		case GameState.FailureState:
			enableSounds(false);
			canvasInit.SetActive (false);
			canvasPlaying.SetActive (false);
			canvasFailure.SetActive (true);
			canvasSuccess.SetActive (false);
			if (GetStateElapsed () >= maxLastingTimeAfterFailure) {
				SwitchToInitState ();
			}
			break;
		case GameState.SuccessState:
			enableSounds (false);
			canvasInit.SetActive (false);
			canvasPlaying.SetActive (false);
			canvasFailure.SetActive (false);
			canvasSuccess.SetActive (true);

			//calculate the time cost


			scoreTimeCost = Time.time - timestampGameStart;
			scoreOxygenConsummed = 100 - playerController.oxygenVolumn;

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

	private void enableSounds(bool enabled) {
		landerSmokeAudio.enabled = enabled;
		backgroundMusicAudio.enabled = enabled;
		breathMusicAudio.enabled = enabled;
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
		SceneManager.LoadScene (sceneName);
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
