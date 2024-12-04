using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;

public class TeacherDemoTestAreaScript : MonoBehaviour
{
    public GameObject cornerMarkerPrefab; // Prefab for the markers to place at corners and along the edges
    public GameObject randomObjectPrefab; // Prefab to spawn at a random location
    private OVRBoundary ovrBoundary;
    public OVRCameraRig cameraRig; // Reference to the OVRCameraRig in the scene

    private Vector3[] boundaryPointsWorld; // Store the boundary points in world space
    public float markerInterval = 1.0f; // Distance between markers along the boundary
    public float randomObjectBuffer = 2.0f; // Buffer distance to ensure the random object is not too close to the boundary

    void Start()
    {
        // Initialize OVRBoundary
        ovrBoundary = new OVRBoundary();

        // Check if the boundary is configured and get its data
        var boundaryData = ovrBoundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        if (boundaryData.Length > 0)
        {
            boundaryPointsWorld = TransformBoundaryPointsToWorld(boundaryData);
            PlaceMarkersAlongBoundary(boundaryPointsWorld);
            SpawnRandomObject();
        }
        else
        {
            Debug.LogWarning("No play area boundary detected. Ensure the play area is configured.");
        }
    }

    private Vector3[] TransformBoundaryPointsToWorld(Vector3[] boundaryPoints)
    {
        if (cameraRig == null)
        {
            Debug.LogError("No OVRCameraRig assigned. Please assign the camera rig in the inspector.");
            return null;
        }

        // Convert boundary points from local tracking origin space to world space
        Vector3[] worldPoints = new Vector3[boundaryPoints.Length];
        for (int i = 0; i < boundaryPoints.Length; i++)
        {
            worldPoints[i] = cameraRig.trackingSpace.TransformPoint(boundaryPoints[i]);
        }

        return worldPoints;
    }

    private void PlaceMarkersAlongBoundary(Vector3[] boundaryPoints)
    {
        if (cornerMarkerPrefab == null)
        {
            Debug.LogError("No cornerMarkerPrefab assigned. Please assign a prefab in the inspector.");
            return;
        }

        for (int i = 0; i < boundaryPoints.Length; i++)
        {
            // Get current and next points to form an edge
            Vector3 start = boundaryPoints[i];
            Vector3 end = boundaryPoints[(i + 1) % boundaryPoints.Length];

            // Place markers along the edge at specified intervals
            float distance = Vector3.Distance(start, end);
            int markerCount = Mathf.FloorToInt(distance / markerInterval);

            for (int j = 0; j <= markerCount; j++)
            {
                Vector3 markerPosition = Vector3.Lerp(start, end, j / (float)markerCount);
                Instantiate(cornerMarkerPrefab, markerPosition, Quaternion.identity, transform);
            }
        }

        Debug.Log("Markers placed along the boundary of the play area.");
    }

    public void SpawnRandomObject()
    {
        if (randomObjectPrefab == null)
        {
            Debug.LogError("No randomObjectPrefab assigned. Please assign a prefab in the inspector.");
            return;
        }

        if (boundaryPointsWorld == null || boundaryPointsWorld.Length < 3)
        {
            Debug.LogError("Boundary points not available. Cannot spawn a random object.");
            return;
        }

        Vector3 randomPosition = GetRandomPointWithinBoundary(boundaryPointsWorld);
        randomPosition = ApplyBufferToPosition(randomPosition, randomObjectBuffer);
        Debug.Log("Attempting to spawn object at: " + randomPosition);

        GameObject spawnedObject = Instantiate(randomObjectPrefab, randomPosition, Quaternion.identity);
        if (spawnedObject != null)
        {
            Debug.Log("Prefab successfully spawned!");
        }
        else
        {
            Debug.LogError("Failed to spawn prefab.");
        }
    }

    private Vector3 GetRandomPointWithinBoundary(Vector3[] boundaryPoints)
    {
        float minX = float.MaxValue, maxX = float.MinValue;
        float minZ = float.MaxValue, maxZ = float.MinValue;

        foreach (Vector3 point in boundaryPoints)
        {
            if (point.x < minX) minX = point.x;
            if (point.x > maxX) maxX = point.x;
            if (point.z < minZ) minZ = point.z;
            if (point.z > maxZ) maxZ = point.z;
        }

        // Temporarily skip polygon check
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        return new Vector3(randomX, boundaryPoints[0].y, randomZ);
    }

    // Apply the buffer to ensure the object is not too close to the boundary
    private Vector3 ApplyBufferToPosition(Vector3 position, float buffer)
    {
        Vector3 adjustedPosition = position;

        // Ensure the random object is within the buffer area
        if (adjustedPosition.x < buffer)
            adjustedPosition.x = buffer;
        else if (adjustedPosition.x > (boundaryPointsWorld[0].x - buffer))
            adjustedPosition.x = boundaryPointsWorld[0].x - buffer;

        if (adjustedPosition.z < buffer)
            adjustedPosition.z = buffer;
        else if (adjustedPosition.z > (boundaryPointsWorld[0].z - buffer))
            adjustedPosition.z = boundaryPointsWorld[0].z - buffer;

        return adjustedPosition;
    }

    private bool IsPointInsidePolygon(Vector3[] polygon, Vector3 point)
    {
        // Ray-casting algorithm to check if a point is inside a polygon
        int intersections = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            Vector3 vertex1 = polygon[i];
            Vector3 vertex2 = polygon[(i + 1) % polygon.Length];

            if (IsIntersecting(point, vertex1, vertex2))
                intersections++;
        }

        return (intersections % 2) == 1; // Odd number of intersections means inside
    }

    private bool IsIntersecting(Vector3 point, Vector3 vertex1, Vector3 vertex2)
    {
        // Check if the ray from the point intersects with the edge defined by vertex1 and vertex2
        if (vertex1.z > point.z != vertex2.z > point.z)
        {
            float intersectionX = (point.z - vertex1.z) * (vertex2.x - vertex1.x) / (vertex2.z - vertex1.z) + vertex1.x;
            return point.x < intersectionX;
        }
        return false;
    }
}
