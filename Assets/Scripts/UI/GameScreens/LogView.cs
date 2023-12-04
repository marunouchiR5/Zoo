using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LogView : BaseView
{
    // locates elements to update
    const string k_LogScrollView = "log-scroll-view";
    const string k_BackButton = "back--button";

    ScrollView m_LogScrollView;
    Button m_BackButton;

    private void OnEnable()
    {
        GameStateManager.RuleSetCollected += OnRuleSetCollected;
    }

    private void OnDisable()
    {
        GameStateManager.RuleSetCollected -= OnRuleSetCollected;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_LogScrollView = m_Screen.Q<ScrollView>(k_LogScrollView);
        m_BackButton = m_Screen.Q<Button>(k_BackButton);
    }

    public override void ShowScreen()
    {
        base.ShowScreen();

        RuleSet selectedRuleSet = GameStateManager.Instance.SelectedRuleSet;
        if (selectedRuleSet != null)
        {
            m_LogScrollView.Clear();

            // Add an empty element at the beginning for margin
            var topMarginElement = new VisualElement();
            topMarginElement.AddToClassList("margin-element");
            m_LogScrollView.Add(topMarginElement);

            var setNameLabel = new Label(selectedRuleSet.SetName);
            setNameLabel.AddToClassList("set-name");
            m_LogScrollView.Add(setNameLabel);

            VisualElement paragraphElement = null;
            string lastSpeaker = null;
            foreach (var element in selectedRuleSet.Elements)
            {
                if (lastSpeaker == null || element.Speaker != lastSpeaker)
                {
                    if (paragraphElement != null)
                    {
                        m_LogScrollView.Add(paragraphElement);
                    }

                    paragraphElement = new VisualElement();
                    paragraphElement.AddToClassList("paragraph-entry");

                    var speakerLabel = new Label(element.Speaker);
                    speakerLabel.AddToClassList("speaker-label");
                    paragraphElement.Add(speakerLabel);
                    lastSpeaker = element.Speaker;
                }

                var contentLabel = new Label(element.Content);
                contentLabel.AddToClassList("content-label");
                paragraphElement.Add(contentLabel);
            }

            if (paragraphElement != null)
            {
                m_LogScrollView.Add(paragraphElement);
            }

            // Add an empty element at the end for margin
            var bottomMarginElement = new VisualElement();
            bottomMarginElement.AddToClassList("margin-element");
            m_LogScrollView.Add(bottomMarginElement);
        }
        else
        {
            Debug.LogError("No RuleSet selected");
        }
    }

    protected override void RegisterButtonCallbacks()
    {
        m_BackButton?.RegisterCallback<ClickEvent>(HideLogView);
    }

    private void HideLogView(ClickEvent evt)
    {
        HideScreen();
    }

    // event-handling methods
    private void OnRuleSetCollected()
    {
        ShowScreen();
    }
}
