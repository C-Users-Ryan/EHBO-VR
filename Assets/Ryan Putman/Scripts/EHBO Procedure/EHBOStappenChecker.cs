using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EHBOStappenChecker : MonoBehaviour
{
    [SerializeField] private List<string> correctOrder;      // Correct sequence of steps
    private List<string> completedSteps = new List<string>(); // Tracks completed steps
    [SerializeField] private TextMeshProUGUI debugPanelText; // For real-time debug display
    [SerializeField] private GameObject summaryPanel;       // Panel to display final summary
    [SerializeField] private TextMeshProUGUI summaryText;   // Summary display text


    void Start()
    {
        summaryPanel.SetActive(false);                       // Hide summary at start
        DisplayDebugInfo();                                  // Initialize debug display
    }

    // Method to register a completed step from another script
    public void RegisterStep(string stepName)
    {
        // Prevent consecutive duplicate steps
        if (completedSteps.Count == 0 || completedSteps[completedSteps.Count - 1] != stepName)
        {
            completedSteps.Add(stepName);
            DisplayDebugInfo();

            // Validate order if the final step in the correct sequence is reached
            if (stepName == "hart compressie" || completedSteps.Count == correctOrder.Count)
            {
                ValidateOrder();
            }
        }
        else
        {
            Debug.Log($"Action '{stepName}' ignored to prevent consecutive duplicate entry.");
        }
    }

    // Validates the order of completed steps against the correct order
    private void ValidateOrder()
    {
        bool isCorrect = true;

        // Compare each completed step with the correct order
        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (i >= completedSteps.Count || completedSteps[i] != correctOrder[i])
            {
                isCorrect = false;
                break;
            }
        }

        // Show the summary with feedback on the order
        ShowSummary(isCorrect);
    }

    // Display real-time debug information for completed steps
    private void DisplayDebugInfo()
    {
        string debugText = "Steps Completed:\n";
        for (int i = 0; i < completedSteps.Count; i++)
        {
            debugText += $"{i + 1}. {completedSteps[i]}\n";
        }
        debugPanelText.text = debugText;
        Debug.Log(debugText); // Output to console as well
    }

    // Display the summary panel with final results and comparison if incorrect
    private void ShowSummary(bool isCorrect)
    {
        summaryPanel.SetActive(true); // Show the summary panel

        string result = isCorrect ? "Correct Order!" : "Incorrect Order!";
        string summary = "Order of Steps Completed:\n";

        for (int i = 0; i < completedSteps.Count; i++)
        {
            summary += $"{i + 1}. {completedSteps[i]}\n";
        }

        // If incorrect, show the correct order as reference
        if (!isCorrect)
        {
            summary += "\nCorrect Order:\n";
            for (int i = 0; i < correctOrder.Count; i++)
            {
                summary += $"{i + 1}. {correctOrder[i]}\n";
            }
        }

        summary += $"\nResult: {result}";
        summaryText.text = summary;
    }

    // Resets the progress for retrying the exercise
    private void ResetLevel()
    {
        completedSteps.Clear();
        summaryPanel.SetActive(false);
        DisplayDebugInfo();  // Clear and refresh debug display
        Debug.Log("Exercise reset.");
    }
}
