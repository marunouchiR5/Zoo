using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ElephantArea : BaseView
{
    [Header("Rule Set to be Collected")]
    [SerializeField] RuleSet m_Rules;

    const string k_Background = "elephant-area";
    const string k_Elephant = "elephant";
    const string k_Sign = "sign";
    const string k_SecurityRoomNote = "security-room-note";
    const string k_Navigation = "navigation";
    const string k_Staff = "staff";

    VisualElement m_Background;
    Button m_Elephant;
    Button m_Sign;
    Button m_SecurityRoomNote;
    Button m_Navigation;
    Button m_Staff;

    private void OnEnable()
    {
        GameStateManager.BecameAware += OnBecameAware;
        GameStateManager.NewGameStarted += OnNewGameStarted;
    }

    private void OnDisable()
    {
        GameStateManager.BecameAware -= OnBecameAware;
        GameStateManager.NewGameStarted -= OnNewGameStarted;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_Background = m_Screen.Q(k_Background);
        m_Elephant = m_Screen.Q<Button>(k_Elephant);
        m_Sign = m_Screen.Q<Button>(k_Sign);
        m_SecurityRoomNote = m_Screen.Q<Button>(k_SecurityRoomNote);
        m_Navigation = m_Screen.Q<Button>(k_Navigation);
        m_Staff = m_Screen.Q<Button>(k_Staff);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_Elephant?.RegisterCallback<ClickEvent>(InteractElephant);
        m_Sign?.RegisterCallback<ClickEvent>(InteractSign);
        m_SecurityRoomNote?.RegisterCallback<ClickEvent>(ClickSecurityRoomNote);
        m_Navigation?.RegisterCallback<ClickEvent>(ClickNavigation);
        m_Staff?.RegisterCallback<ClickEvent>(ClickStaff);
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

        if (GameStateManager.Instance.Aware)
        {
            GameStateManager.Instance.SetActiveConversationData("ElephantArea", "SignAware");
            m_GameViewManager.ShowConversationView();

            // state related
            int currentSanity = GameStateManager.Instance.CurrentSanity;
            GameStateManager.Instance.UpdateSanity(currentSanity - 1);
        }
        else
        {
            GameStateManager.Instance.SetActiveConversationData("ElephantArea", "Sign");
            m_GameViewManager.ShowConversationView();
        }
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
            case DecisionAction.GoToZooDirectorRoom:
                GoToZooDirectorRoom();
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

    private void GoToZooDirectorRoom()
    {
        m_GameViewManager.ShowZooDirectorRoom();
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void ClickStaff(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        GameStateManager.Instance.SetActiveConversationData("ElephantArea", "Staff");
        m_GameViewManager.ShowConversationView();

        // state related
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

    // event-handling methods
    private void OnBecameAware()
    {
        // Load the new background image
        Texture2D newBackgroundTexture = Resources.Load<Texture2D>("UI/Textures/background/game design_elephant-07");
        if (newBackgroundTexture == null)
        {
            Debug.LogError("Failed to load new background texture.");
            return;
        }
        // Apply the new background image to m_Background
        m_Background.style.backgroundImage = new StyleBackground(newBackgroundTexture);

        // Load the new elephant image
        Texture2D newElephantTexture = Resources.Load<Texture2D>("UI/Textures/elements/elephant_rabbit_ear");
        if (newElephantTexture == null)
        {
            Debug.LogError("Failed to load new elephant texture.");
            return;
        }
        // Apply the new background image to m_Elephant
        m_Elephant.style.backgroundImage = new StyleBackground(newElephantTexture);

        // Load the new sign image
        Texture2D newSignTexture = Resources.Load<Texture2D>("UI/Textures/elements/sign_rabbit_elephant");
        if (newSignTexture == null)
        {
            Debug.LogError("Failed to load new sign texture.");
            return;
        }
        // Apply the new background image to m_Sign
        m_Sign.style.backgroundImage = new StyleBackground(newSignTexture);
    }

    private void OnNewGameStarted()
    {
        // Load the new background image
        Texture2D newBackgroundTexture = Resources.Load<Texture2D>("UI/Textures/background/game design_elephant-06");
        if (newBackgroundTexture == null)
        {
            Debug.LogError("Failed to load new background texture.");
            return;
        }
        // Apply the new background image to m_Background
        m_Background.style.backgroundImage = new StyleBackground(newBackgroundTexture);

        // Load the new elephant image
        Texture2D newElephantTexture = Resources.Load<Texture2D>("UI/Textures/elements/elephant");
        if (newElephantTexture == null)
        {
            Debug.LogError("Failed to load new elephant texture.");
            return;
        }
        // Apply the new background image to m_Elephant
        m_Elephant.style.backgroundImage = new StyleBackground(newElephantTexture);

        // Load the new sign image
        Texture2D newSignTexture = Resources.Load<Texture2D>("UI/Textures/elements/sign_normal_elephant");
        if (newSignTexture == null)
        {
            Debug.LogError("Failed to load new sign texture.");
            return;
        }
        // Apply the new background image to m_Sign
        m_Sign.style.backgroundImage = new StyleBackground(newSignTexture);
    }
}
