using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Game Data/Item/Equipment")]
public class Equipment : Item, IEquipable
{
    public static event Action EquipmentChanged;

    public EquipmentType EquipmentType;

    public void Equip()
    {
        // Ensure the selected item is an equipment item
        if (GameStateManager.Instance.SelectedItem is Equipment selectedEquipment)
        {
            // Check the type of equipment and assign it to the corresponding slot
            switch (EquipmentType)
            {
                case EquipmentType.Head:
                    GameStateManager.Instance.CurrentHeadEquipment = selectedEquipment;
                    break;
                case EquipmentType.Body:
                    GameStateManager.Instance.CurrentBodyEquipment = selectedEquipment;
                    break;
                    // Add cases for other equipment types if necessary
            }

            EquipmentChanged?.Invoke();
        }
        else
        {
            Debug.LogError("Selected item is not an equipment.");
        }
    }
}

public enum EquipmentType
{
    Head,
    Body,
    // Add other types as needed
}
