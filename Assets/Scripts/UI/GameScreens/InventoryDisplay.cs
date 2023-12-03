using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryDisplay : HUD
{
    [Header("Item Asset")]
    [Tooltip("Item Asset to instantiate")]
    [SerializeField] VisualTreeAsset m_ItemAsset;

    // string IDs
    const string k_InventoryList = "inventory-list";
    const string k_Token = "token";

    ListView m_InventoryList;
    Label m_Token;

    private void OnEnable()
    {
        GameStateManager.InventoryUpdated += OnInventoryUpdated;
        GameStateManager.TokenUpdated += OnTokenUpdated;

        ItemInspectView.ItemUsed += OnItemUsed;
    }

    private void OnDisable()
    {
        GameStateManager.InventoryUpdated -= OnInventoryUpdated;
        GameStateManager.TokenUpdated -= OnTokenUpdated;

        ItemInspectView.ItemUsed -= OnItemUsed;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_InventoryList = m_Screen.Q<ListView>(k_InventoryList);
        m_Token = m_Screen.Q<Label>(k_Token);
    }

    private void FillInventoryList(List<Item> inventoryList)
    {
        // Set up a make item function for a task entry
        m_InventoryList.makeItem = () =>
        {
            var newItemEntry = m_ItemAsset.Instantiate();

            // If you have a specific controller or logic for each task entry
            var newItemEntryLogic = new ItemEntryController();
            newItemEntry.userData = newItemEntryLogic;
            newItemEntryLogic.SetVisualElement(newItemEntry);

            return newItemEntry;
        };

        // Set up bind function for a specific task entry
        m_InventoryList.bindItem = (item, index) =>
        {
            if (index >= 0 && index < inventoryList.Count)
            {
                var itemEntryController = item.userData as ItemEntryController;
                itemEntryController.SetItemData(inventoryList[index], OpenItemInspectView);
            }
            else
            {
                Debug.LogError($"Invalid index: {index}. Inventory list size: {inventoryList.Count}");
            }
        };

        // Optionally set a fixed item height
        m_InventoryList.fixedItemHeight = 50;

        // Set the actual item's source list/array
        m_InventoryList.itemsSource = inventoryList;
    }

    private void OpenItemInspectView()
    {
        m_GameViewManager.ShowItemInspectView(); // Open the item inspect view
    }

    // event-handling methods
    private void OnInventoryUpdated()
    {
        FillInventoryList(GameStateManager.Instance.CurrentInventory);
    }

    private void OnTokenUpdated()
    {
        Debug.Log("OnTokenUpdated - current token: " + GameStateManager.Instance.CurrentToken.ToString());
        m_Token.text = "Token: " + GameStateManager.Instance.CurrentToken.ToString();
    }
    
    private void OnItemUsed()
    {
        // Retrieve the selected item
        Item selectedItem = GameStateManager.Instance.SelectedItem;

        // Check if the item is in the inventory and remove it if found
        if (GameStateManager.Instance.CurrentInventory.Contains(selectedItem))
        {
            GameStateManager.Instance.CurrentInventory.Remove(selectedItem);

            // Update the inventory
            FillInventoryList(GameStateManager.Instance.CurrentInventory);
        }

        // Optionally, clear the selected item in GameStateManager
        GameStateManager.Instance.SelectedItem = null;
    }
}
