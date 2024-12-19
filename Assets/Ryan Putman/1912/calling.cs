using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class calling : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textElement; // TextMeshPro element to update
    private string currentText = ""; // Holds the current text value

    // Method to add a digit to the TextMeshPro element
    public void AddDigit(string digit)
    {
        if (textElement != null)
        {
            currentText += digit;
            textElement.text = currentText;
        }
    }

    // Optional: Method to clear the text
    public void ClearText()
    {
        currentText = "";
        if (textElement != null)
        {
            textElement.text = currentText;
        }
    }
}
