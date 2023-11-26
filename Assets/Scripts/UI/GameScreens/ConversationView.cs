using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConversationView : BaseView
{
    [Header("Option Asset")]
    [Tooltip("Option Asset to instantiate")]
    [SerializeField] VisualTreeAsset m_OptionAsset;

    // string IDs
    const string k_Options = "options";

    // locates elements to update
    const string k_ConversationArea = "conversation-area";
    const string k_Speaker = "speaker";
    const string k_LogButton = "log--button";

    // class names
    const string k_OptionClass = "option-button";

    Button m_ConversationArea;
    Label m_Speaker;
    Button m_LogButton;

    private ConversationData conversationData;
    private int currentElementIndex;

    public delegate void OptionClickHandler(DecisionOption option);
    public OptionClickHandler OnOptionClicked;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_ConversationArea = m_Screen.Q<Button>(k_ConversationArea);
        m_Speaker = m_Screen.Q<Label>(k_Speaker);
        m_LogButton = m_Screen.Q<Button>(k_LogButton);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_ConversationArea?.RegisterCallback<ClickEvent>(NextConversation);
        m_LogButton?.RegisterCallback<ClickEvent>(ShowLogView);
    }

    public override void ShowScreen()
    {
        ShowVisualElement(m_Screen, true);
        StartConversation();
    }

    public void StartConversation()
    {
        // Access the ConversationData from the GameStateManager
        if (GameStateManager.Instance.ConversationData != null)
        {
            conversationData = GameStateManager.Instance.ConversationData;
            currentElementIndex = 0;
            DisplayCurrentConversationElement();
        }
        else
        {
            Debug.LogError("GameStateManager instance is null");
        }
    }

    private void DisplayCurrentConversationElement()
    {
        if (conversationData != null && currentElementIndex < conversationData.Elements.Count)
        {
            var currentElement = conversationData.Elements[currentElementIndex];
            m_Speaker.text = currentElement.Speaker;
            m_ConversationArea.text = currentElement.Content;

            // Disable the conversationArea button (on the last element and having options)
            if (currentElementIndex == conversationData.Elements.Count - 1 && currentElement.Options.Count > 0)
            {
                m_ConversationArea.SetEnabled(false);
            }
            else
            {
                m_ConversationArea.SetEnabled(true);
            }
            

            // Remove existing option buttons if any
            RemoveExistingOptionButtons();

            // Check if it's a decision point
            if (currentElement.Options != null && currentElement.Options.Count > 0)
            {
                // Create new option buttons based on the current element's options
                foreach (var option in currentElement.Options)
                {
                    CreateOptionButton(option);
                }
            }
        }
    }

    // Helper method to remove existing option buttons
    private void RemoveExistingOptionButtons()
    {
        // Assuming you have a parent container for option buttons
        var optionButtonContainer = m_Screen.Q<VisualElement>(k_Options);
        optionButtonContainer.Clear();
    }

    // Helper method to create an option button
    private void CreateOptionButton(DecisionOption option)
    {
        var optionButtonContainer = m_Screen.Q<VisualElement>(k_Options);
        var optionButtonInstance = m_OptionAsset.Instantiate();

        if (optionButtonInstance is null)
        {
            Debug.LogError("OptionAsset is not set correctly in the inspector or failed to instantiate.");
            return;
        }

        // Assuming the first child of the cloned tree is the button. Adjust if your setup is different.
        if (optionButtonInstance.childCount > 0 && optionButtonInstance[0] is Button button)
        {
            button.text = option.Text; // Set the text of the button
            button.AddToClassList(k_OptionClass); // Add the class for styling
            button.RegisterCallback<ClickEvent>(evt => HandleOptionClick(option)); // Register the click event

            optionButtonContainer.Add(button); // Add the button to the container
        }
        else
        {
            Debug.LogError("OptionAsset does not contain a button as the first child.");
        }
    }

    // Handler for option button click
    private void HandleOptionClick(DecisionOption option)
    {
        OnOptionClicked?.Invoke(option);
    }

    private void NextConversation(ClickEvent evt)
    {
        currentElementIndex++;
        if (currentElementIndex >= conversationData.Elements.Count)
        {
            // End of conversation logic
            HideScreen();
        }
        else
        {
            DisplayCurrentConversationElement();
        }
    }

    private void ShowLogView(ClickEvent evt)
    {
        m_GameViewManager.ShowLogView();
    }
}
