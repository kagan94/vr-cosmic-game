using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSelfDestory : MonoBehaviour {
    [Tooltip("Distance after player's pos where rock will be destroyed")]
    public int destroyDistance = 5;

    private Vector3 initialRockPos;

    public void SetInitialRockPos(Vector3 initialPos) {
        initialRockPos = initialPos;
    }

    // Update is called once per frame
    void Update() {
        Vector3 playerHeadPos = Camera.main.transform.position;
        float distanceToPlayer = Vector3.Distance(playerHeadPos, initialRockPos);
        float distanceToRock = Vector3.Distance(transform.position, initialRockPos);

        if (distanceToRock - distanceToPlayer >= destroyDistance) {
            // TODO: Add effect of explosion
            Destroy(gameObject);
        }
    }
}