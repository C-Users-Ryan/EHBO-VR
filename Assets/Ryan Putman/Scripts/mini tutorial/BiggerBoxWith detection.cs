using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerBoxWithdetection : MonoBehaviour
{
    [SerializeField] private clipboard clipboardTasks; // Reference to the ClipboardTasks script
    [SerializeField] private string taskToComplete; // Task name to signal as completed

    [SerializeField] private Material newMaterial; // The new material to change to
    private Material originalMaterial; // To store the original material
    private BoxCollider boxCollider; // BoxCollider to modify

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
        if (other.CompareTag("Player"))
        {
            // Change the material
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && newMaterial != null)
            {
                renderer.material = newMaterial;
            }

            // Expand the BoxCollider
            if (boxCollider != null)
            {
                boxCollider.size *= 3f; // Increase the size by 3x
            }

            // Complete the task
            CompleteTask();
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

    // Method to trigger task completion
    private void CompleteTask()
    {
        if (clipboardTasks != null)
        {
            clipboardTasks.RegisterTaskCompletion(taskToComplete);
        }
        else
        {
            Debug.LogError("ClipboardTasks reference is not assigned in the inspector.");
        }
    }

    // Optional: Trigger task completion via a Unity event
    public void CompleteTaskByName(string taskName)
    {
        if (clipboardTasks != null)
        {
            clipboardTasks.RegisterTaskCompletion(taskName);
        }
        else
        {
            Debug.LogError("ClipboardTasks reference is not assigned in the inspector.");
        }
    }
}
