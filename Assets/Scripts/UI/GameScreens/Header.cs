using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Header : BaseView
{
    const string k_LocationLabel = "location";

    Label m_LocationLabel;

    private Coroutine blinkCoroutine;

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

        // Start or stop blinking effect based on location
        if (location == "Whale Area")
        {
            if (blinkCoroutine == null)
            {
                blinkCoroutine = StartCoroutine(BlinkTextEffect());
            }
        }
        else
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
                m_LocationLabel.style.display = DisplayStyle.Flex; // Ensure label is visible when not in Whale Area
            }
        }
    }

    private IEnumerator BlinkTextEffect()
    {
        string[] locations = new string[] { "Elephant Area", "Whale Area" }; // Possible location labels

        while (true)
        {
            // Show the label
            m_LocationLabel.style.display = DisplayStyle.Flex;
            // Wait for a random time interval while the label is visible
            yield return new WaitForSeconds(Random.Range(0.2f, 1.0f));

            // Hide the label
            m_LocationLabel.style.display = DisplayStyle.None;
            // Wait for a random time interval while the label is hidden
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));

            // Randomly change the label text
            m_LocationLabel.text = locations[Random.Range(0, locations.Length)];
        }
    }
}
