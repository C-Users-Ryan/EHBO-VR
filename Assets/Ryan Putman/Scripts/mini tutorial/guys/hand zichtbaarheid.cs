using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handzichtbaarheid : MonoBehaviour
{
    [SerializeField] private GameObject ObjectToMakeVisable;
    [SerializeField] private float delayTime = 1.0f; // 1 second delay

    private Coroutine showCoroutine; // Coroutine reference for control

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Adjust detection as needed
        {
            // Start the coroutine to show the object after the delay
            if (showCoroutine == null)
            {
                showCoroutine = StartCoroutine(ShowObjectWithDelay());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide the object immediately when the player exits
            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine); // Stop the coroutine if the player exits early
                showCoroutine = null;
            }
            ObjectToMakeVisable.SetActive(false); // Make sure the object is hidden
        }
    }

    private IEnumerator ShowObjectWithDelay()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // After the delay, make the object visible
        ObjectToMakeVisable.SetActive(true);
    }
}
