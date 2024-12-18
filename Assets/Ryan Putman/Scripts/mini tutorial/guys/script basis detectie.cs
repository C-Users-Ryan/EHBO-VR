using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptbasisdetectie : MonoBehaviour
{
    // Customizable step name and required duration, assignable in the Inspector
    [SerializeField] private float requiredDuration = 2.0f; // Duration in seconds

    [SerializeField] private clipboard clipboardTasks; // Reference to the ClipboardTasks script
    [SerializeField] private string taskToComplete; // Task name to signal as completed

    // Internal timer to track how long the action is performed
    private float actionTimer = 0.0f;
    private bool isPerformingAction = false;

    // BoxCollider to modify
    private BoxCollider boxCollider;

    // Method to start counting when the action begins

    // Method to call when this step is completed
    public void CompleteTask()
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

    void Start()
    {
        // Get the BoxCollider component on the object
        boxCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPerformingAction = true;  // Start counting

            // Expand the BoxCollider size when the player enters
            if (boxCollider != null)
            {
                boxCollider.size *= 3f; // Increase the size by 3x
            }
        }
    }

    // Method to reset if the action is interrupted
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPerformingAction = false;
            actionTimer = 0.0f;  // Reset timer if the action stops

            // Reset the BoxCollider size when the player exits
            if (boxCollider != null)
            {
                boxCollider.size /= 3f; // Restore original size
            }
        }
    }

    void Update()
    {
        // Only count time if the action is being performed
        if (isPerformingAction)
        {
            actionTimer += Time.deltaTime;

            // Check if the required duration is reached
            if (actionTimer >= requiredDuration)
            {
                CompleteTask();
                isPerformingAction = false;  // Stop counting after completion
            }
        }
    }
}
