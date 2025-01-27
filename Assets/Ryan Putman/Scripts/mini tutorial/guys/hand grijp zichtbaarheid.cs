using System.Collections.Generic;
using UnityEngine;

public class handgrijpzichtbaarheid : MonoBehaviour
{
    [SerializeField] private GameObject ObjectToMakeVisable; // Object to show
    [SerializeField] private bool isGrabbing = false; // Visibility condition

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isGrabbing) // Check if isGrabbing is true
        {
            // Make the object visible immediately
            ObjectToMakeVisable.SetActive(true);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isGrabbing)
        {


        }
        if (other.CompareTag("Player") && isGrabbing)
        {
            ObjectToMakeVisable.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide the object immediately when the player exits
            ObjectToMakeVisable.SetActive(false);
        }
    }

    // Optional methods to control the isGrabbing flag
    public void EnableGrabbing()
    {
        isGrabbing = true;
    }

    public void DisableGrabbing()
    {
        isGrabbing = false;
    }
}