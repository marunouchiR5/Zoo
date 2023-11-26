using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerProfileDisplay : HUD
{
    const string k_PlayerProfile = "player-profile";

    VisualElement m_PlayerProfile;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_PlayerProfile = m_Screen.Q(k_PlayerProfile);
    }

    // event-handling methods
    private void OnPlayerProfileChanged(string profileNumber)
    {
        // todo
        Debug.Log(profileNumber);
    }
}
