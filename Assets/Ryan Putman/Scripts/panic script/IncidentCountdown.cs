using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncidentCountdown : MonoBehaviour
{
    // Variables for countdown and control
    public float countdownTime = 5f; // Default countdown time
    public bool useRandomTime = false; // Toggle for random countdown time
    public float randomMin = 3f; // Minimum random countdown time
    public float randomMax = 10f; // Maximum random countdown time

    public Animator animator; // Reference to the animator

    private float currentTime;

    void Start()
    {
        // Set countdown time
        if (useRandomTime)
        {
            countdownTime = Random.Range(randomMin, randomMax);
        }

        currentTime = countdownTime;
    }

    void Update()
    {
        // Reduce countdown timer
        currentTime -= Time.deltaTime;

        // Check if countdown has finished
        if (currentTime <= 0f)
        {
            ActivateAction();
        }
    }

    void ActivateAction()
    {
        // Set Animator's 'lean' bool to true
        if (animator != null)
        {
            animator.SetBool("lean", true);
        }

        // Find and disable all instances of ScriptToDisable
        MoveToWaypoint[] scriptsToDisable = FindObjectsOfType<MoveToWaypoint>();
        foreach (MoveToWaypoint script in scriptsToDisable)
        {
            script.DisableScript();
        }

        // Optionally destroy this script to stop further updates
        Destroy(this);
    }
}
