using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using Oculus.Interaction.HandGrab;

public class Phonegrab : MonoBehaviour
{
    [SerializeField] private GameObject prefabToPlace; // The prefab to instantiate in the player's hand
    [SerializeField] private HandGrabInteractor handGrabInteractor; // Reference to the HandGrabInteractor component on the player's hand
    [SerializeField] private Transform handTransform; // The transform of the player's hand

    private void OnTriggerStay(Collider other)
    {
        // Check if the handGrabInteractor is in a grabbing state
        if (handGrabInteractor != null)
        {
            Debug.Log("Collider detected. Checking if hand is grabbing...");

            if (handGrabInteractor.IsGrabbing)
            {
                Debug.Log("Grabbing detected!");
                PlacePrefabInHand();
            }
            else
            {
                Debug.Log("Not grabbing.");
            }
        }
    }

    private void PlacePrefabInHand()
    {
        // Instantiate the prefab at the hand's position and parent it to the hand
        if (handTransform != null && prefabToPlace != null)
        {
            Debug.Log("Placing prefab in hand...");
            GameObject instantiatedPrefab = Instantiate(prefabToPlace, handTransform.position, handTransform.rotation);
            instantiatedPrefab.transform.SetParent(handTransform);
        }
        else
        {
            Debug.LogWarning("Prefab or Hand Transform is not set!");
        }
    }
}
