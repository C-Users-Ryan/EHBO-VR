using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabdetectieman : MonoBehaviour
{
    private BoxCollider boxCollider; // BoxCollider to modify

    [SerializeField] private float requiredDuration = 2.0f; // Duration in seconds
    [SerializeField] private clipboard clipboardTasks; // Reference to the ClipboardTasks script
    [SerializeField] private string taskToComplete; // Task name to signal as completed

    [SerializeField] public bool isGrabbing = false; // Determines if the cube can turn green

    private float actionTimer = 0.0f; // Internal timer for action duration
    private bool isPerformingAction = false; // Flag to track if action is being performed
    private Coroutine showCoroutine; // Coroutine reference for delay control

    void Start()
    {
        // Get the BoxCollider component
        boxCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start the action timer if not already performing action
            if (!isPerformingAction)
            {
                isPerformingAction = true;
            if (isGrabbing)
            {
                actionTimer = 0.0f; // Reset the timer
            }
            }

            // Change the material if grabbing is enabled



                boxCollider.size *= 3f; // Increase the size by 3x
            

            // Start the coroutine for delayed visibility (if required)
            if (showCoroutine == null)
            {
                showCoroutine = StartCoroutine(ShowObjectWithDelay());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {


            // Reset the BoxCollider size back to normal when the player exits
            if (boxCollider != null)
            {
                boxCollider.size = new Vector3(1, 1, 1); // Reset to the original size
            }

            // Reset action tracking
            isPerformingAction = false;
            actionTimer = 0.0f; // Reset timer if action stops

            // Call the task completion method after the action is performed
            if (actionTimer >= requiredDuration && clipboardTasks != null)
            {
                clipboardTasks.RegisterTaskCompletion(taskToComplete);
            }

            // Stop coroutine if the player exits early
            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine);
                showCoroutine = null;
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
                isPerformingAction = false; // Stop counting after completion
            }
        }
    }

    private void CompleteTask()
    {
        // Register the task completion in the clipboard tasks
        if (clipboardTasks != null)
        {
            clipboardTasks.RegisterTaskCompletion(taskToComplete);
        }
        else
        {
            Debug.LogError("ClipboardTasks reference is not assigned in the inspector.");
        }
    }

    private IEnumerator ShowObjectWithDelay()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(1.0f); // 1 second delay

        // Optionally, you can add code to make an object visible after the delay
        // (if needed, based on your task logic)
        // For example: ObjectToMakeVisable.SetActive(true);
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
