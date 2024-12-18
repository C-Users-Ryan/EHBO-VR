using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modeselection : MonoBehaviour
{
    [SerializeField] private List<GameObject> task1Objects; // Objects to activate for Task 1
    [SerializeField] private List<GameObject> task2Objects; // Objects to activate for Task 2
    [SerializeField] private List<GameObject> task3Objects; // Objects to activate for Task 3
    [SerializeField] private List<GameObject> task4Objects; // Objects to activate for Task 4

    // Method to activate objects for Task 1
    public void ActivateTask1()
    {
        SetActiveObjects(task1Objects);
    }

    // Method to activate objects for Task 2
    public void ActivateTask2()
    {
        SetActiveObjects(task2Objects);
    }

    // Method to activate objects for Task 3
    public void ActivateTask3()
    {
        SetActiveObjects(task3Objects);
    }

    // Method to activate objects for Task 4
    public void ActivateTask4()
    {
        SetActiveObjects(task4Objects);
    }

    // Helper method to activate the specified objects and deactivate others
    private void SetActiveObjects(List<GameObject> activeObjects)
    {
        // Deactivate all objects in the scene managed by this script
        DeactivateAllObjects();

        // Activate the specified objects
        foreach (GameObject obj in activeObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    // Deactivates all managed objects in the lists
    private void DeactivateAllObjects()
    {
        List<List<GameObject>> allTasks = new List<List<GameObject>> { task1Objects, task2Objects, task3Objects, task4Objects };

        foreach (List<GameObject> taskObjects in allTasks)
        {
            foreach (GameObject obj in taskObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
