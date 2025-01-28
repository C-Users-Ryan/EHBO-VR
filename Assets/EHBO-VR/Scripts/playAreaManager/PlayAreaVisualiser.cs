using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;

public class PlayAreaVisualiser : MonoBehaviour
{
    public GameObject cornerMarkerPrefab;
    public Transform objectToMove; // Reference to the object to move
    private OVRBoundary ovrBoundary;
    public OVRCameraRig cameraRig;

    private Vector3[] boundaryPointsWorld;
    public float edgeBufferDistance = 1.0f; // Adjustable buffer distance from the edges
    public float cameraBufferDistance = 1.0f; // Adjustable buffer distance from the camera rig

    void Start()
    {
        ovrBoundary = new OVRBoundary();
        var boundaryData = ovrBoundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

        if (boundaryData.Length > 0)
        {
            boundaryPointsWorld = TransformBoundaryPointsToWorld(boundaryData);
            PlaceCornerMarkers(boundaryPointsWorld);
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

        Vector3[] worldPoints = new Vector3[boundaryPoints.Length];
        for (int i = 0; i < boundaryPoints.Length; i++)
        {
            worldPoints[i] = cameraRig.trackingSpace.TransformPoint(boundaryPoints[i]);
        }

        return worldPoints;
    }

    private void PlaceCornerMarkers(Vector3[] boundaryPoints)
    {
        if (cornerMarkerPrefab == null)
        {
            Debug.LogError("No cornerMarkerPrefab assigned. Please assign a prefab in the inspector.");
            return;
        }

        foreach (Vector3 worldPoint in boundaryPoints)
        {
            Instantiate(cornerMarkerPrefab, worldPoint, Quaternion.identity, transform);
        }

        Debug.Log("Markers placed at the corners of the play area.");
        MoveObjectToRandomPoint();
    }

    public void MoveObjectToRandomPoint()
    {
        if (objectToMove == null)
        {
            Debug.LogError("No objectToMove assigned. Please assign a transform in the inspector.");
            return;
        }

        if (boundaryPointsWorld == null || boundaryPointsWorld.Length < 3)
        {
            Debug.LogError("Boundary points not available. Cannot move the object.");
            return;
        }

        Vector3 targetPosition = GetValidRandomPoint();
        Debug.Log("Moving object to: " + targetPosition);

        objectToMove.position = targetPosition;
        Debug.Log("Object successfully moved!");
    }

    private Vector3 GetValidRandomPoint()
    {
        float minX = float.MaxValue, maxX = float.MinValue;
        float minZ = float.MaxValue, maxZ = float.MinValue;

        foreach (Vector3 point in boundaryPointsWorld)
        {
            if (point.x < minX) minX = point.x;
            if (point.x > maxX) maxX = point.x;
            if (point.z < minZ) minZ = point.z;
            if (point.z > maxZ) maxZ = point.z;
        }

        float buffer = edgeBufferDistance;

        // Ensure buffer doesn't make moving impossible
        if (maxX - minX < 2 * buffer || maxZ - minZ < 2 * buffer)
        {
            buffer = 0;
        }

        float randomX = Random.Range(minX + buffer, maxX - buffer);
        float randomZ = Random.Range(minZ + buffer, maxZ - buffer);
        Vector3 candidatePosition = new Vector3(randomX, boundaryPointsWorld[0].y, randomZ);

        // Check if it's too close to the camera rig
        if (cameraRig != null)
        {
            Vector3 cameraPosition = cameraRig.transform.position;
            if (Vector3.Distance(candidatePosition, cameraPosition) < cameraBufferDistance)
            {
                return GetValidRandomPoint(); // Retry for another point
            }
        }

        return candidatePosition;
    }
}
