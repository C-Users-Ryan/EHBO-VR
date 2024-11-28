using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;

public class PlayAreaMarker : MonoBehaviour
{
    public GameObject cornerMarkerPrefab; // Prefab for the markers to place at corners
    private OVRBoundary ovrBoundary;
    public OVRCameraRig cameraRig; // Reference to the OVRCameraRig in the scene

    void Start()
    {
        // Initialize OVRBoundary
        ovrBoundary = new OVRBoundary();

        // Check if the boundary is configured and get its data
        var boundaryData = ovrBoundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        if (boundaryData.Length > 0)
        {
            PlaceCornerMarkers(boundaryData);
        }
        else
        {
            Debug.LogWarning("No play area boundary detected. Ensure the play area is configured.");
        }
    }

    private void PlaceCornerMarkers(Vector3[] boundaryPoints)
    {
        if (cornerMarkerPrefab == null)
        {
            Debug.LogError("No cornerMarkerPrefab assigned. Please assign a prefab in the inspector.");
            return;
        }

        if (cameraRig == null)
        {
            Debug.LogError("No OVRCameraRig assigned. Please assign the camera rig in the inspector.");
            return;
        }

        // Convert boundary points from local tracking origin space to world space
        foreach (Vector3 localPoint in boundaryPoints)
        {
            Vector3 worldPoint = cameraRig.trackingSpace.TransformPoint(localPoint);

            // Instantiate a marker at each corner in world space
            Instantiate(cornerMarkerPrefab, worldPoint, Quaternion.identity, transform);
        }

        Debug.Log("Markers placed at the corners of the play area.");
    }
}
