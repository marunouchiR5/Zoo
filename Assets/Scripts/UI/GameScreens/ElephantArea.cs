using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ElephantArea : BaseView
{
    [Header("Rule Set to be Collected")]
    [SerializeField] RuleSet m_Rules;

    const string k_Elephant = "elephant";
    const string k_Sign = "sign";
    const string k_SecurityRoomNote = "security-room-note";
    const string k_Navigation = "navigation";

    Button m_Elephant;
    Button m_Sign;
    Button m_SecurityRoomNote;
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
        m_Elephant = m_Screen.Q<Button>(k_Elephant);
        m_Sign = m_Screen.Q<Button>(k_Sign);
        m_SecurityRoomNote = m_Screen.Q<Button>(k_SecurityRoomNote);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_Elephant?.RegisterCallback<ClickEvent>(InteractElephant);
        m_Sign?.RegisterCallback<ClickEvent>(InteractSign);
        m_SecurityRoomNote?.RegisterCallback<ClickEvent>(ClickSecurityRoomNote);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
    }

    private void InteractElephant(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        GameStateManager.Instance.SetActiveConversationData("ElephantArea", "Elephant");
        m_GameViewManager.ShowConversationView();

        // state related
    }

    private void InteractSign(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        GameStateManager.Instance.SetActiveConversationData("ElephantArea", "Sign");
        m_GameViewManager.ShowConversationView();

        // state related
    }

    private void ClickSecurityRoomNote(ClickEvent evt)
    {
        if (!GameStateManager.Instance.CollectedRuleSets.Contains(m_Rules))
        {
            Debug.Log(m_ScreenName + " " + evt.ToString());

            GameStateManager.Instance.SetActiveConversationData("ElephantArea", "SecurityRoomNote");
            m_GameViewManager.ShowConversationView();

            // ... item related code ...
            GameStateManager.Instance.AddRuleSet(m_Rules);
        }
        else
        {
            Debug.Log("Security Room Note already collected.");
        }
    }

    private void ClickNavigation(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        GameStateManager.Instance.SetActiveConversationData("ElephantArea", "Navigation");
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
            case "Go to Monkey Area":
                GoToMonkeyArea();
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

    public override void ShowScreen()
    {
        base.ShowScreen();

        if (GameStateManager.Instance != null)
        {
            if (!GameStateManager.Instance.VisitedAreas.Contains(m_ScreenName))
            {
                GameStateManager.Instance.UpdateVisitedAreas(m_ScreenName);
            }
        }
    }
}
