using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileLauncher : Puffy_MultiSpawner {
	
	//private List<HomingMissile> missiles = new List<HomingMissile>();
	private Transform missilesContainer;
	
	public Transform target; // object to follow

	public int launchCount = 1;
	public float missileAccuracy = 0.5f;
	public float missileCrazyness = 0.2f;
	
	
	void Start(){
		// missiles will be parented to this gameObject, to prevent long list of items in the editor hierachy view
		missilesContainer = new GameObject("Missiles Container of ["+name+"]").transform;
	}
	
	void Update () {
		
		// synchronize freeze state with puffy smoke freeze
		HomingMissile.globalFreeze = Puffy_Emitter.globalFreeze;
		
		if(Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height - 70){
			// get worldspace mouse pointer position
			Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 5));
			
			// launch missiles
			Fire(launchCount,pos);
		}
	}
	
	public void Fire(int count,Vector3 startPosition){
		HomingMissile missileScript = null;
		Vector3 position;
		
		for(int m = 0; m < count; m++){

			position = startPosition;
			// randomly offset the missile start position if more than one missile are launched at the same time
			if(count > 1) position += Random.onUnitSphere;

			// get the next available missile (or instantiate a new one if none is free to reuse)
			Puffy_ParticleSpawner p = CreateSpawner(position,Camera.main.transform.forward);
			p.transform.parent = missilesContainer;

			missileScript = p.GetComponent<HomingMissile>();

			if(missileScript == null) missileScript = p.gameObject.AddComponent<HomingMissile>();

			// update turning and craziness values for this missile
			missileScript.angularThreshold = missileAccuracy;
			missileScript.craziness = missileCrazyness;
			
			// spawn the missile and initialize it with its new start position, direction, target and speed
			missileScript.Spawn(position, Camera.main.transform.forward , target, missileScript.speed);
		}
	}
}
