using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EHBOStappenChecker : MonoBehaviour
{
    // Array to hold the correct order of steps, assignable in the Inspector
    [SerializeField] private List<string> correctOrder;

    // List to track the order of completed steps
    private List<string> completedSteps = new List<string>();

    // TextMeshPro UI Elements
    public TextMeshProUGUI debugPanelText;      // TextMeshPro component for in-game panel
    public GameObject summaryPanel;             // Summary window panel
    public TextMeshProUGUI summaryText;         // TextMeshPro component for summary display
    public Button resetButton;                  // Button to reset the level

    void Start()
    {
        // Hide the summary panel at the beginning
        summaryPanel.SetActive(false);
        resetButton.onClick.AddListener(ResetLevel);
    }

    // Method called by each step script when an action is completed
    public void RegisterStep(string stepName)
    {
        // Check if the last completed step is different from the current step
        if (completedSteps.Count == 0 || completedSteps[completedSteps.Count - 1] != stepName)
        {
            completedSteps.Add(stepName);
            DisplayDebugInfo();
        }
        else
        {
            Debug.Log($"Action '{stepName}' ignored to prevent duplicate consecutive entry.");
        }
    }

    // Compares completed steps to the correct order
    public void ValidateOrder()
    {
        bool isCorrect = true;

        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (i >= completedSteps.Count || completedSteps[i] != correctOrder[i])
            {
                isCorrect = false;
                break;
            }
        }

        ShowSummary(isCorrect);
    }

    // Updates the debug panel and console
    private void DisplayDebugInfo()
    {
        string debugText = "Steps Completed:\n";

        for (int i = 0; i < completedSteps.Count; i++)
        {
            debugText += $"{i + 1}. {completedSteps[i]}\n";
        }

        debugPanelText.text = debugText;
        Debug.Log(debugText);
    }

    // Shows the summary of completed steps and correctness
    private void ShowSummary(bool isCorrect)
    {
        summaryPanel.SetActive(true);

        string result = isCorrect ? "Correct Order!" : "Incorrect Order!";
        string summary = "Order of Steps Completed:\n";

        for (int i = 0; i < completedSteps.Count; i++)
        {
            summary += $"{i + 1}. {completedSteps[i]}\n";
        }

        summary += $"\nResult: {result}";
        summaryText.text = summary;
    }

    // Resets the level by clearing steps and hiding the summary
    private void ResetLevel()
    {
        completedSteps.Clear();
        summaryPanel.SetActive(false);
        DisplayDebugInfo();  // Clear debug info display
    }
}
