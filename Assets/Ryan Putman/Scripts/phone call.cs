using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phonecall : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float activationDelay = 2.0f; // Delay before the collider starts detecting triggers

    public void PlaySound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (audioSource == null)
        {
            Debug.LogError("AudioSource not assigned to the script.");
        }
    }
}
