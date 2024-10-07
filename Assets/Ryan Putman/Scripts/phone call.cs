using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phonecall : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float activationDelay = 2.0f; // Delay before the collider starts detecting triggers

    private bool isColliderActive = false;

    private void Start()
    {
        StartCoroutine(ActivateColliderAfterDelay());
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
            PlaySound();
        }
    }

    private void PlaySound()
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
