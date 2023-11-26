using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameViewManager : MonoBehaviour
{
    // events
    public static event Action<string> LocationChanged;

    [Header("Scenes Views")]
    [Tooltip("Only one scene interface can appear on-screen at a time.")]
    [SerializeField] ZooGate m_ZooGate;
    [SerializeField] RabbitArea m_RabbitArea;
    [SerializeField] LionArea m_LionArea;
    [SerializeField] ElephantArea m_ElephantArea;
    [SerializeField] MonkeyArea m_MonkeyArea;

    [Header("HUDs")]
    [Tooltip("HUDs remain active at all times unless explicitly disabled.")]
    [SerializeField] Header m_Header;
    [SerializeField] HUD m_HUD;

    [Header("Full Main Game View Overlays")]
    [Tooltip("Full Main Game View Overlays block other controls in main-game-view until dismissed.")]
    [SerializeField] ConversationView m_ConversationView;
    [SerializeField] LogView m_LogView;
    [SerializeField] MapView m_MapView;
    [SerializeField] RulesView m_RulesView;
    [SerializeField] ItemInspectView m_ItemInspectView;

    List<BaseView> m_AllSceneViews = new List<BaseView>();
    List<BaseView> m_AllOverlayViews = new List<BaseView>();

    UIDocument m_GameViewDocument;
    public UIDocument GameViewDocument => m_GameViewDocument;

    // Public property to access the ConversationView
    public ConversationView ConversationView => m_ConversationView;

    void OnEnable()
    {
        m_GameViewDocument = GetComponent<UIDocument>();
        SetupSceneViews();
        SetupOverlayViews();

        // start new game or load previous game
        ShowZooGate();
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    void SetupSceneViews()
    {
        if (m_ZooGate != null)
            m_AllSceneViews.Add(m_ZooGate);

        if (m_RabbitArea != null)
            m_AllSceneViews.Add(m_RabbitArea);

        if (m_LionArea != null)
            m_AllSceneViews.Add(m_LionArea);

        if (m_ElephantArea != null)
            m_AllSceneViews.Add(m_ElephantArea);

        if (m_MonkeyArea != null)
            m_AllSceneViews.Add(m_MonkeyArea);
    }

    // shows one screen at a time
    void ShowSceneView(BaseView sceneView)
    {
        foreach (BaseView m in m_AllSceneViews)
        {
            if (m == sceneView)
            {
                m?.ShowScreen();
            }
            else
            {
                m?.HideScreen();
            }
        }
    }

    // scene view methods 
    public void ShowZooGate()
    {
        ShowSceneView(m_ZooGate);
        LocationChanged?.Invoke("Zoo Gate");
    }

    public void ShowRabbitArea()
    {
        ShowSceneView(m_RabbitArea);
        LocationChanged?.Invoke("Rabbit Area");
    }

    public void ShowLionArea()
    {
        ShowSceneView(m_LionArea);
        LocationChanged?.Invoke("Lion Area");
    }

    public void ShowElephantArea()
    {
        ShowSceneView(m_ElephantArea);
        LocationChanged?.Invoke("Elephant Area");
    }

    public void ShowMonkeyArea()
    {
        ShowSceneView(m_MonkeyArea);
        LocationChanged?.Invoke("Monkey Area");
    }

    void SetupOverlayViews()
    {
        if (m_ConversationView != null)
            m_AllOverlayViews.Add(m_ConversationView);

        if (m_LogView != null)
            m_AllOverlayViews.Add(m_LogView);

        if (m_MapView != null)
            m_AllOverlayViews.Add(m_MapView);

        if (m_RulesView != null)
            m_AllOverlayViews.Add(m_RulesView);

        if (m_ItemInspectView != null)
            m_AllOverlayViews.Add(m_ItemInspectView);
    }

    // shows one screen at a time
    void ShowOverlayView(BaseView overlayView)
    {
        foreach (BaseView m in m_AllOverlayViews)
        {
            if (m == overlayView)
            {
                m?.ShowScreen();
            }
            else
            {
                m?.HideScreen();
            }
        }
    }

    // overlay screen methods
    public void ShowConversationView()
    {
        ShowOverlayView(m_ConversationView);
    }

    public void ShowLogView()
    {
        // opening log view will not close other views
        m_LogView?.ShowScreen();
    }

    public void ShowMapView()
    {
        ShowOverlayView(m_MapView);
    }

    public void ShowRulesView()
    {
        ShowOverlayView(m_RulesView);
    }

    public void ShowItemInspectView()
    {
        ShowOverlayView(m_ItemInspectView);
    }
}
