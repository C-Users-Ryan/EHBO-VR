using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luchtweg : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))  // Adjust detection as needed
        {
            PlayAnimation();
        }

    }

    private void PlayAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("luchtweg", true);
        }
        else
        {
            Debug.LogError("Animator not assigned to the script.");
        }
    }
}
