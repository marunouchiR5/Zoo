using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUsableItem", menuName = "Game Data/Item/Usable Item")]
public class UsableItem : Item, IUsable
{
    public ItemType ItemType;
    public int EffectValue;

    public void Use()
    {
        // Implement use logic here
        int currentSanity = GameStateManager.Instance.CurrentSanity;
        if (ItemType == ItemType.Good)
        {
            GameStateManager.Instance.UpdateSanity(Mathf.Min(currentSanity + EffectValue, GameStateManager.MaxSanity));
        }
        else if (ItemType == ItemType.Bad)
        {
            GameStateManager.Instance.UpdateSanity(currentSanity - EffectValue);
        }
    }
}

public enum ItemType
{
    Good,
    Bad,
    // Add other types as needed
}