using UnityEngine;
using TMPro;

public class TextUpdater : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    [SerializeField] private string variableName; // Set this in the Inspector or via code to match "Gold", "Coffee", etc.
    [SerializeField] public bool DisplayVarName = true; // Set this to toggle displaying the variable name

    private void Awake()
    {
        // Get the TextMeshPro component on the same GameObject
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // Subscribe to the variable update event
        GameEvents.OnVariableUpdated.AddListener(OnVariableUpdated);
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameEvents.OnVariableUpdated.RemoveListener(OnVariableUpdated);
    }

    // Callback function for the event, updating the text based on the variable name
    private void OnVariableUpdated(string updatedVariableName, int updatedValue)
    {
        if (updatedVariableName == variableName)
        {
            // Update the text based on the DisplayVarName flag
            if (DisplayVarName)
            {
                textMeshPro.text = updatedVariableName + ": " + updatedValue.ToString();
            }
            else
            {
                textMeshPro.text = updatedValue.ToString();
            }
        }
    }
}
