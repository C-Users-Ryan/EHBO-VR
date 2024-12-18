using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerboxCollider : MonoBehaviour
{
    public Material newMaterial; // The new material to change to
    private Material originalMaterial; // To store the original material
    private BoxCollider boxCollider;

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
    }

    // When the player enters the box collider
    void OnTriggerEnter(Collider other)
    {
        // Change the material
        if (other.CompareTag("Player"))
        {

       
            Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && newMaterial != null)
        {
            renderer.material = newMaterial;
        }

        // Expand the BoxCollider
        if (boxCollider != null)
        {
            boxCollider.size *= 2f; // Increase the size by 50%
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
            renderer.material = originalMaterial; // Reset to the original material
        }

        // Reset the BoxCollider size back to normal
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(1, 1, 1); // Reset the size back to original
        }
        }
    }
}
