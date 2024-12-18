using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;

public class GrabDetection : MonoBehaviour
{
    [SerializeField] private OVRHand handToTrack;  // The hand used for detecting grab gestures
    [SerializeField] private Collider grabCollider;  // Collider within which the grab is detected
    [SerializeField] private float grabThreshold = 0.8f;  // Threshold for a closed hand/grabbing gesture
    [SerializeField] private Material grabbedMaterial;  // Material for the grabbed state

    private Material defaultMaterial;  // Material for the default state
    private Renderer objectRenderer;  // Renderer of the object
    private bool isGrabbing = false;  // Tracks if a grabbing action is currently happening

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            defaultMaterial = objectRenderer.material;  // Save the current material as the default
        }
        else
        {
            Debug.LogError("No Renderer found on the object. Please attach this script to an object with a Renderer.");
        }
    }

    void Update()
    {
        DetectGrabGesture();
    }

    private void DetectGrabGesture()
    {
        // Check if the hand is being tracked
        if (handToTrack.IsTracked && grabCollider != null)
        {
            // Check if the hand is inside the collider
            if (grabCollider.bounds.Contains(handToTrack.transform.position))
            {
                // Detect grabbing based on pinch strength
                float grabStrength = handToTrack.GetFingerPinchStrength(OVRHand.HandFinger.Index);

                if (!isGrabbing && grabStrength >= grabThreshold)
                {
                    isGrabbing = true;
                    ChangeMaterial(grabbedMaterial);  // Change to grabbed material
                }
                else if (isGrabbing && grabStrength < grabThreshold)
                {
                    isGrabbing = false;
                    ChangeMaterial(defaultMaterial);  // Revert to default material
                }
            }
        }
    }

    private void ChangeMaterial(Material newMaterial)
    {
        if (objectRenderer != null && newMaterial != null)
        {
            objectRenderer.material = newMaterial;
        }
    }
}
