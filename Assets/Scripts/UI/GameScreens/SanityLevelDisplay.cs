using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SanityLevelDisplay : HUD
{
    // events
    public static event Action GameOver;

    const string k_SanityLevel = "sanity-level";

    const string k_SanityLevelBackgroundFull = "sanity-level-box--full";
    const string k_SanityLevelBackgroundHalf = "sanity-level-box--half";

    VisualElement m_SanityLevel;

    private void OnEnable()
    {
        GameStateManager.SanityChanged += OnSanityLevelChanged;
    }

    private void OnDisable()
    {
        GameStateManager.SanityChanged -= OnSanityLevelChanged;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_SanityLevel = m_Screen.Q(k_SanityLevel);
    }

    // event-handling methods
    private void OnSanityLevelChanged()
    {
        int currentSanity = GameStateManager.Instance.CurrentSanity;

        for (int i = 0; i < m_SanityLevel.childCount; i++)
        {
            VisualElement child = m_SanityLevel[i];
            if (i < GameStateManager.MaxSanity - currentSanity)
            {
                child.RemoveFromClassList(k_SanityLevelBackgroundFull);
                child.AddToClassList(k_SanityLevelBackgroundHalf);
            }
            else
            {
                child.RemoveFromClassList(k_SanityLevelBackgroundHalf);
                child.AddToClassList(k_SanityLevelBackgroundFull);
            }
        }

        // game over when current sanity reaches zero
        if (currentSanity <= 0)
        {
            Debug.Log("Game Over - Player Sanity reached 0");
            GameOver?.Invoke();
            return;
        }
    }
}
