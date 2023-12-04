using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RabbitArea : BaseView
{
    // events
    public static event Action SceneScreenStarted;

    const string k_Bench = "bench";
    const string k_RabbitOutside = "rabbit1";
    const string k_RabbitInside1 = "rabbit2";
    const string k_RabbitInside2 = "rabbit3";
    const string k_RabbitInside3 = "rabbit4";
    const string k_Navigation = "navigation";
    const string k_Staff = "staff";

    Button m_Bench;
    Button m_RabbitOutside;
    Button m_RabbitInside1;
    Button m_RabbitInside2;
    Button m_RabbitInside3;
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
        m_Bench = m_Screen.Q<Button>(k_Bench);
        m_RabbitOutside = m_Screen.Q<Button>(k_RabbitOutside);
        m_RabbitInside1 = m_Screen.Q<Button>(k_RabbitInside1);
        m_RabbitInside2 = m_Screen.Q<Button>(k_RabbitInside2);
        m_RabbitInside3 = m_Screen.Q<Button>(k_RabbitInside3);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
        m_Staff = m_Screen.Q<Button>(k_Staff);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_Bench?.RegisterCallback<ClickEvent>(InteractBench);
        m_RabbitOutside?.RegisterCallback<ClickEvent>(InteractRabbitOutside);
        m_RabbitInside1?.RegisterCallback<ClickEvent>(InteractRabbitInside);
        m_RabbitInside2?.RegisterCallback<ClickEvent>(InteractRabbitInside);
        m_RabbitInside3?.RegisterCallback<ClickEvent>(InteractRabbitInside);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
        m_Staff?.RegisterCallback<ClickEvent>(ClickStaff);
    }

    private void InteractBench(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        GameStateManager.Instance.SetActiveConversationData("RabbitArea", "Bench");
        m_GameViewManager.ShowConversationView();

        // state related
        int currentSanity = GameStateManager.Instance.CurrentSanity;
        GameStateManager.Instance.UpdateSanity(currentSanity - 1);
    }

    private void InteractRabbitOutside(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        GameStateManager.Instance.SetActiveConversationData("RabbitArea", "RabbitOutside");
        m_GameViewManager.ShowConversationView();

        // state related
        int currentSanity = GameStateManager.Instance.CurrentSanity;
        GameStateManager.Instance.UpdateSanity(currentSanity - 1);
    }

    private void InteractRabbitInside(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        if (GameStateManager.Instance.IsCurrentBodyEquipment("Black Clothes") && GameStateManager.Instance.Aware && GameStateManager.Instance.HasItem("MapCorner"))
        {
            GameStateManager.Instance.SetActiveConversationData("RabbitArea", "RabbitInsideCanFeed");

            // Set the delegate to handle option clicks
            m_GameViewManager.ConversationView.OnOptionClicked = HandleFeedRabbitOptionClick;
        }
        else
        {
            GameStateManager.Instance.SetActiveConversationData("RabbitArea", "RabbitInside");
        }
        
        m_GameViewManager.ShowConversationView();

        // state related
    }

    private void HandleFeedRabbitOptionClick(DecisionOption option)
    {
        switch (option.Action)
        {
            case DecisionAction.Cancel:
                Cancel();
                break;
            case DecisionAction.FeedMapCorner:
                FeedMapCorner();
                break;
                // ... other cases as needed ...
        }
    }

    private void ClickNavigation(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        if (!GameStateManager.Instance.Aware)
        {
            Task task = GameStateManager.Instance.CurrentTasks[0];
            if (task.Progress < task.ProgressGoal)
            {
                GameStateManager.Instance.SetActiveConversationData("RabbitArea", "NavigationUnawareUnfinished");
            }
            else
            {
                GameStateManager.Instance.SetActiveConversationData("RabbitArea", "NavigationUnawareFinished");
            }
        }
        else
        {
            GameStateManager.Instance.SetActiveConversationData("RabbitArea", "NavigationAware");
        }
        
        m_GameViewManager.ShowConversationView();

        // Set the delegate to handle option clicks
        m_GameViewManager.ConversationView.OnOptionClicked = HandleConversationOptionClick;
    }

    // conversation decision options
    private void HandleConversationOptionClick(DecisionOption option)
    {
        if (!GameStateManager.Instance.Aware)
        {
            Task task = GameStateManager.Instance.CurrentTasks[0];
            if (task.Progress < task.ProgressGoal)
            {
                switch (option.Action)
                {
                    case DecisionAction.Cancel:
                        Cancel();
                        break;
                    case DecisionAction.GoToMonkeyArea:
                        GoToMonkeyArea();
                        break;
                    case DecisionAction.GoToLionArea:
                        GoToLionArea();
                        break;
                        // ... other cases as needed ...
                }
            }
            else
            {
                switch (option.Action)
                {
                    case DecisionAction.Cancel:
                        Cancel();
                        break;
                    case DecisionAction.GoToMonkeyArea:
                        GoToMonkeyArea();
                        break;
                    case DecisionAction.GoToLionArea:
                        GoToLionArea();
                        break;
                    case DecisionAction.GoToZooGate:
                        GoToZooGate();
                        break;
                        // ... other cases as needed ...
                }
            }
        }
        else
        {
            switch (option.Action)
            {
                case DecisionAction.Cancel:
                    Cancel();
                    break;
                case DecisionAction.GoToMonkeyArea:
                    GoToMonkeyArea();
                    break;
                case DecisionAction.GoToLionArea:
                    GoToLionArea();
                    break;
                case DecisionAction.GoToAquariumOutside:
                    GoToAquariumOutside();
                    break;
                    // ... other cases as needed ...
            }
        }
    }

    private void Cancel()
    {
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoToMonkeyArea()
    {
        m_GameViewManager.ShowMonkeyArea();
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoToLionArea()
    {
        m_GameViewManager.ShowLionArea();
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoToAquariumOutside()
    {
        m_GameViewManager.ShowAquariumOutside();
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void GoToZooGate()
    {
        m_GameViewManager.ShowZooGate();
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void FeedMapCorner()
    {
        // use map corner in inventory
        GameStateManager.Instance.RemoveItem("MapCorner");
        GameStateManager.Instance.MapCornerFed = true;

        m_GameViewManager.ConversationView.HideScreen();
    }

    private void ClickStaff(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        GameStateManager.Instance.SetActiveConversationData("RabbitArea", "Staff");
        m_GameViewManager.ShowConversationView();

        // Set the delegate to handle option clicks
        m_GameViewManager.ConversationView.OnOptionClicked = HandleStaffOptionClick;
    }

    // conversation decision options
    private void HandleStaffOptionClick(DecisionOption option)
    {
        switch (option.Action)
        {
            case DecisionAction.Cancel:
                Cancel();
                break;
            case DecisionAction.OpenShop:
                OpenShop();
                break;
                // ... other cases as needed ...
        }
    }

    private void OpenShop()
    {
        GameStateManager.Instance.SetCurrentShopInventoryId("RabbitArea");
        m_GameViewManager.ShowShopView();
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

        SceneScreenStarted?.Invoke();
    }
}
