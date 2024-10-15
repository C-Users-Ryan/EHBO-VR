using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNGMoveToWaypoint : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints the object will move between
    public float speed = 5.0f; // Speed of movement
    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return; // If no waypoints are set, do nothing

        // Move towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        // Check if we've reached the waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
