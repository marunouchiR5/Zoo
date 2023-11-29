using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WhaleArea : BaseView
{
    const string k_Whale = "whale";
    const string k_Navigation = "navigation";

    Button m_Whale;
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
        m_Whale = m_Screen.Q<Button>(k_Whale);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_Whale?.RegisterCallback<ClickEvent>(InteractWhale);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
    }

    private void InteractWhale(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        GameStateManager.Instance.SetActiveConversationData("WhaleArea", "Whale");
        m_GameViewManager.ShowConversationView();

        // state related
    }

    private void ClickNavigation(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        GameStateManager.Instance.SetActiveConversationData("WhaleArea", "Navigation");
        m_GameViewManager.ShowConversationView();

        // Set the delegate to handle option clicks
        m_GameViewManager.ConversationView.OnOptionClicked = HandleConversationOptionClick;
    }

    // conversation decision options
    private void HandleConversationOptionClick(DecisionOption option)
    {
        switch (option.Action)
        {
            case DecisionAction.Cancel:
                Cancel();
                break;
            case DecisionAction.GoToAquariumInside:
                GoToAquariumInside();
                break;
                // ... other cases as needed ...
        }
    }

    private void Cancel()
    {
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoToAquariumInside()
    {
        m_GameViewManager.ShowAquariumInside();
        m_GameViewManager.ConversationView.HideScreen();
    }

    public override void ShowScreen()
    {
        base.ShowScreen();
    }
}
