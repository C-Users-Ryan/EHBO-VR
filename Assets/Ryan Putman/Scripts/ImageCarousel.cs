using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCarousel : MonoBehaviour
{
    public RawImage rawImage; // Het RawImage UI-element
    public Texture[] images; // Array van afbeeldingen
    private int currentIndex = 0; // Huidige index in de array

    void Start()
    {
        // Zorg ervoor dat er ten minste één afbeelding is
        if (images.Length > 0)
        {
            rawImage.texture = images[currentIndex]; // Zet de eerste afbeelding
        }
    }

    void Update()
    {
        // Controleer op het indrukken van de "N" toets
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextImage(); // Ga naar de volgende afbeelding
        }
    }

    void NextImage()
    {
        // Verhoog de index
        currentIndex++;

        // Als de index de lengte van de array overschrijdt, reset naar 0
        if (currentIndex >= images.Length)
        {
            currentIndex = 0;
        }

        // Update de texture van de RawImage
        rawImage.texture = images[currentIndex];
    }
}
