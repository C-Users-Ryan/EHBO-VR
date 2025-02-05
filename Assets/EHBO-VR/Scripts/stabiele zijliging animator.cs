using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stabielezijliginganimator : MonoBehaviour
{
    // Zorg dat dit script een Animator component op hetzelfde GameObject vindt.
    private Animator animator;

    private void Awake()
    {
        // Haal de Animator-component op
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Geen Animator component gevonden op " + gameObject.name);
        }
    }

    // Roep deze methode aan om de 'zijliging'-bool op true te zetten
    public void ActivateZijliging()
    {
        if (animator != null)
        {
            animator.SetBool("zijliging", true);
        }
    }
}
