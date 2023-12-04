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
        // Check if GameStateManager instance is available
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("GameStateManager instance is not available.");
            return;
        }

        // Validate the location string
        if (string.IsNullOrEmpty(location))
        {
            Debug.LogWarning("Received an invalid location update.");
            return;
        }

        // Update the current location in GameStateManager
        GameStateManager.Instance.CurrentLocation = location;
        Debug.Log("Updated location in GameStateManager: " + location);

        // Update the UI if the label is available
        if (m_LocationLabel != null)
        {
            m_LocationLabel.text = location;
            Debug.Log("Location label updated in the UI.");
        }
        else
        {
            Debug.LogWarning("Location label in UI is not available for update.");
        }
    }
}
