using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AquariumInside : BaseView
{
    [Header("Rule Set to be Collected")]
    [SerializeField] RuleSet m_Rules;

    const string k_NoticeInside = "notice-inside";
    const string k_GuestRoomDoor = "guest-room-door";
    const string k_Jellyfish1 = "jellyfish1";
    const string k_Jellyfish2 = "jellyfish2";
    const string k_Jellyfish3 = "jellyfish3";
    const string k_Jellyfish4 = "jellyfish4";
    const string k_Jellyfish5 = "jellyfish5";
    const string k_Navigation = "navigation";
    const string k_Staff = "staff";

    Button m_NoticeInside;
    Button m_GuestRoomDoor;
    Button m_Jellyfish1;
    Button m_Jellyfish2;
    Button m_Jellyfish3;
    Button m_Jellyfish4;
    Button m_Jellyfish5;
    Button m_Navigation;
    Button m_Staff;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_NoticeInside = m_Screen.Q<Button>(k_NoticeInside);
        m_GuestRoomDoor = m_Screen.Q<Button>(k_GuestRoomDoor);
        m_Jellyfish1 = m_Screen.Q<Button>(k_Jellyfish1);
        m_Jellyfish2 = m_Screen.Q<Button>(k_Jellyfish2);
        m_Jellyfish3 = m_Screen.Q<Button>(k_Jellyfish3);
        m_Jellyfish4 = m_Screen.Q<Button>(k_Jellyfish4);
        m_Jellyfish5 = m_Screen.Q<Button>(k_Jellyfish5);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
        m_Staff = m_Screen.Q<Button>(k_Staff);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_NoticeInside?.RegisterCallback<ClickEvent>(ClickNoticeInside);
        m_GuestRoomDoor?.RegisterCallback<ClickEvent>(ClickGuestRoomDoor);
        m_Jellyfish1?.RegisterCallback<ClickEvent>(InteractJellyfish);
        m_Jellyfish2?.RegisterCallback<ClickEvent>(InteractJellyfish);
        m_Jellyfish3?.RegisterCallback<ClickEvent>(InteractJellyfish);
        m_Jellyfish4?.RegisterCallback<ClickEvent>(InteractJellyfish);
        m_Jellyfish5?.RegisterCallback<ClickEvent>(InteractJellyfish);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
        m_Staff?.RegisterCallback<ClickEvent>(ClickStaff);
    }

    private void ClickNoticeInside(ClickEvent evt)
    {
        if (!GameStateManager.Instance.CollectedRuleSets.Contains(m_Rules))
        {
            Debug.Log(m_ScreenName + " " + evt.ToString());

            GameStateManager.Instance.SetActiveConversationData("AquariumInside", "NoticeInside");
            m_GameViewManager.ShowConversationView();

            // ... item related code ...
            GameStateManager.Instance.AddRuleSet(m_Rules);
        }
        else
        {
            Debug.Log("Guest Note already collected.");
        }
    }

    private void ClickGuestRoomDoor(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        GameStateManager.Instance.SetActiveConversationData("AquariumInside", "GuestRoomDoor");
        m_GameViewManager.ShowConversationView();

        // Set the delegate to handle option clicks
        m_GameViewManager.ConversationView.OnOptionClicked = HandleGuestRoomDoorOptionClick;
    }

    private void InteractJellyfish(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        GameStateManager.Instance.SetActiveConversationData("AquariumInside", "Jellyfish");
        m_GameViewManager.ShowConversationView();

        // state related
    }

    private void ClickNavigation(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        GameStateManager.Instance.SetActiveConversationData("AquariumInside", "Navigation");
        m_GameViewManager.ShowConversationView();

        // Set the delegate to handle option clicks
        m_GameViewManager.ConversationView.OnOptionClicked = HandleConversationOptionClick;
    }

    // conversation decision options
    private void HandleGuestRoomDoorOptionClick(DecisionOption option)
    {
        switch (option.Action)
        {
            case DecisionAction.Cancel:
                Cancel();
                break;
            case DecisionAction.GoToGuestRoom:
                GoToGuestRoom();
                break;
                // ... other cases as needed ...
        }
    }

    private void HandleConversationOptionClick(DecisionOption option)
    {
        switch (option.Action)
        {
            case DecisionAction.Cancel:
                Cancel();
                break;
            case DecisionAction.GoToAquariumOutside:
                GoToAquariumOutside();
                break;
            case DecisionAction.GoToWhaleArea:
                GoToWhaleArea();
                break;
                // ... other cases as needed ...
        }
    }

    private void Cancel()
    {
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoToGuestRoom()
    {
        m_GameViewManager.ShowGuestRoom();
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoToAquariumOutside()
    {
        m_GameViewManager.ShowAquariumOutside();
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoToWhaleArea()
    {
        m_GameViewManager.ShowWhaleArea();
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void ClickStaff(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        //GameStateManager.Instance.SetActiveConversationData("AquariumInside", "Staff");
        //m_GameViewManager.ShowConversationView();

        GameStateManager.Instance.SetCurrentShopInventoryId("AquariumInside");
        m_GameViewManager.ShowShopView();
        // state related
    }

    public override void ShowScreen()
    {
        base.ShowScreen();
    }
}
