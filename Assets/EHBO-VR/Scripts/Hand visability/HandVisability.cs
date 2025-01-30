using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVisability : MonoBehaviour
{
    [SerializeField] private GameObject ObjectToMakeVisable;

    private void OnTriggerEnter(Collider other)
    {

            if (other.CompareTag("Player"))  // Adjust detection as needed
            {
            ObjectToMakeVisable.SetActive(true);
            }
        
    }


    private void OnTriggerExit(Collider other)
    {

            if (other.CompareTag("Player"))
            {
            ObjectToMakeVisable.SetActive(false);
            }
        
    }
}
