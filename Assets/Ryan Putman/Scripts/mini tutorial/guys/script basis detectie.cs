using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptbasisdetectie : MonoBehaviour
{
    [SerializeField] private float requiredDuration = 2.0f; // Duration in seconds
    [SerializeField] private clipboard clipboardTasks; // Reference to the ClipboardTasks script
    [SerializeField] private string taskToComplete; // Task name to signal as completed

    [Header("Feedback Settings")]
    [SerializeField] private AudioClip completionSound; // Sound to play on completion
    [SerializeField] private Canvas completionCanvas; // Canvas to show on completion
    [SerializeField] private float canvasDisplayDuration = 2.0f; // Duration to show the canvas

    [Header("Trigger Settings")]
    [SerializeField] private List<GameObject> objectsToDeActivateOnEnter; // Objects to activate on enter
    [SerializeField] private List<GameObject> objectsToactivateOnExit; // Objects to deactivate on exit

    private float actionTimer = 0.0f;
    private bool isPerformingAction = false;
    private BoxCollider boxCollider;
    private AudioSource audioSource; // For playing sound effects

    void Start()
    {
        // Get the BoxCollider component on the object
        boxCollider = GetComponent<BoxCollider>();

        // Ensure an AudioSource is present
        audioSource = gameObject.AddComponent<AudioSource>();
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

            // Activate objects
            foreach (GameObject obj in objectsToDeActivateOnEnter)
            {
                obj.SetActive(false);
            }
        }
    }

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

            // Deactivate objects
            foreach (GameObject obj in objectsToactivateOnExit)
            {
                obj.SetActive(true);
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

        // Play completion sound
        if (completionSound != null)
        {
            audioSource.PlayOneShot(completionSound);
        }

        // Show completion canvas
        if (completionCanvas != null)
        {
            StartCoroutine(ShowCanvasTemporarily());
        }
    }

    public IEnumerator ShowCanvasTemporarily()
    {
        completionCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(canvasDisplayDuration);
        completionCanvas.gameObject.SetActive(false);
    }
}