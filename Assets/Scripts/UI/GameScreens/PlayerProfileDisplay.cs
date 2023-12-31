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
        GameStateManager.SanityChanged += OnPlayerProfileChanged;
        Equipment.EquipmentChanged += OnPlayerProfileChanged;
    }

    private void OnDisable()
    {
        GameStateManager.SanityChanged -= OnPlayerProfileChanged;
        Equipment.EquipmentChanged -= OnPlayerProfileChanged;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_PlayerProfile = m_Screen.Q(k_PlayerProfile);
    }

    // Event-handling method
    private void OnPlayerProfileChanged()
    {
        int currentSanity = GameStateManager.Instance.CurrentSanity;
        string profileImage = DetermineProfileImage(currentSanity);
        UpdatePlayerProfileImage(profileImage);
    }

    private string DetermineProfileImage(int currentSanity)
    {
        // Adjust the returned strings to match the names of your actual image assets
        if (GameStateManager.Instance.IsCurrentBodyEquipment("Black Clothes"))
        {
            switch (currentSanity)
            {
                case 4:
                    return "black"; // Sanity level 4 - Normal state
                case 3:
                    return "black+rabbitear"; // Sanity level 3 - Slightly stressed
                case 2:
                    return "rabbit+black"; // Sanity level 2 - Stressed
                case 1:
                    return "goat+black"; // Sanity level 1 - Very stressed
                default:
                    return "goat+black"; // In case of an unexpected value
            }
        }
        else
        {
            switch (currentSanity)
            {
                case 4:
                    return "unaware"; // Sanity level 4 - Normal state
                case 3:
                    return "rabbitear"; // Sanity level 3 - Slightly stressed
                case 2:
                    return "rabbithead"; // Sanity level 2 - Stressed
                case 1:
                    return "goathead"; // Sanity level 1 - Very stressed
                default:
                    return "goathead"; // In case of an unexpected value
            }
        }
    }

    private void UpdatePlayerProfileImage(string imageName)
    {
        // Assuming you have a method to fetch the correct image based on imageName
        Texture2D profileTexture = FetchProfileImage(imageName);
        if (profileTexture != null)
        {
            Debug.Log("Setting profile texture.");
            m_PlayerProfile.style.backgroundImage = new StyleBackground(profileTexture);
        }
    }

    private Texture2D FetchProfileImage(string imageName)
    {
        // Fetch the texture from your resources
        return Resources.Load<Texture2D>($"UI/Textures/elements/{imageName}");
    }
}
