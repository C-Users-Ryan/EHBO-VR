using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskCompleter : MonoBehaviour
{
    [SerializeField] private clipboard clipboardTasks; // Reference to the ClipboardTasks script
    [SerializeField] private string taskToComplete; // Task name to signal as completed

    // Method to trigger task completion
    public void CompleteTask()
    {
        if (clipboardTasks != null)
        {
            clipboardTasks.RegisterTaskCompletion(taskToComplete);
        }
        else
        {
            Debug.LogError("ClipboardTasks reference is not assigned in the inspector.");
        }
    }

    // Optional: Trigger task completion via a Unity event
    public void CompleteTaskByName(string taskName)
    {
        if (clipboardTasks != null)
        {
            clipboardTasks.RegisterTaskCompletion(taskName);
        }
        else
        {
            Debug.LogError("ClipboardTasks reference is not assigned in the inspector.");
        }
    }
}
