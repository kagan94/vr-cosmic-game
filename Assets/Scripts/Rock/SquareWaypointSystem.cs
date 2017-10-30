using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using DG.Tweening;
using UnityEngine;

public class SquareWaypointSystem : MonoBehaviour {
    public List<Transform> waypoints;
    public float stopDistance = 2.2f;
    public float speed = 5.25f;
    public int destPoint = 0;
//    Vector3[] positionsOfWaypoints = new Vector3[4];//= new List<Vector3>();
    
    // public Vector3[] points;
    private void Start() {
//        List<Vector3> tmpWaypointsPositions = new List<Vector3>();
//        foreach (Transform waypoint in waypoints) {
//            tmpWaypointsPositions.Add(waypoint.position);
//        }
//        float duration = 8.5f;
//        Vector3[] positionsOfWaypoints = tmpWaypointsPositions.ToArray();
//        
//        transform.DOPath(positionsOfWaypoints, duration, PathType.CatmullRom, PathMode.Full3D)
//            .SetLoops(4, LoopType.Restart)
//            .From(true);
    }

    void Update() {
    }

//    void Move() {
//        gameObject.transform.LookAt(waypoints[destPoint].transform.position);
//        gameObject.transform.position += gameObject.transform.forward * speed * Time.deltaTime;
//    }
}
