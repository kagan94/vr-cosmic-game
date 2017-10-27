using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using DG.Tweening;
using UnityEngine;

public class SquareWaypointSystem : MonoBehaviour {
    public List<Transform> waypoints;
    public float stopDistance = 2.2f;
    public float speed = 5.25f;
    public int destPoint = 0;
    Vector3[] positionsOfWaypoints = new Vector3[4];//= new List<Vector3>();
    
    // public Vector3[] points;
    private void Start() {
        List<Vector3> tmpWaypointsPositions = new List<Vector3>();
        foreach (Transform waypoint in waypoints) {
            tmpWaypointsPositions.Add(waypoint.position);
        }
        // positionsOfWaypoints = tmpWaypointsPositions.ToArray();
        positionsOfWaypoints[0] = new Vector3(12, 32, -0.7099972f);
        positionsOfWaypoints[1] = new Vector3(-2.880859f, 31, -18);
        positionsOfWaypoints[2] = new Vector3(-2.880859f, 41, -18);
        positionsOfWaypoints[3] = new Vector3(-2.880859f, 51, -20);
    }

    void Update() {
        float duration = 15.5f;
//        transform.DOPath(positionsOfWaypoints, duration, PathType.CatmullRom, PathMode.Full3D).SetLoops(5);
         transform.DOMove(positionsOfWaypoints[1], 10.0f);

//        float dist = Vector3.Distance(gameObject.transform.position, waypoints[destPoint].transform.position);
//
//        if (dist < stopDistance) {
//            destPoint = (destPoint + 1) % waypoints.Count;
//        } else {
//            Move();
//        }
    }

//    void Move() {
//        gameObject.transform.LookAt(waypoints[destPoint].transform.position);
//        gameObject.transform.position += gameObject.transform.forward * speed * Time.deltaTime;
//    }
}
