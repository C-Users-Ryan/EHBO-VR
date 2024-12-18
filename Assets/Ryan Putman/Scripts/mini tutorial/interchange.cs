using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interchange : MonoBehaviour
{
    [Tooltip("List of objects to cycle through.")]
    public List<GameObject> objectsToManage;

    private int currentIndex = 0;

    /// <summary>
    /// Activates the next object in the list and deactivates the rest.
    /// </summary>
    public void ActivateNextObject()
    {
        if (objectsToManage == null || objectsToManage.Count == 0)
        {
            Debug.LogWarning("No objects assigned to manage.");
            return;
        }

        // Deactivate all objects
        foreach (GameObject obj in objectsToManage)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Activate the next object
        if (objectsToManage[currentIndex] != null)
            objectsToManage[currentIndex].SetActive(true);

        // Increment index and loop back if necessary
        currentIndex = (currentIndex + 1) % objectsToManage.Count;
    }

    /// <summary>
    /// Resets the cycle to the first object in the list.
    /// </summary>
    public void ResetCycle()
    {
        currentIndex = 0;
        ActivateNextObject();
    }
}
