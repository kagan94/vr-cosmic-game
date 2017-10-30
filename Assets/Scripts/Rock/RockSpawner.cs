using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockSpawner : MonoBehaviour {
    [Tooltip("Rock prefab that will be generated")]
    public GameObject rockPrefab;

    [Tooltip("Distance from player where new rock will be generated")]
    public int distanceToGeneratedRock = 2;

    [Tooltip("Max Distance from player where new rock will be generated")]
    public int maxDistance = 1;

    [Tooltip("Generate with random position shift")]
    public bool randomPosShift = true;

    [Tooltip("Max Angle between initial and current player pos from Left side (if exceeded, new rock won't be generated)")]
    [Range(0, 180)]
    public int maxLeftAngle = 30;

    [Tooltip("Max Angle between initial and current player pos from Right side (if exceeded, new rock won't be generated)")]
    [Range(0, 180)]
    public int maxRightAngle = 30;

    public int generateEveryNSeconds = 1;
    public bool debug = true;
    private float rockSpeed = 25;

    private float timeElapsed;
    private Vector3 initialCameraPos;

    void Start() {
        initialCameraPos = Camera.main.transform.position;
        InstantiateRock();
    }

    // Update is called once per frame
    void Update() {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= generateEveryNSeconds) {
            InstantiateRock();
            timeElapsed = 0;
        }
    }

    void InstantiateRock() {
        Vector3 startPos = transform.position;
        if (debug)
            Debug.DrawLine(startPos, initialCameraPos, Color.green, Mathf.Infinity);
        
        Vector3 targetPos = Camera.main.transform.position; // pos of player head
        if (randomPosShift) {
            targetPos += new Vector3(Random.Range(-1.5f, 1.5f), 0, 0);
        }

        Vector3 initialDirectionVector = initialCameraPos - startPos;
        Vector3 currentDirectionVector = targetPos - startPos;
        float angle = AngleBetweenVectors(initialDirectionVector, currentDirectionVector);
        if (debug)
            Debug.Log("Angle between initial and current player's position: " + angle);

        float distanceToPlayer = Vector3.Distance(targetPos, startPos);
        if (debug)
            Debug.Log("Distance to player from RockSpawner: " + distanceToPlayer);

        if (distanceToPlayer >= distanceToGeneratedRock && distanceToPlayer <= maxDistance && AngleNotExceeded(angle)) {
            // Calculate relative distance [0.0; 1.0] (from Player vector) where new Rock will be generated.
            float relativePos = distanceToGeneratedRock / distanceToPlayer;
            if (debug)
                Debug.Log("Relative distance where new rock will be generated [0;1]: " + relativePos);

            Vector3 newRockPos = Vector3.Lerp(targetPos, startPos, relativePos);
            Vector3 directionLine = targetPos - newRockPos;

            GameObject newRock = Instantiate(rockPrefab, newRockPos, transform.rotation);
            Rigidbody newRockRigidbody = newRock.GetComponent<Rigidbody>();
            newRockRigidbody.AddForce(directionLine.normalized * rockSpeed);

            newRock.GetComponent<RockSelfDestory>().SetInitialRockPos(newRockPos);
            
            if (debug)
                Debug.DrawLine(startPos, targetPos, Color.red, Mathf.Infinity);
        }
    }

    // Detect the angle between 2 directional vectors with sign 
    float AngleBetweenVectors(Vector3 vectorA, Vector3 vectorB) {
        float angle = Vector3.Angle(vectorA, vectorB);
        Vector3 cross = Vector3.Cross(vectorA, vectorB);
        return cross.y < 0 ? -angle : angle;
    }

    bool AngleNotExceeded(float angle) {
        if (angle >= 0 && angle <= maxLeftAngle)
            return true;
        if (angle < 0 && Mathf.Abs(angle) <= maxRightAngle)
            return true;
        return false;
    }
}