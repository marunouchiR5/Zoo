using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ZooGate : BaseView
{
    [Header("Rule Set to be Collected")]
    [SerializeField] RuleSet m_Rules;

    const string k_VisitorGuidelinesName = "visitor-guidelines";
    const string k_MapName = "map";
    const string k_NavigationName = "navigation";

    Button m_VisitorGuidelines;
    Button m_Map;
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
        m_VisitorGuidelines = m_Screen.Q<Button>(k_VisitorGuidelinesName);
        m_Map = m_Screen.Q<Button>(k_MapName);
        m_Navigation = m_Screen.Q<Button>(k_NavigationName);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_VisitorGuidelines?.RegisterCallback<ClickEvent>(ClickVisitorGuidelines);
        m_Map?.RegisterCallback<ClickEvent>(ClickMap);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
    }

    private void ClickVisitorGuidelines(ClickEvent evt)
    {
        if (!GameStateManager.Instance.CollectedRuleSets.Contains(m_Rules))
        {
            Debug.Log(m_ScreenName + " " + evt.ToString());

            GameStateManager.Instance.SetActiveConversationData("ZooGate", "VisitorGuidelines");
            m_GameViewManager.ShowConversationView();

            // ... item related code ...
            GameStateManager.Instance.AddRuleSet(m_Rules);
        }
        else
        {
            Debug.Log("Visitor Guidelines already collected.");
        }
    }

    private void ClickMap(ClickEvent evt)
    {
        if (!GameStateManager.Instance.ZooGateMapCollected)
        {
            Debug.Log(m_ScreenName + " " + evt.ToString());

            GameStateManager.Instance.SetActiveConversationData("ZooGate", "Map");
            m_GameViewManager.ShowConversationView();

            // mark the map as collected
            GameStateManager.Instance.ZooGateMapCollected = true;

            // item related
        }
        else
        {
            Debug.Log("Map already collected.");
        }
    }

    private void ClickNavigation(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        GameStateManager.Instance.SetActiveConversationData("ZooGate", "Navigation");
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

    public override void ShowScreen()
    {
        base.ShowScreen();
    }
}
