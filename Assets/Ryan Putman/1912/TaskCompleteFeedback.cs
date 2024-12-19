using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskCompleteFeedback : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // AudioSource to play the sound
    [SerializeField] private AudioClip soundEffect; // The sound effect to play
    [SerializeField] private TextMeshProUGUI textElement; // TextMeshPro UI element to show
    [SerializeField] private float displayDuration = 2.0f; // Time to display the text

    private float timer;
    private bool isActive;

    private void Start()
    {
        // Ensure the text element starts invisible
        if (textElement != null)
        {
            textElement.gameObject.SetActive(false);
        }
    }

    public void Activate()
    {
        if (audioSource != null && soundEffect != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }

        if (textElement != null)
        {
            textElement.gameObject.SetActive(true);
            timer = displayDuration;
            isActive = true;
        }
    }

    private void Update()
    {
        if (isActive)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                if (textElement != null)
                {
                    textElement.gameObject.SetActive(false);
                }

                isActive = false;
            }
        }
    }
}
