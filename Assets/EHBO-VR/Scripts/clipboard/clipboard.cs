using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class clipboard : MonoBehaviour
{
    [System.Serializable] // Ensure Unity can serialize this class
    public class Task
    {
        [Tooltip("Name of the task")] public string taskName; // Name of the task
        [Tooltip("Image to display task state")] public RawImage taskImage; // RawImage to display task state
        [Tooltip("Text for the task description")] public TextMeshProUGUI taskText; // TextMeshPro component for task description
        [Tooltip("Placeholder text for unrevealed tasks")] public string placeholderText; // Placeholder text for unreadable tasks
        [Tooltip("Texture for completed task")] public Texture completedTexture; // Texture for completed task
    }

    [SerializeField]
    [Tooltip("List of tasks to display on the clipboard")]
    private List<Task> tasks = new List<Task>(); // List of tasks

    private int currentTaskIndex = 0; // Tracks the index of the current task

    void Start()
    {
        InitializeTasks();
    }

    // Initialize all tasks, showing only the first task with its description
    private void InitializeTasks()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            if (i == 0)
            {
                tasks[i].taskText.text = tasks[i].taskName; // Show first task's name
            }
            else
            {
                tasks[i].taskText.text = tasks[i].placeholderText; // Show placeholder for other tasks
            }
        }
    }

    // Method to register a completed task from another script
    public void RegisterTaskCompletion(string taskName)
    {
        // Ensure the taskName matches the current task
        if (currentTaskIndex < tasks.Count && tasks[currentTaskIndex].taskName == taskName)
        {
            CompleteCurrentTask();
        }
        else
        {
            Debug.Log($"Task '{taskName}' ignored. It is not the current task.");
        }
    }

    // Completes the current task and updates the clipboard
    private void CompleteCurrentTask()
    {
        Task currentTask = tasks[currentTaskIndex];

        // Update the image to the completed texture
        if (currentTask.completedTexture != null)
        {
            currentTask.taskImage.texture = currentTask.completedTexture;
        }

        // Proceed to the next task if available
        currentTaskIndex++;
        if (currentTaskIndex < tasks.Count)
        {
            Task nextTask = tasks[currentTaskIndex];
            nextTask.taskText.text = nextTask.taskName; // Reveal the next task's description
        }
        else
        {
            Debug.Log("All tasks completed!");
        }
    }
}
