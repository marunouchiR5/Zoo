using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverView : BaseView
{
    // locates elements to update
    const string k_RestartButton = "restart";

    Button m_RestartButton;

    private void OnEnable()
    {
        GameStateManager.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameStateManager.GameOver -= OnGameOver;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_RestartButton = m_Screen.Q<Button>(k_RestartButton);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_RestartButton?.RegisterCallback<ClickEvent>(RestartGame);
    }

    private void RestartGame(ClickEvent evt)
    {
        HideScreen();
        m_GameViewManager.ShowZooGate();
        GameStateManager.Instance.StartNewGame();
    }

    // event-handling methods
    private void OnGameOver()
    {
        m_GameViewManager.ShowGameOverView();
    }
}
