using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LionArea : BaseView
{
    // events
    public static event Action SceneScreenStarted;

    const string k_WhiteLion1 = "white-lion1";
    const string k_WhiteLion2 = "white-lion2";
    const string k_WhiteLion3 = "white-lion3";
    const string k_WhiteLion4 = "white-lion4";
    const string k_Navigation = "navigation";
    const string k_Staff = "staff";

    Button m_WhiteLion1;
    Button m_WhiteLion2;
    Button m_WhiteLion3;
    Button m_WhiteLion4;
    Button m_Navigation;
    Button m_Staff;

    private void OnEnable()
    {
        ShopView.StaffButtonClicked += OnClickStaff;
    }

    private void OnDisable()
    {
        ShopView.StaffButtonClicked -= OnClickStaff;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_WhiteLion1 = m_Screen.Q<Button>(k_WhiteLion1);
        m_WhiteLion2 = m_Screen.Q<Button>(k_WhiteLion2);
        m_WhiteLion3 = m_Screen.Q<Button>(k_WhiteLion3);
        m_WhiteLion4 = m_Screen.Q<Button>(k_WhiteLion4);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
        m_Staff = m_Screen.Q<Button>(k_Staff);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_WhiteLion1?.RegisterCallback<ClickEvent>(InteractWhiteLion);
        m_WhiteLion2?.RegisterCallback<ClickEvent>(InteractWhiteLion);
        m_WhiteLion3?.RegisterCallback<ClickEvent>(InteractWhiteLion);
        m_WhiteLion4?.RegisterCallback<ClickEvent>(InteractWhiteLion);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
        m_Staff?.RegisterCallback<ClickEvent>(OpenShop);
    }

    private void InteractWhiteLion(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        GameStateManager.Instance.SetActiveConversationData("LionArea", "WhiteLion");
        m_GameViewManager.ShowConversationView();

        // state related
    }

    private void ClickNavigation(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        if (!GameStateManager.Instance.Aware)
        {
            Task task = GameStateManager.Instance.CurrentTasks[0];
            if (task.Progress < task.ProgressGoal)
            {
                GameStateManager.Instance.SetActiveConversationData("LionArea", "NavigationUnawareUnfinished");
            }
            else
            {
                GameStateManager.Instance.SetActiveConversationData("LionArea", "NavigationUnawareFinished");
            }
        }
        else
        {
            GameStateManager.Instance.SetActiveConversationData("LionArea", "NavigationAware");
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
                    case DecisionAction.GoToRabbitArea:
                        GoToRabbitArea();
                        break;
                    case DecisionAction.GoToMonkeyArea:
                        GoToMonkeyArea();
                        break;
                    case DecisionAction.GoToElephantArea:
                        GoToElephantArea();
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
                    case DecisionAction.GoToRabbitArea:
                        GoToRabbitArea();
                        break;
                    case DecisionAction.GoToMonkeyArea:
                        GoToMonkeyArea();
                        break;
                    case DecisionAction.GoToElephantArea:
                        GoToElephantArea();
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
                case DecisionAction.GoToRabbitArea:
                    GoToRabbitArea();
                    break;
                case DecisionAction.GoToMonkeyArea:
                    GoToMonkeyArea();
                    break;
                case DecisionAction.GoToElephantArea:
                    GoToElephantArea();
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

    private void GoToRabbitArea()
    {
        m_GameViewManager.ShowRabbitArea();
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

    private void OpenShop(ClickEvent evt)
    {
        GameStateManager.Instance.SetCurrentShopInventoryId("LionArea");
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

    // event-handling methods
    private void OnClickStaff()
    {
        if (!GameStateManager.Instance.IsCurrentBodyEquipment("Black Clothes") && GameStateManager.Instance.Aware)
        {
            GameStateManager.Instance.SetActiveConversationData("LionArea", "StaffAware");
            m_GameViewManager.ShowConversationView();

            // Set the delegate to handle option clicks
            m_GameViewManager.ConversationView.OnOptionClicked = HandleAwareOptionClick;
        }
        else
        {
            GameStateManager.Instance.SetActiveConversationData("LionArea", "Staff");
            m_GameViewManager.ShowConversationView();
        }

        // state related
    }

    // conversation decision options
    private void HandleAwareOptionClick(DecisionOption option)
    {
        switch (option.Action)
        {
            case DecisionAction.Cancel:
                Cancel();
                break;
            case DecisionAction.Escape:
                Escape();
                break;
                // ... other cases as needed ...
        }
    }

    private void Escape()
    {
        // after showing conversation, show win screen
        Debug.Log("Escaped!");
        m_GameViewManager.ShowGameFinalView();
    }
}
