using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemInspectView : BaseView
{
    // events
    public static event Action ItemUsed;

    // locates elements to update
    const string k_ItemIcon = "item-icon";
    const string k_ItemName = "item-name";
    const string k_ItemDesc = "item-desc";
    const string k_CancelButton = "cancel--button";
    const string k_UseButton = "use--button";
    const string k_EquipButton = "equip--button";

    VisualElement m_ItemIcon;
    Label m_ItemName;
    Label m_ItemDesc;
    Button m_CancelButton;
    Button m_UseButton;
    Button m_EquipButton;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_ItemIcon = m_Screen.Q(k_ItemIcon);
        m_ItemName = m_Screen.Q<Label>(k_ItemName);
        m_ItemDesc = m_Screen.Q<Label>(k_ItemDesc);
        m_CancelButton = m_Screen.Q<Button>(k_CancelButton);
        m_UseButton = m_Screen.Q<Button>(k_UseButton);
        m_EquipButton = m_Screen.Q<Button>(k_EquipButton);
    }

    public override void ShowScreen()
    {
        base.ShowScreen();

        Item selectedItem = GameStateManager.Instance.SelectedItem;
        if (selectedItem != null)
        {
            // Set the item icon
            m_ItemIcon.style.backgroundImage = selectedItem.Icon;

            // Set the item name
            m_ItemName.text = selectedItem.Name;

            // Set the item description
            m_ItemDesc.text = selectedItem.Description;

            // Enable the use button if the item is usable
            m_UseButton.SetEnabled(selectedItem is IUsable);

            // Determine if the equip button should be enabled
            bool isEquipable = selectedItem is IEquipable;
            bool isCurrentlyEquipped = selectedItem == GameStateManager.Instance.CurrentHeadEquipment ||
                                       selectedItem == GameStateManager.Instance.CurrentBodyEquipment;
            m_EquipButton.SetEnabled(isEquipable && !isCurrentlyEquipped);
        }
    }

    protected override void RegisterButtonCallbacks()
    {
        m_CancelButton?.RegisterCallback<ClickEvent>(HideItemInspectView);
        m_UseButton?.RegisterCallback<ClickEvent>(UseItem);
        m_EquipButton?.RegisterCallback<ClickEvent>(EquipItem);
    }

    private void HideItemInspectView(ClickEvent evt)
    {
        HideScreen();
    }

    private void UseItem(ClickEvent evt)
    {
        // Perform use item logics here...
        Item selectedItem = GameStateManager.Instance.SelectedItem;
        if (selectedItem is IUsable usableItem)
        {
            usableItem.Use();
        }

        // Inform Inventory Display to update
        ItemUsed?.Invoke();

        // Close the inspect view after the item is used
        HideScreen();
    }

    private void EquipItem(ClickEvent evt)
    {
        // Perform use item logics here...
        Item selectedItem = GameStateManager.Instance.SelectedItem;
        if (selectedItem is IEquipable equipment)
        {
            equipment.Equip();
        }

        // Inform Inventory Display to update
        ItemUsed?.Invoke();

        // Close the inspect view after the item is used
        HideScreen();
    }
}
