using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapView : BaseView
{
    // locates elements to update
    const string k_MapCorner = "map-corner";
    const string k_BackButton = "back--button";

    Button m_MapCorner;
    Button m_BackButton;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_MapCorner = m_Screen.Q<Button>(k_MapCorner);
        m_BackButton = m_Screen.Q<Button>(k_BackButton);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_MapCorner?.RegisterCallback<ClickEvent>(TearMapCorner);
        m_BackButton?.RegisterCallback<ClickEvent>(HideMapView);
    }

    private void TearMapCorner(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());

        GameStateManager.Instance.SetActiveConversationData("Map", "TearMapCorner");
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
            case "Tear Map":
                TearMap();
                break;
                // ... other cases as needed ...
        }
    }

    private void Cancel()
    {
        m_GameViewManager.ConversationView.HideScreen();
    }

    private void TearMap()
    {
        // add map corner to inventory
        string itemPath = $"GameData/Items/MapCorner";
        GameStateManager.Instance.AddItem(itemPath);

        m_GameViewManager.ConversationView.HideScreen();
    }

    private void HideMapView(ClickEvent evt)
    {
        HideScreen();
    }
}
