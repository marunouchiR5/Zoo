using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;

public class MonkeyArea : BaseView
{
    const string k_Background = "monkey-area";
    const string k_Monkey1 = "monkey1";
    const string k_Monkey2 = "monkey2";
    const string k_Navigation = "navigation";
    const string k_Staff = "staff";

    VisualElement m_Background;
    Button m_Monkey1;
    Button m_Monkey2;
    Button m_Navigation;
    Button m_Staff;

    private void OnEnable()
    {
        GameStateManager.BecameAware += OnBecameAware;
    }

    private void OnDisable()
    {
        GameStateManager.BecameAware -= OnBecameAware;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_Background = m_Screen.Q(k_Background);
        m_Monkey1 = m_Screen.Q<Button>(k_Monkey1);
        m_Monkey2 = m_Screen.Q<Button>(k_Monkey2);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
        m_Staff = m_Screen.Q<Button>(k_Staff);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_Monkey1?.RegisterCallback<ClickEvent>(InteractMonkey);
        m_Monkey2?.RegisterCallback<ClickEvent>(InteractMonkey);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
        m_Staff?.RegisterCallback<ClickEvent>(ClickStaff);
    }

    private void InteractMonkey(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        GameStateManager.Instance.SetActiveConversationData("MonkeyArea", "Monkey");
        m_GameViewManager.ShowConversationView();

        // state related
    }

    private void ClickNavigation(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        if (GameStateManager.Instance.Aware)
        {
            GameStateManager.Instance.SetActiveConversationData("MonkeyArea", "NavigationAware");
            m_GameViewManager.ShowConversationView();

            // Set the delegate to handle option clicks
            m_GameViewManager.ConversationView.OnOptionClicked = HandleAwareOptionClick;
        }
        else
        {
            GameStateManager.Instance.SetActiveConversationData("MonkeyArea", "Navigation");
            m_GameViewManager.ShowConversationView();

            // Set the delegate to handle option clicks
            m_GameViewManager.ConversationView.OnOptionClicked = HandleUnawareOptionClick;
        }
    }

    // conversation decision options
    private void HandleAwareOptionClick(DecisionOption option)
    {
        switch (option.Text)
        {
            case "Cancel":
                Cancel();
                break;
            case "Go Left":
                GoLeft();
                break;
            case "Go Right":
                GoRight();
                break;
                // ... other cases as needed ...
        }
    }

    private void HandleUnawareOptionClick(DecisionOption option)
    {
        switch (option.Text)
        {
            case "Cancel":
                Cancel();
                break;
            case "Go to Rabbit Area":
                GoToRabbitArea();
                break;
            case "Go to Lion Area":
                GoToLionArea();
                break;
            case "Go to Elephant Area":
                GoToElephantArea();
                break;
                // ... other cases as needed ...
        }
    }

    private void Cancel()
    {
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoToRabbitArea()
    {
        m_GameViewManager.ShowRabbitArea();
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoToLionArea()
    {
        m_GameViewManager.ShowLionArea();
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoToElephantArea()
    {
        m_GameViewManager.ShowElephantArea();
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoLeft()
    {
        // Generate a random number between 0 and 2 (inclusive)
        int randomNumber = Random.Range(0, 3);

        switch (randomNumber)
        {
            case 0:
                m_GameViewManager.ShowRabbitArea();
                break;
            case 1:
                m_GameViewManager.ShowLionArea();
                break;
            case 2:
                m_GameViewManager.ShowElephantArea();
                break;
        }

        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoRight()
    {
        if (GameStateManager.Instance.IsCurrentBodyEquipment("Black Clothes") && GameStateManager.Instance.MapCornerFed)
        {
            Debug.Log("Escaped!");
            m_GameViewManager.ShowGameFinalView();
        }
        else
        {
            // stay at monkey area
            // decrease sanity level
            int currentSanity = GameStateManager.Instance.CurrentSanity;
            GameStateManager.Instance.UpdateSanity(currentSanity - 1);
            m_GameViewManager.ConversationView.HideScreen();
        }
    }

    private void ClickStaff(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        GameStateManager.Instance.SetActiveConversationData("MonkeyArea", "Staff");
        m_GameViewManager.ShowConversationView();

        // state related
    }

    public override void ShowScreen()
    {
        base.ShowScreen();

        if (GameStateManager.Instance != null)
        {
            if (!GameStateManager.Instance.Aware && !GameStateManager.Instance.VisitedAreas.Contains(m_ScreenName))
            {
                GameStateManager.Instance.UpdateVisitedAreas(m_ScreenName);
            }
        }
    }

    // event-handling methods
    private void OnBecameAware()
    {
        // Load the new background image
        Texture2D newBackgroundTexture = Resources.Load<Texture2D>("UI/Textures/game design_monkey-04");
        if (newBackgroundTexture == null)
        {
            Debug.LogError("Failed to load new background texture.");
            return;
        }

        // Apply the new background image to m_Background
        m_Background.style.backgroundImage = new StyleBackground(newBackgroundTexture);
    }
}
