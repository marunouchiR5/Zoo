using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Header : BaseView
{
    const string k_LocationLabel = "location";

    Label m_LocationLabel;

    private void OnEnable()
    {
        GameViewManager.LocationChanged += OnLocationChanged;
    }

    private void OnDisable()
    {
        GameViewManager.LocationChanged -= OnLocationChanged;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_LocationLabel = m_Screen.Q<Label>(k_LocationLabel);
    }

    // event-handling methods
    private void OnLocationChanged(string location)
    {
        if (m_LocationLabel != null)
        {
            m_LocationLabel.text = location;
        }
    }
}
