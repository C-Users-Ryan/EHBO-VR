using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeOnDelay : MonoBehaviour
{
    [SerializeField] private Material newMaterial; // Material to apply after a delay
    [SerializeField] private float delay = 1f;     // Delay in seconds before changing material
    private Material originalMaterial;            // Stores the original material of the object
    private Renderer objectRenderer;              // Renderer component of the object
    private Coroutine changeMaterialCoroutine;    // Reference to the coroutine
    private BoxCollider boxCollider;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material; // Save the initial material
        }
        else
        {
            Debug.LogError("Renderer component missing on this GameObject.");
        }
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (objectRenderer != null && newMaterial != null)
        {
            if (other.CompareTag("Player"))
            { 
                // Start a coroutine to change material after a delay
                if (changeMaterialCoroutine == null)
            {
                changeMaterialCoroutine = StartCoroutine(ChangeMaterialAfterDelay());
            }    
                if (boxCollider != null)
            {
                boxCollider.size *= 3f; // Increase the size by 50%
            }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Immediately revert to the original material and stop any ongoing coroutine
        if (objectRenderer != null && originalMaterial != null)
        {
            objectRenderer.material = originalMaterial;
            if (other.CompareTag("Player"))
            {
                if (changeMaterialCoroutine != null)
                {
                    StopCoroutine(changeMaterialCoroutine);
                    changeMaterialCoroutine = null;
                }
                if (boxCollider != null)
                {
                    boxCollider.size = new Vector3(1, 1, 1); // Reset the size back to original
                }
            }
        }
    }

    private IEnumerator ChangeMaterialAfterDelay()
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        if (objectRenderer != null)
        {
            objectRenderer.material = newMaterial; // Change to the new material
        }
        changeMaterialCoroutine = null; // Reset the coroutine reference
    }
}
