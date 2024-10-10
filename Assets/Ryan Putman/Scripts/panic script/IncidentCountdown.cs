using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IncidentCountdown : MonoBehaviour
{
    // Variables for countdown and control
    public bool IsActive = false;
    public float countdownTime = 5f; // Default countdown time
    public bool useRandomTime = false; // Toggle for random countdown time
    public float randomMin = 3f; // Minimum random countdown time
    public float randomMax = 10f; // Maximum random countdown time

    public Animator[] objectsToAnimate; // Reference to the animators

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
        if (IsActive)
        {
            // Reduce countdown timer
            currentTime -= Time.deltaTime;

            // Check if countdown has finished
            if (currentTime <= 0f)
            {
                ActivateAction();
            }
        }
    }

    void ActivateAction()
    {
        // Trigger the animation on all specified objects
        SwapAnimation();

        // Find and disable all NavMeshAgents in the scene
        NavMeshAgent[] navAgents = FindObjectsOfType<NavMeshAgent>();
        foreach (NavMeshAgent agent in navAgents)
        {
            agent.enabled = false;
        }

        // Optionally, you can destroy this script to stop further updates
        Destroy(this);
    }

    // Call this method to start the countdown
    public void Activate()
    {
        IsActive = true;
        currentTime = countdownTime; // Reset countdown time when activated
    }

    // Swap animation on the specified animator objects
    public void SwapAnimation()
    {
        foreach (Animator animator in objectsToAnimate)
        {
            if (animator != null)
            {
                animator.SetBool("shocked", true); // Set the 'shocked' animation parameter to true
            }
        }
    }
}
