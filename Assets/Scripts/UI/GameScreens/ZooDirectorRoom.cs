using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ZooDirectorRoom : BaseView
{
    [Header("Rule Set to be Collected")]
    [SerializeField] RuleSet m_Rules;

    const string k_Map = "map";
    const string k_ZooDirectorDocument = "zoo-director-document";
    const string k_Navigation = "navigation";

    Button m_Map;
    Button m_ZooDirectorDocument;
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
        m_Map = m_Screen.Q<Button>(k_Map);
        m_ZooDirectorDocument = m_Screen.Q<Button>(k_ZooDirectorDocument);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_Map?.RegisterCallback<ClickEvent>(ClickMap);
        m_ZooDirectorDocument?.RegisterCallback<ClickEvent>(ClickZooDirectorDocument);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
    }

    private void ClickMap(ClickEvent evt)
    {
        if (!GameStateManager.Instance.ZooDirectorRoomMapCollected)
        {
            Debug.Log(m_ScreenName + " " + evt.ToString());

            GameStateManager.Instance.SetActiveConversationData("ZooDirectorRoom", "Map");
            m_GameViewManager.ShowConversationView();

            // mark the map as collected
            GameStateManager.Instance.ZooDirectorRoomMapCollected = true;

            // item related
        }
        else
        {
            Debug.Log("Map already collected.");
        }
    }

    private void ClickZooDirectorDocument(ClickEvent evt)
    {
        if (!GameStateManager.Instance.CollectedRuleSets.Contains(m_Rules))
        {
            Debug.Log(m_ScreenName + " " + evt.ToString());

            GameStateManager.Instance.SetActiveConversationData("ZooDirectorRoom", "ZooDirectorDocument");
            m_GameViewManager.ShowConversationView();

            // ... item related code ...
            GameStateManager.Instance.AddRuleSet(m_Rules);
        }
        else
        {
            Debug.Log("Zoo Director Document already collected.");
        }
    }

    private void ClickNavigation(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        GameStateManager.Instance.SetActiveConversationData("ZooDirectorRoom", "Navigation");
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
            case DecisionAction.GoToElephantArea:
                GoToElephantArea();
                break;
                // ... other cases as needed ...
        }
    }

    private void Cancel()
    {
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
