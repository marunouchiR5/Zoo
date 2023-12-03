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
        RabbitArea.SceneScreenStarted += OnSceneScreenStarted;
        MonkeyArea.SceneScreenStarted += OnSceneScreenStarted;
        ElephantArea.SceneScreenStarted += OnSceneScreenStarted;
        LionArea.SceneScreenStarted += OnSceneScreenStarted;
    }

    private void OnDisable()
    {
        RabbitArea.SceneScreenStarted -= OnSceneScreenStarted;
        MonkeyArea.SceneScreenStarted -= OnSceneScreenStarted;
        ElephantArea.SceneScreenStarted -= OnSceneScreenStarted;
        LionArea.SceneScreenStarted -= OnSceneScreenStarted;
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

    // event-handling methods

    private void OnSceneScreenStarted()
    {
        // Define the probability (50% chance in this example)
        float chance = 0.5f;

        // Generate a random number between 0.0 and 1.0
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);

        // Check if the random value is less than the defined chance
        if (randomValue < chance)
        {
            // The following code will run with the defined probability
            if (!GameStateManager.Instance.TicketSold && !GameStateManager.Instance.Aware)
            {
                GameStateManager.Instance.SetActiveConversationData("AllScene", "SellTicket");

                // Start the coroutine to delay showing the conversation view
                StartCoroutine(DelayedShowScreen(0.3f));  // Delay for 0.5 seconds, adjust as needed

                // Set the delegate to handle option clicks
                m_GameViewManager.ConversationView.OnOptionClicked = HandleConversationOptionClick;
            }
        }
        else
        {
            // Optionally, handle the scenario where the chance condition is not met
            // For example, log a message or perform an alternative action
            Debug.Log("Chance condition not met, skipping OnSceneScreenStarted actions.");
        }
    }


    // Coroutine to delay the showing of the conversation screen
    private IEnumerator DelayedShowScreen(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowScreen();
    }

    // conversation decision options
    private void HandleConversationOptionClick(DecisionOption option)
    {
        switch (option.Action)
        {
            case DecisionAction.Cancel:
                Cancel();
                break;
            case DecisionAction.BuyTicket:
                BuyTicket();
                break;
                // ... other cases as needed ...
        }
    }

    private void Cancel()
    {
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void BuyTicket()
    {
        if (GameStateManager.Instance.CurrentToken > 1)
        {
            GameStateManager.Instance.BuyItem("Ticket");
            GameStateManager.Instance.TicketSold = true;

            int currentSanity = GameStateManager.Instance.CurrentSanity;
            GameStateManager.Instance.UpdateSanity(currentSanity - 1);
        }
        m_GameViewManager.ConversationView.HideScreen();
    }
}
