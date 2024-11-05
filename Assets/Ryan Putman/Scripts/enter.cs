using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float activationDelay = 2.0f; // Delay before the collider starts detecting triggers
    [SerializeField] private bool testAnimation = false;

    private bool isColliderActive = false;

    private void Start()
    {
        StartCoroutine(ActivateColliderAfterDelay());
    }

    private void Update()
    {
        // For testing purposes, trigger the animation if testAnimation is toggled on
        if (testAnimation)
        {
            PlayAnimation();
            testAnimation = false; // Reset after triggering
        }
    }

    private IEnumerator ActivateColliderAfterDelay()
    {
        yield return new WaitForSeconds(activationDelay);
        isColliderActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isColliderActive)
        {
            if (other.CompareTag("Player"))  // Adjust detection as needed
            {
                PlayAnimation();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isColliderActive)
        {
            if (other.CompareTag("Player"))  
            {
                StopAnimation();
            }
        }
    }

    private void PlayAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("shaking", true);
        }
        else
        {
            Debug.LogError("Animator not assigned to the script.");
        }
    }

    private void StopAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("shaking", false);
        }
        else
        {
            Debug.LogError("Animator not assigned to the script.");
        }
    }
}
