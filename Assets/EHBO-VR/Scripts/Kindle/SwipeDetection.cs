using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Oculus.Interaction;       // For Oculus interaction utilities, if needed
using Oculus.Interaction.Input; // For accessing OVRHand and hand tracking components
using UnityEngine.UI;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;  // The RawImage UI element to display images
    [SerializeField] private List<Texture> images;  // List of textures to cycle through
    [SerializeField] private float swipeThreshold = 0.3f;  // Minimum swipe distance to register
    [SerializeField] private TextMeshProUGUI displayText;  // Optional text display for the current image
    [SerializeField] private OVRHand handToTrack;  // The hand used for swipe gestures (e.g., left or right)

    private int currentIndex = 0;  // Tracks the current image index
    private Vector3 initialHandPosition;
    private bool isSwiping = false;

    void Start()
    {
        // Ensure there's at least one image
        if (images.Count > 0)
        {
            rawImage.texture = images[currentIndex]; // Set the first image
        }
        UpdateDisplay();
    }

    void Update()
    {
        DetectSwipeGesture();
    }

    private void DetectSwipeGesture()
    {
        // Check if the hand is being tracked
        if (handToTrack.IsTracked)
        {
            Vector3 handPosition = handToTrack.transform.position;

            // Check if the hand is in a swiping motion
            if (!isSwiping && handToTrack.GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                isSwiping = true;
                initialHandPosition = handPosition;
            }
            else if (isSwiping && !handToTrack.GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                Vector3 swipeDirection = handPosition - initialHandPosition;

                // Detect swipe right
                if (swipeDirection.x > swipeThreshold) // Swipe to the right
                {
                    CycleForward();
                }
                // Detect swipe left
                else if (swipeDirection.x < -swipeThreshold) // Swipe to the left
                {
                    CycleBackward();
                }

                // Reset swiping state
                isSwiping = false;
            }
        }
    }

    private void CycleForward()
    {
        currentIndex = (currentIndex + 1) % images.Count;
        UpdateDisplay();
    }

    private void CycleBackward()
    {
        currentIndex = (currentIndex - 1 + images.Count) % images.Count;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        // Update the RawImage with the current texture
        if (images.Count > 0)
        {
            rawImage.texture = images[currentIndex];
        }

        // Optional: Update text display with the current image's name
        if (displayText != null)
        {
            displayText.text = $"Current Image: {images[currentIndex].name}";
        }
    }
}

