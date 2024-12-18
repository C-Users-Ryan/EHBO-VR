using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grijpclipboard : MonoBehaviour
{
    [SerializeField] private clipboard clipboardTasks; // Reference to the ClipboardTasks script
    [SerializeField] private string taskToComplete; // Task name to signal as completed

    // Method to trigger task completion
    public void CompleteTask()
    {
            clipboardTasks.RegisterTaskCompletion(taskToComplete);
    }

    // Optional: Trigger task completion via a Unity event
    public void CompleteTaskByName(string taskName)
    {
        clipboardTasks.RegisterTaskCompletion(taskName);
    }
    
}
