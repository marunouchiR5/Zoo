using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GuestRoom : BaseView
{
    [Header("Rule Set to be Collected")]
    [SerializeField] RuleSet m_Rules;

    const string k_GuestRoomNote = "note-guest-room";
    const string k_JellyfishLight = "jellyfish-light";
    const string k_Navigation = "navigation";

    Button m_GuestRoomNote;
    Button m_JellyfishLight;
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
        m_GuestRoomNote = m_Screen.Q<Button>(k_GuestRoomNote);
        m_JellyfishLight = m_Screen.Q<Button>(k_JellyfishLight);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_GuestRoomNote?.RegisterCallback<ClickEvent>(ClickGuestRoomNote);
        m_JellyfishLight?.RegisterCallback<ClickEvent>(ClickJellyfishLight);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
    }

    private void ClickGuestRoomNote(ClickEvent evt)
    {
        if (!GameStateManager.Instance.CollectedRuleSets.Contains(m_Rules))
        {
            Debug.Log(m_ScreenName + " " + evt.ToString());

            GameStateManager.Instance.SetActiveConversationData("GuestRoom", "GuestRoomNote");
            m_GameViewManager.ShowConversationView();

            // ... item related code ...
            GameStateManager.Instance.AddRuleSet(m_Rules);
        }
        else
        {
            Debug.Log("Guest Room Note already collected.");
        }
    }

    private void ClickJellyfishLight(ClickEvent evt)
    {
        if (!GameStateManager.Instance.JellyfishLightUsed)
        {
            Debug.Log(m_ScreenName + " " + evt.ToString());

            GameStateManager.Instance.SetActiveConversationData("GuestRoom", "JellyfishLight");
            m_GameViewManager.ShowConversationView();

            // mark the map as collected
            GameStateManager.Instance.JellyfishLightUsed = true;

            // item related
            int currentSanity = GameStateManager.Instance.CurrentSanity;
            GameStateManager.Instance.UpdateSanity(currentSanity + 1);
        }
        else
        {
            Debug.Log("Black Clothes already collected.");
        }
    }

    private void ClickNavigation(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        GameStateManager.Instance.SetActiveConversationData("GuestRoom", "Navigation");
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
