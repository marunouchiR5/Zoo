using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameFinalView : BaseView
{
    // locates elements to update
    const string k_RestartButton = "restart";
    const string k_MainMenu = "main-menu";

    Button m_RestartButton;
    Button m_MainMenu;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_RestartButton = m_Screen.Q<Button>(k_RestartButton);
        m_MainMenu = m_Screen.Q<Button>(k_MainMenu);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_RestartButton?.RegisterCallback<ClickEvent>(RestartGame);
        m_MainMenu?.RegisterCallback<ClickEvent>(OpenMainMenu);
    }

    private void RestartGame(ClickEvent evt)
    {
        HideScreen();
        m_GameViewManager.ShowZooGate();
        GameStateManager.Instance.StartNewGame();
    }

    private void OpenMainMenu(ClickEvent evt)
    {
        SceneManager.LoadScene("MainMenu");
        m_GameViewManager.ShowZooGate();
        GameStateManager.Instance.StartNewGame();
    }
}
