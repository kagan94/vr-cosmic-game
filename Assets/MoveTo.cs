using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour {

	public Transform goal;
	
	// Use this for initialization
	void Start () {
		
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		if (!agent)
			agent.destination = goal.position; 
		Debug.Log(agent);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
