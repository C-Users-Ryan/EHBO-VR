using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EHBOStappenChecker : MonoBehaviour
{
    [SerializeField] private List<string> correctOrder; // Correct sequence of steps
    private List<string> completedSteps = new List<string>(); // Tracks completed steps
    public TextMeshProUGUI debugPanelText;
    public GameObject summaryPanel;
    public TextMeshProUGUI summaryText;
    public Button resetButton;

    void Start()
    {
        summaryPanel.SetActive(false);
        resetButton.onClick.AddListener(ResetLevel);
    }

    public void RegisterStep(string stepName)
    {
        // Prevents consecutive duplicate actions
        if (completedSteps.Count == 0 || completedSteps[completedSteps.Count - 1] != stepName)
        {
            completedSteps.Add(stepName);
            DisplayDebugInfo();

            // Check if the completed steps match the required length
            if (completedSteps.Count == correctOrder.Count)
            {
                ValidateOrder();
            }
        }
        else
        {
            Debug.Log($"Action '{stepName}' ignored to prevent duplicate consecutive entry.");
        }
    }

    public void ValidateOrder()
    {
        bool isCorrect = true;

        // Compares completed steps to the correct order
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

    private void ShowSummary(bool isCorrect)
    {
        summaryPanel.SetActive(true);
        string result = isCorrect ? "Correct Order!" : "Incorrect Order!";

        string summary = "Order of Steps Completed:\n";
        for (int i = 0; i < completedSteps.Count; i++)
        {
            summary += $"{i + 1}. {completedSteps[i]}\n";
        }

        // If the order is incorrect, display the correct order as well
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

    private void ResetLevel()
    {
        completedSteps.Clear();
        summaryPanel.SetActive(false);
        DisplayDebugInfo();  // Clear debug display
    }
}
