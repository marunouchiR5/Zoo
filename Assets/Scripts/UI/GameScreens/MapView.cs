using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapView : BaseView
{
    // locates elements to update
    const string k_BackButton = "back--button";

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
        m_BackButton = m_Screen.Q<Button>(k_BackButton);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_BackButton?.RegisterCallback<ClickEvent>(HideLogView);
    }

    private void HideLogView(ClickEvent evt)
    {
        HideScreen();
    }
}
