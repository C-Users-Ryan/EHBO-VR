using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabclipboard : MonoBehaviour
{

    private Material originalMaterial; // To store the original material
    private BoxCollider boxCollider; // BoxCollider to modify
    [SerializeField] private clipboard clipboardTasks; // Reference to the ClipboardTasks script
    [SerializeField] private string taskToComplete; // Task name to signal as completed

    [SerializeField] public bool isGrabbing = false; // Determines if the cube can turn green

    // Set initial conditions
    void Start()
    {


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
                CompleteTask();
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

            
        }
        if (other.CompareTag("Player") && isGrabbing)
        {
            CompleteTask();
        }
    }

    // When the player exits the box collider
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

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

    public void CompleteTask()
    {
        clipboardTasks.RegisterTaskCompletion(taskToComplete);
    }

    // Optional: Trigger task completion via a Unity event
    public void CompleteTaskByName(string taskName)
    {
        clipboardTasks.RegisterTaskCompletion(taskName);
    }
}
