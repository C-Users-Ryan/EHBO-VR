using System.Collections;
using UnityEngine;

public class HandgrijpZichtbaarheid : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer; // Renderer of the object to change color
    [SerializeField] private Color targetColor = Color.green; // Target color (green)
    [SerializeField] private float colorTransitionDuration = 2.0f; // Duration for the color transition

    private Color originalColor; // Original color of the object
    private Coroutine colorChangeCoroutine; // To keep track of the current coroutine

    private void Start()
    {
        // Get the original color from the renderer's material
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
        else
        {
            Debug.LogError("Renderer is not assigned in the inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start changing to green
            if (colorChangeCoroutine != null)
            {
                StopCoroutine(colorChangeCoroutine); // Stop any ongoing color change
            }
            colorChangeCoroutine = StartCoroutine(ChangeColor(targetColor));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start changing back to the original color
            if (colorChangeCoroutine != null)
            {
                StopCoroutine(colorChangeCoroutine); // Stop any ongoing color change
            }
            colorChangeCoroutine = StartCoroutine(ChangeColor(originalColor));
        }
    }

    private IEnumerator ChangeColor(Color targetColor)
    {
        // Smoothly transition the color over time
        float elapsedTime = 0f;
        Color currentColor = objectRenderer.material.color;

        while (elapsedTime < colorTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            objectRenderer.material.color = Color.Lerp(currentColor, targetColor, elapsedTime / colorTransitionDuration);
            yield return null;
        }

        // Ensure the color is exactly the target color at the end
        objectRenderer.material.color = targetColor;
    }
}
