using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RabbitArea : BaseView
{
    const string k_Bench = "bench";
    const string k_RabbitOutside = "rabbit1";
    const string k_RabbitInside1 = "rabbit2";
    const string k_RabbitInside2 = "rabbit3";
    const string k_RabbitInside3 = "rabbit4";
    const string k_Navigation = "navigation";

    Button m_Bench;
    Button m_RabbitOutside;
    Button m_RabbitInside1;
    Button m_RabbitInside2;
    Button m_RabbitInside3;
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
        m_Bench = m_Screen.Q<Button>(k_Bench);
        m_RabbitOutside = m_Screen.Q<Button>(k_RabbitOutside);
        m_RabbitInside1 = m_Screen.Q<Button>(k_RabbitInside1);
        m_RabbitInside2 = m_Screen.Q<Button>(k_RabbitInside2);
        m_RabbitInside3 = m_Screen.Q<Button>(k_RabbitInside3);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_Bench?.RegisterCallback<ClickEvent>(InteractBench);
        m_RabbitOutside?.RegisterCallback<ClickEvent>(InteractRabbitOutside);
        m_RabbitInside1?.RegisterCallback<ClickEvent>(InteractRabbitInside);
        m_RabbitInside2?.RegisterCallback<ClickEvent>(InteractRabbitInside);
        m_RabbitInside3?.RegisterCallback<ClickEvent>(InteractRabbitInside);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
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
        GameStateManager.Instance.SetActiveConversationData("RabbitArea", "RabbitInside");
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
                switch (option.Text)
                {
                    case "Cancel":
                        Cancel();
                        break;
                    case "Go to Monkey Area":
                        GoToMonkeyArea();
                        break;
                    case "Go to Lion Area":
                        GoToLionArea();
                        break;
                        // ... other cases as needed ...
                }
            }
            else
            {
                switch (option.Text)
                {
                    case "Cancel":
                        Cancel();
                        break;
                    case "Go to Monkey Area":
                        GoToMonkeyArea();
                        break;
                    case "Go to Lion Area":
                        GoToLionArea();
                        break;
                    case "Go to Zoo Gate":
                        GoToZooGate();
                        break;
                        // ... other cases as needed ...
                }
            }
        }
        else
        {
            switch (option.Text)
            {
                case "Cancel":
                    Cancel();
                    break;
                case "Go to Monkey Area":
                    GoToMonkeyArea();
                    break;
                case "Go to Lion Area":
                    GoToLionArea();
                    break;
                case "Go to Aquarium Entrance":
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
