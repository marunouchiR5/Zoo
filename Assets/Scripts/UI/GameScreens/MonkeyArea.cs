using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonkeyArea : BaseView
{
    const string k_Monkey1 = "monkey1";
    const string k_Monkey2 = "monkey2";
    const string k_Navigation = "navigation";

    Button m_Monkey1;
    Button m_Monkey2;
    Button m_Navigation;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_Monkey1 = m_Screen.Q<Button>(k_Monkey1);
        m_Monkey2 = m_Screen.Q<Button>(k_Monkey2);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_Monkey1?.RegisterCallback<ClickEvent>(InteractMonkey);
        m_Monkey2?.RegisterCallback<ClickEvent>(InteractMonkey);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
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

        GameStateManager.Instance.SetActiveConversationData("MonkeyArea", "Navigation");
        m_GameViewManager.ShowConversationView();

        // Set the delegate to handle option clicks
        m_GameViewManager.ConversationView.OnOptionClicked = HandleConversationOptionClick;
    }

    // conversation decision options
    private void HandleConversationOptionClick(DecisionOption option)
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
}
