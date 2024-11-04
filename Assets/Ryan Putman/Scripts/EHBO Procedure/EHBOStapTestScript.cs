using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHBOStapTestScript : MonoBehaviour
{
    // Reference to the main ExerciseTracker script
    public EHBOStappenChecker StappenTracker;

    // Customizable step name, assignable in the Inspector
    [SerializeField] private string stepName = "no action";

    // Method to call when this step is completed
    public void CompleteStep()
    {
        StappenTracker.RegisterStep(stepName);
    }

    // Example trigger method for gesture or touch detection
    void OnTriggerEnter(Collider other)
    {
            CompleteStep();
    }
}
