using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationCall : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;

    // Animator parameter names
    private const string LeanA = "call";
    private const string LeanB = "Stop";

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator not found! Please attach this script to a GameObject with an Animator.");
        }
    }

    // Activates leanA and deactivates leanB
    public void SetLeanA()
    {
        if (animator != null)
        {
            animator.SetBool(LeanA, true);
            animator.SetBool(LeanB, false);
        }
    }

    // Activates leanB and deactivates leanA
    public void SetLeanB()
    {
        if (animator != null)
        {
            animator.SetBool(LeanA, false);
            animator.SetBool(LeanB, true);
        }
    }
}
