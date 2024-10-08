using Meta.XR.MRUtilityKit.SceneDecorator;
using Oculus.Interaction.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePeople : MonoBehaviour
{
    // Array to hold game objects to be activated
    public GameObject[] objectsToActivate;

    // Boolean to toggle activation of objects
    public bool Activate = false;

    // A flag to track the previous state of the "Activate" bool
    private bool previousState;

    void Start()
    {
        // Initialize objects based on the starting value of Activate
        SetObjectsActive(Activate);

        // Store the initial state of Activate
        previousState = Activate;
    }

    void Update()
    {
        // If the value of Activate changes, update the objects' active state
        if (previousState != Activate)
        {
            SetObjectsActive(Activate);
            previousState = Activate;  // Update the previous state
        }
    }

    // Function to set objects active or inactive
    public void SetObjectsActive(bool isActive)
    {
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null) // Check if the object is not null
            {
                obj.SetActive(isActive);
                Debug.Log(isActive ? "Object set active" : "Object set inactive");
            }
        }
    }
}
