using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RawImage background;         // Background image of the progress bar
    [SerializeField] private RawImage fill;               // Fill image that grows with progress
    [SerializeField] private float actionDuration = 2f;   // Duration for the bar to fill (in seconds)
    [SerializeField] private Collider targetCollider;     // Collider that triggers the progress bar

    private float currentProgress = 0f;                   // Tracks current progress (0 to 1)
    private bool isTouching = false;                      // Whether the collider is being touched
    private RectTransform fillRectTransform;              // Reference to the RectTransform of the fill image
    private float initialFillWidth;                       // Original width of the fill image

    void Start()
    {
        // Get RectTransform for the fill image and record its original width
        fillRectTransform = fill.GetComponent<RectTransform>();
        initialFillWidth = fillRectTransform.sizeDelta.x;

        // Ensure the fill image and background are invisible initially
        SetProgressBarVisibility(false);
        SetFillAmount(0f);
    }

    void Update()
    {
        if (isTouching)
        {
            // Increment progress over time
            currentProgress += Time.deltaTime / actionDuration;

            // Update the fill image based on current progress
            SetFillAmount(currentProgress);

            // Check if progress is complete
            if (currentProgress >= 1f)
            {
                ActionCompleted();
            }
        }
    }

    // Sets the visibility of the progress bar images
    private void SetProgressBarVisibility(bool isVisible)
    {
        if (background != null) background.enabled = isVisible;
        if (fill != null) fill.enabled = isVisible;
    }

    // Sets the fill amount of the progress bar based on progress (0-1)
    private void SetFillAmount(float progress)
    {
        // Clamp progress to range [0, 1]
        progress = Mathf.Clamp01(progress);

        // Adjust the fill image's width based on progress
        fillRectTransform.sizeDelta = new Vector2(initialFillWidth * progress, fillRectTransform.sizeDelta.y);
    }

    // Called when the action completes (when progress reaches 100%)
    private void ActionCompleted()
    {
        Debug.Log("Action completed!");
        ResetProgress();
    }

    // Resets the progress bar and hides it
    private void ResetProgress()
    {
        currentProgress = 0f;
        SetFillAmount(0f);
        SetProgressBarVisibility(false);
    }

    // Trigger event for when the collider is entered
    private void OnTriggerEnter(Collider other)
    {
        if (other == targetCollider)
        {
            isTouching = true;
            SetProgressBarVisibility(true);  // Show the progress bar
        }
    }

    // Trigger event for while the collider is being touched
    private void OnTriggerStay(Collider other)
    {
        if (other == targetCollider)
        {
            isTouching = true;
        }
    }

    // Trigger event for when the collider is exited
    private void OnTriggerExit(Collider other)
    {
        if (other == targetCollider)
        {
            isTouching = false;
            ResetProgress();  // Reset and hide the progress bar
        }
    }
}
