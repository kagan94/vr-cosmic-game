using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundTarget : MonoBehaviour {
    public GameObject target;
    public float speed = 215.5f;
    public bool oppositeDirection;
    
    private Vector3 randomDirection;
    private int direction;
    
    // Use this for initialization
    void Start() {
        randomDirection = new Vector3(-Random.value, Random.value, Random.value);
        speed = 35.5f; // TODO: Remove later, after speed adjustment
        direction = oppositeDirection ? -1 : 1;
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(randomDirection); // rotate object itself
        transform.RotateAround(target.transform.position, Vector3.one, direction * Time.deltaTime * speed);
    }
}