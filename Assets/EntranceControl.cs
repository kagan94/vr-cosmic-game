using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceControl : MonoBehaviour {

	public Light beacon;
	public float interval;
	private float timeElapsed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
		if(timeElapsed > interval) {
			if(beacon.enabled == true) {
				beacon.enabled = false;
			} else {
				beacon.enabled = true;
			}
			timeElapsed = 0f;
		}
	}
}
