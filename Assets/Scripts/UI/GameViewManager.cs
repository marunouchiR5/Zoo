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
    [SerializeField] ZooDirectorRoom m_ZooDirectorRoom;
    [SerializeField] AquariumOutside m_AquariumOutside;
    [SerializeField] AquariumInside m_AquariumInside;
    [SerializeField] GuestRoom m_GuestRoom;
    [SerializeField] WhaleArea m_WhaleArea;

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
    [SerializeField] GameOverView m_GameOverView;
    [SerializeField] GameFinalView m_GameFinalView;
    [SerializeField] ShopView m_ShopView;

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

        if (m_ZooDirectorRoom != null)
            m_AllSceneViews.Add(m_ZooDirectorRoom);

        if (m_AquariumOutside != null)
            m_AllSceneViews.Add(m_AquariumOutside);

        if (m_AquariumInside != null)
            m_AllSceneViews.Add(m_AquariumInside);

        if (m_GuestRoom != null)
            m_AllSceneViews.Add(m_GuestRoom);

        if (m_WhaleArea != null)
            m_AllSceneViews.Add(m_WhaleArea);
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
        LocationChanged?.Invoke(m_ZooGate.GetScreenName());
    }

    public void ShowRabbitArea()
    {
        ShowSceneView(m_RabbitArea);
        LocationChanged?.Invoke(m_RabbitArea.GetScreenName());
    }

    public void ShowLionArea()
    {
        ShowSceneView(m_LionArea);
        LocationChanged?.Invoke(m_LionArea.GetScreenName());
    }

    public void ShowElephantArea()
    {
        ShowSceneView(m_ElephantArea);
        LocationChanged?.Invoke(m_ElephantArea.GetScreenName());
    }

    public void ShowMonkeyArea()
    {
        ShowSceneView(m_MonkeyArea);
        LocationChanged?.Invoke(m_MonkeyArea.GetScreenName());
    }

    public void ShowZooDirectorRoom()
    {
        ShowSceneView(m_ZooDirectorRoom);
        LocationChanged?.Invoke(m_ZooDirectorRoom.GetScreenName());
    }

    public void ShowAquariumOutside()
    {
        ShowSceneView(m_AquariumOutside);
        LocationChanged?.Invoke(m_AquariumOutside.GetScreenName());
    }

    public void ShowAquariumInside()
    {
        ShowSceneView(m_AquariumInside);
        LocationChanged?.Invoke(m_AquariumInside.GetScreenName());
    }

    public void ShowGuestRoom()
    {
        ShowSceneView(m_GuestRoom);
        LocationChanged?.Invoke(m_GuestRoom.GetScreenName());
    }

    public void ShowWhaleArea()
    {
        ShowSceneView(m_WhaleArea);
        LocationChanged?.Invoke(m_WhaleArea.GetScreenName());
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

        if (m_GameOverView != null)
            m_AllOverlayViews.Add(m_GameOverView);

        if (m_GameFinalView != null)
            m_AllOverlayViews.Add(m_GameFinalView);

        if (m_ShopView != null)
            m_AllOverlayViews.Add(m_ShopView);
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

    public void ShowGameOverView()
    {
        ShowOverlayView(m_GameOverView);
    }

    public void ShowGameFinalView()
    {
        ShowOverlayView(m_GameFinalView);
    }

    public void ShowShopView()
    {
        ShowOverlayView(m_ShopView);
    }
}
