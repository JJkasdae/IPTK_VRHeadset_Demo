using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPrompt : MonoBehaviour
{
    [SerializeField] private SessionData currentSceneData; // ScriptableObject holding scene information

    private Canvas UIDisplay; // Reference to the Canvas UI
    private TextMeshProUGUI textComponent; // Reference to the Text component on Canvas
    private Player player;
    private bool isDisplaying = false; // Tracks whether the prompt is visible

    // Start is called before the first frame update
    private void Start()
    {
        // Automatically find UIDisplay and textComponent within the children
        UIDisplay = GetComponentInChildren<Canvas>();
        textComponent = UIDisplay.GetComponentInChildren<TextMeshProUGUI>();

        if (UIDisplay != null)
        {
            UIDisplay.gameObject.SetActive(false); // Ensure the UI starts as hidden
        }
        UpdatePromptText();
    }

    // Method to initialize this prompt for the presenter
    public void InitializeForPresenter(Player presenter)
    {
        player = presenter;

        if (player.userType == PlayerType.Presenter)
        {
            // The player is the presenter, and this prompt can be displayed
            Debug.Log("Prompt initialized for Presenter.");
        }
        else
        {
            Debug.LogWarning("This prompt is for presenters only. Current player is not a presenter.");
            enabled = false; // Disable this script if the player is not a presenter
        }
    }

    // Toggle the prompt display on and off
    public void TogglePrompt()
    {
        isDisplaying = !isDisplaying;
        UIDisplay.gameObject.SetActive(isDisplaying);
    }

    // Update the text component with scene data or default text
    void UpdatePromptText()
    {
        if (currentSceneData != null)
        {
            // Assuming currentSceneData contains a field `description`
            textComponent.text = currentSceneData.Description;
        }
        else
        {
            // Use the default text in the Text component
            textComponent.text = textComponent.text;
        }
    }
}
