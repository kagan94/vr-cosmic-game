using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockSpawner : MonoBehaviour {
    [Tooltip("Rock prefab that will be generated")]
    public GameObject rockPrefab;

    [Tooltip("Distance from player where new rock will be generated")]
    public int distanceToGeneratedRock = 40;

    [Tooltip("Max Distance from player where new rock will be generated")]
    public int maxDistance = 220;

    [Tooltip("Max Angle between initial and current player pos from Left side (if exceeded, new rock won't be generated)")]
    [Range(0, 180)]
    public int maxLeftAngle = 30;

    [Tooltip("Max Angle between initial and current player pos from Right side (if exceeded, new rock won't be generated)")]
    [Range(0, 180)]
    public int maxRightAngle = 30;

    public int generateEveryNSeconds = 1;
    public int sphereRadius = 5;
    public bool debug = true;
    
    private GameManager gameManager;
    private Vector3 initialCameraPos;
    private GameObject player;
    private Rigidbody playerRb;
    private float timeElapsed;
    private float hitPlayerInXSeconds = 8.0f;
    private float rockSpeed;

    void Start() {
        initialCameraPos = Camera.main.transform.position;
        gameManager = (GameManager) Camera.main.GetComponent(typeof(GameManager));

        player = GameObject.Find("Player");
        playerRb = player.GetComponent<Rigidbody>();
        InstantiateRock();
    }

    void Update() {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= generateEveryNSeconds) {
            InstantiateRock();
            timeElapsed = 0;
        }
    }

    void OnDrawGizmos() {
        if (debug)
            Gizmos.DrawSphere(transform.position, sphereRadius);
    }

    void InstantiateRock() {
        if (!gameManager.IsPlayingState())
            return;
        
        // Generate rand position in rock spawner sphere
        Vector3 startPos = (Random.insideUnitSphere * sphereRadius) + transform.position;
        if (debug) // draw new random start pos
            Debug.DrawLine(startPos, startPos + Vector3.forward, Color.red, 1);

        Vector3 targetPos = Camera.main.transform.position; // pos of player head

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

            // Change the distance where new rock will be generated 
            Vector3 newRockPos = Vector3.Lerp(targetPos, startPos, relativePos);
            startPos = newRockPos;

            // Calculate the speed needed to reach the Player in X seconds
            float newDistanceToPlayer = Vector3.Distance(targetPos, startPos);
            float speed = newDistanceToPlayer / hitPlayerInXSeconds;
            rockSpeed = speed;

            // Make a prediction of next Player's position
            Vector3 projectedPos;
            try {
                projectedPos = PredictPosition(targetPos, startPos, playerRb.velocity, rockSpeed);
            } catch(Exception e) {
                Debug.Log(e.Message);
                return;
            }

            // Set target pos to Predicted pos
            targetPos = projectedPos;

            GameObject newRock = Instantiate(rockPrefab, newRockPos, transform.rotation);
            Debug.Log("Scale: " + newRock.transform.localScale.x + " " + newRock.transform.localScale.y + " " + newRock.transform.localScale.z);
            newRock.transform.localScale = new Vector3(Random.Range(15.0f, 50.0f), Random.Range(15.0f, 50.0f), Random.Range(15.0f, 50.0f));

            Rigidbody newRockRb = newRock.GetComponent<Rigidbody>();
            newRock.GetComponent<RockSelfDestory>().SetInitialRockPos(newRockPos);

            Vector3 directionLine = targetPos - newRockPos;
            Vector3 force = directionLine.normalized * rockSpeed;
            newRockRb.AddForce(force, ForceMode.VelocityChange);

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

    private Vector3 PredictPosition(Vector3 targetPosition, Vector3 shooterPosition, Vector3 targetVelocity, float projectileSpeed) {
        Vector3 displacement = targetPosition - shooterPosition;
        float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
        //if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
        if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed
            && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude) {
            throw new Exception("Position prediction is not feasible.");
        }
        // use Sine Formula to detect new position
        float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
        return targetPosition + targetVelocity * displacement.magnitude /
                   Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
    }
}
