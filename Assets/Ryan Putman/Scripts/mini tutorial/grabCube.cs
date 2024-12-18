using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    [SerializeField] private Material newMaterial; // The new material to change to
    private Material originalMaterial; // To store the original material
    private BoxCollider boxCollider; // BoxCollider to modify

    [SerializeField] public bool isGrabbing = false; // Determines if the cube can turn green

    // Set initial conditions
    void Start()
    {
        // Store the original material
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
        }

        // Get the BoxCollider component
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("No BoxCollider found on the GameObject.");
        }
    }

    // When the player enters the box collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isGrabbing)
        {
            // Change the material
            if (isGrabbing)
            {
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null && newMaterial != null)
                {
                    renderer.material = newMaterial;
                }
            }

            // Expand the BoxCollider
            if (boxCollider != null)
            {
                boxCollider.size *= 3f; // Increase the size by 3x
            }
        }
    }

    // Continuously check if the player is staying in the collider
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isGrabbing)
        {
            // Ensure the material does not change if isGrabbing is false
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && renderer.material != originalMaterial)
            {
                renderer.material = originalMaterial;
            }
        }
        if (other.CompareTag("Player") && isGrabbing)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && newMaterial != null)
            {
                renderer.material = newMaterial;
            }
        }
    }

    // When the player exits the box collider
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset the material back to the original one
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterial;
            }

            // Reset the BoxCollider size back to normal
            if (boxCollider != null)
            {
                boxCollider.size = new Vector3(1, 1, 1); // Reset to the original size
            }
        }
    }

    // Set isGrabbing to true
    public void EnableGrabbing()
    {
        isGrabbing = true;
    }

    // Set isGrabbing to false
    public void DisableGrabbing()
    {
        isGrabbing = false;
    }
}
