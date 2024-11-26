using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RawImage backgroundImage;         // Background image of the loading bar
    [SerializeField] private RawImage fillImage;               // Fill image that grows with progress
    [SerializeField] private float fillDuration = 2f;          // Time in seconds to fill the bar
    private RectTransform fillRectTransform;                   // Reference to the RectTransform of the fill image
    private float initialWidth;                                // Initial width of the fill image
    private Coroutine fillCoroutine;                           // Coroutine to control filling

    private void Start()
    {
        // Get RectTransform for the fill image and record its original width
        fillRectTransform = fillImage.GetComponent<RectTransform>();
        initialWidth = fillRectTransform.sizeDelta.x;

        // Ensure the fill image and background are invisible initially
        SetProgressBarVisibility(false);
        SetFillAmount(0f);
    }

    // Called when the player enters the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider belongs to an object with the "Player" tag
        {
            if (fillCoroutine == null)  // Only start filling if not already in progress
            {
                SetProgressBarVisibility(true);  // Show the progress bar
                fillCoroutine = StartCoroutine(FillProgress());
            }
        }
    }

    // Called when the player exits the collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider belongs to an object with the "Player" tag
        {
            if (fillCoroutine != null)
            {
                StopCoroutine(fillCoroutine);
                fillCoroutine = null;
            }
            ResetProgress();  // Reset the progress and hide the progress bar
        }
    }

    // Coroutine to gradually fill the progress bar over the specified duration
    private IEnumerator FillProgress()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / fillDuration);
            SetFillAmount(progress);  // Update fill image width based on progress
            yield return null;
        }

        // Optional: Handle any logic for when the loading is complete
        Debug.Log("Loading complete!");
        fillCoroutine = null;  // Reset coroutine when complete
    }

    // Sets the visibility of the progress bar
    private void SetProgressBarVisibility(bool isVisible)
    {
        backgroundImage.enabled = isVisible;
        fillImage.enabled = isVisible;
    }

    // Sets the fill amount of the progress bar based on progress (0 to 1)
    private void SetFillAmount(float progress)
    {
        progress = Mathf.Clamp01(progress);  // Ensure progress is between 0 and 1
        fillRectTransform.sizeDelta = new Vector2(initialWidth * progress, fillRectTransform.sizeDelta.y);
    }

    // Resets the progress bar to 0 and hides it
    private void ResetProgress()
    {
        SetFillAmount(0f);
        SetProgressBarVisibility(false);
    }
}
