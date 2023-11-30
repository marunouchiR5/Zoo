using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AquariumOutside : BaseView
{
    [Header("Rule Set to be Collected")]
    [SerializeField] RuleSet m_Rules;

    const string k_NoticeOutside = "notice-outside";
    const string k_BlackClothes = "black-clothes";
    const string k_NavigationEnter = "navigation-enter";
    const string k_Navigation = "navigation";

    Button m_NoticeOutside;
    Button m_BlackClothes;
    Button m_NavigationEnter;
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
        m_NoticeOutside = m_Screen.Q<Button>(k_NoticeOutside);
        m_BlackClothes = m_Screen.Q<Button>(k_BlackClothes);
        m_NavigationEnter = m_Screen.Q<Button>(k_NavigationEnter);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_NoticeOutside?.RegisterCallback<ClickEvent>(ClickNoticeOutside);
        m_BlackClothes?.RegisterCallback<ClickEvent>(ClickBlackClothes);
        m_NavigationEnter?.RegisterCallback<ClickEvent>(ClickNavigationEnter);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
    }

    private void ClickNoticeOutside(ClickEvent evt)
    {
        if (!GameStateManager.Instance.CollectedRuleSets.Contains(m_Rules))
        {
            Debug.Log(m_ScreenName + " " + evt.ToString());

            GameStateManager.Instance.SetActiveConversationData("AquariumOutside", "NoticeOutside");
            m_GameViewManager.ShowConversationView();

            // ... item related code ...
            GameStateManager.Instance.AddRuleSet(m_Rules);
        }
        else
        {
            Debug.Log("Notice Outside already collected.");
        }
    }

    private void ClickBlackClothes(ClickEvent evt)
    {
        if (!GameStateManager.Instance.BlackClothesCollected)
        {
            Debug.Log(m_ScreenName + " " + evt.ToString());

            GameStateManager.Instance.SetActiveConversationData("AquariumOutside", "BlackClothes");
            m_GameViewManager.ShowConversationView();

            // mark the map as collected
            GameStateManager.Instance.BlackClothesCollected = true;

            // item related
            // add black clothes to inventory
            string itemPath = $"GameData/Items/BlackClothes";
            GameStateManager.Instance.AddItem(itemPath);
        }
        else
        {
            Debug.Log("Black Clothes already collected.");
        }
    }

    private void ClickNavigationEnter(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        GameStateManager.Instance.SetActiveConversationData("AquariumOutside", "NavigationEnter");
        m_GameViewManager.ShowConversationView();

        // Set the delegate to handle option clicks
        m_GameViewManager.ConversationView.OnOptionClicked = HandleEnterConversationOptionClick;
    }

    private void ClickNavigation(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        GameStateManager.Instance.SetActiveConversationData("AquariumOutside", "Navigation");
        m_GameViewManager.ShowConversationView();

        // Set the delegate to handle option clicks
        m_GameViewManager.ConversationView.OnOptionClicked = HandleConversationOptionClick;
    }

    // conversation decision options
    private void HandleEnterConversationOptionClick(DecisionOption option)
    {
        switch (option.Text)
        {
            case "Cancel":
                Cancel();
                break;
            case "Go Inside":
                GoToAquariumInside();
                break;
                // ... other cases as needed ...
        }
    }

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

    private void GoToMonkeyArea()
    {
        m_GameViewManager.ShowMonkeyArea();
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
    }
}
