using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : BaseView
{
    const string k_MapButton = "map--button";
    const string k_RulesButton = "rules--button";
    const string k_SettingsButton = "settings--button";

    Button m_MapButton;
    Button m_RulesButton;
    Button m_SettingsButton;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_MapButton = m_Screen.Q<Button>(k_MapButton);
        m_RulesButton = m_Screen.Q<Button>(k_RulesButton);
        m_SettingsButton = m_Screen.Q<Button>(k_SettingsButton);
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        m_MapButton?.RegisterCallback<ClickEvent>(ShowMapView);
        m_RulesButton?.RegisterCallback<ClickEvent>(ShowRulesView);
        m_SettingsButton?.RegisterCallback<ClickEvent>(ShowSettingsView);
    }

    private void ShowMapView(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        m_GameViewManager.ShowMapView();
    }

    private void ShowRulesView(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
        m_GameViewManager.ShowRulesView();
    }

    private void ShowSettingsView(ClickEvent evt)
    {
        Debug.Log(m_ScreenName + " " + evt.ToString());
    }
}
