using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopView : BaseView
{
    // events
    public static event Action StaffButtonClicked;

    [Header("Shop Items")]
    [SerializeField] VisualTreeAsset m_ShopItemAsset;

    // UI elements
    const string k_ItemList = "item-list-view";
    const string k_ItemIcon = "item-icon";
    const string k_ItemName = "item-name";
    const string k_ItemDesc = "item-desc";
    const string k_MessageLabel = "message--label";
    const string k_SubtractButton = "subtract--button";
    const string k_ConfirmButton = "confirm--button";
    const string k_AddButton = "add--button";
    const string k_WantAllButton = "want-all--button";
    const string k_CancelButton = "cancel--button";
    const string k_StaffButton = "staff--button";

    ListView m_ItemList;
    VisualElement m_ItemIcon;
    Label m_ItemName;
    Label m_ItemDesc;
    Label m_MessageLabel;
    Button m_SubtractButton;
    Button m_ConfirmButton;
    Button m_AddButton;
    Button m_WantAllButton;
    Button m_CancelButton;
    Button m_StaffButton;

    private Item currentShopItem; // The currently selected item in the shop
    private int currentAmount = 1; // Default amount

    private void OnEnable()
    {
        // Event subscriptions if needed
    }

    private void OnDisable()
    {
        // Event unsubscriptions if needed
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_ItemList = m_Screen.Q<ListView>(k_ItemList);
        m_ItemIcon = m_Screen.Q<VisualElement>(k_ItemIcon);
        m_ItemName = m_Screen.Q<Label>(k_ItemName);
        m_ItemDesc = m_Screen.Q<Label>(k_ItemDesc);
        m_MessageLabel = m_Screen.Q<Label>(k_MessageLabel);
        m_SubtractButton = m_Screen.Q<Button>(k_SubtractButton);
        m_ConfirmButton = m_Screen.Q<Button>(k_ConfirmButton);
        m_AddButton = m_Screen.Q<Button>(k_AddButton);
        m_WantAllButton = m_Screen.Q<Button>(k_WantAllButton);
        m_CancelButton = m_Screen.Q<Button>(k_CancelButton);
        m_StaffButton = m_Screen.Q<Button>(k_StaffButton);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_SubtractButton?.RegisterCallback<ClickEvent>(evt => AdjustAmount(-1));
        m_AddButton?.RegisterCallback<ClickEvent>(evt => AdjustAmount(1));
        m_ConfirmButton?.RegisterCallback<ClickEvent>(ConfirmPurchase);
        m_WantAllButton?.RegisterCallback<ClickEvent>(BuyAllItems);
        m_CancelButton?.RegisterCallback<ClickEvent>(CloseShopView);
        m_StaffButton?.RegisterCallback<ClickEvent>(evt => StaffButtonClicked?.Invoke());
    }

    private void UpdateItemDetailsUI(Item item)
    {
        // Set the currentShopItem
        currentShopItem = item;

        // Update the item icon, name, and description
        m_ItemIcon.style.backgroundImage = item.Icon;
        m_ItemName.text = item.Name;
        m_ItemDesc.text = item.Description;

        // Update the message label to include the item name and a prompt for the quantity
        m_MessageLabel.text = $"{item.Name}?\nHow many do you desire?";

        // Reset currentAmount to default value and update the confirmation button text
        currentAmount = 1;
        m_ConfirmButton.text = $"{currentAmount}"; // Update this string as per your UI's format
    }

    private void FillItemList()
    {
        List<ItemEntry> currentShopInventory = GameStateManager.Instance.GetCurrentShopInventory();

        m_ItemList.makeItem = () =>
        {
            var newItemEntry = m_ShopItemAsset.Instantiate();
            var newItemEntryController = new ShopItemEntryController();
            newItemEntry.userData = newItemEntryController;
            newItemEntryController.SetVisualElement(newItemEntry);
            return newItemEntry;
        };

        m_ItemList.bindItem = (item, index) =>
        {
            if (index >= 0 && index < currentShopInventory.Count)
            {
                var itemEntryController = item.userData as ShopItemEntryController;
                int stock = GetItemStock(currentShopInventory[index].item);
                itemEntryController.SetItemData(currentShopInventory[index].item, stock);

                // Add click event listener to update the item details UI
                item.RegisterCallback<ClickEvent>(evt => UpdateItemDetailsUI(currentShopInventory[index].item));
            }
            else
            {
                Debug.LogError($"Invalid index: {index}. Inventory list size: {currentShopInventory.Count}");
            }
        };

        m_ItemList.fixedItemHeight = 80;
        m_ItemList.itemsSource = currentShopInventory;
    }

    private int GetItemStock(Item item)
    {
        if (GameStateManager.Instance.CopiedShopInventories.TryGetValue(GameStateManager.Instance.currentShopInventoryId, out var shopInventory))
        {
            foreach (var itemEntry in shopInventory)
            {
                if (itemEntry.item == item)
                {
                    return itemEntry.quantity;
                }
            }
        }

        Debug.LogWarning($"Stock for item {item.Name} not found in current shop inventory.");
        return 0; // Return 0 or handle as needed
    }

    private void AdjustAmount(int change)
    {
        int newAmount = currentAmount + change;

        // Check if newAmount is within the stock limit
        int stockLimit = GetItemStock(currentShopItem);
        if (newAmount > stockLimit)
        {
            Debug.Log("Not enough stock available.");
            return;
        }

        // Check if total price does not exceed player's tokens
        int totalPrice = newAmount * currentShopItem.Price;
        if (totalPrice > GameStateManager.Instance.CurrentToken)
        {
            Debug.Log("Not enough tokens.");
            return;
        }

        // Update currentAmount and the confirmation button text
        currentAmount = Mathf.Max(1, newAmount);
        m_ConfirmButton.text = $"{currentAmount}";
    }

    private void ConfirmPurchase(ClickEvent evt)
    {
        if (currentShopItem == null)
        {
            Debug.LogError("No item selected for purchase.");
            return;
        }

        int totalCost = currentAmount * currentShopItem.Price;

        // Check if the player has enough tokens to make the purchase
        if (GameStateManager.Instance.CurrentToken < totalCost)
        {
            Debug.LogWarning("Not enough tokens to complete purchase.");
            return; // Exit the method as player doesn't have enough tokens
        }

        for (int i = 0; i < currentAmount; i++)
        {
            // Safety check, though this should always be true due to the earlier check
            if (GameStateManager.Instance.CurrentToken < currentShopItem.Price)
            {
                Debug.LogWarning("Not enough tokens to continue purchasing.");
                break; // Exit the loop if not enough tokens for the next item
            }

            GameStateManager.Instance.BuyItem(currentShopItem.name);
        }

        // Deduct the stock for the purchased amount
        DeductStock(currentShopItem, currentAmount);

        // Update the ListView
        m_ItemList.Rebuild();
    }

    private void BuyAllItems(ClickEvent evt)
    {
        if (currentShopItem == null)
        {
            Debug.LogError("No item selected for bulk purchase.");
            return;
        }

        int stockAvailable = GetItemStock(currentShopItem);
        int totalCost = stockAvailable * currentShopItem.Price;

        // Check if the player has enough tokens for the total cost
        if (GameStateManager.Instance.CurrentToken < totalCost)
        {
            Debug.LogWarning("Not enough tokens to buy all available stock.");
            return; // Exit the method as player doesn't have enough tokens
        }

        for (int i = 0; i < stockAvailable; i++)
        {
            // Additional check to ensure tokens are still sufficient
            if (GameStateManager.Instance.CurrentToken < currentShopItem.Price)
            {
                Debug.LogWarning("Not enough tokens to continue purchasing.");
                break; // Exit the loop if not enough tokens for the next item
            }

            GameStateManager.Instance.BuyItem(currentShopItem.name);
        }

        // Deduct all stock for the selected item
        DeductStock(currentShopItem, stockAvailable);

        // Update the ListView
        m_ItemList.Rebuild();
    }

    private void DeductStock(Item item, int amount)
    {
        if (GameStateManager.Instance.CopiedShopInventories.TryGetValue(GameStateManager.Instance.currentShopInventoryId, out var shopInventory))
        {
            foreach (var itemEntry in shopInventory)
            {
                if (itemEntry.item == item)
                {
                    itemEntry.quantity = Mathf.Max(0, itemEntry.quantity - amount);
                    return;
                }
            }
        }

        Debug.LogWarning($"Item {item.Name} not found in current shop inventory.");
    }

    private void CloseShopView(ClickEvent evt)
    {
        Debug.Log("Close Hide Screen");
        HideScreen();
        // Any additional logic for closing the shop view
    }

    public override void ShowScreen()
    {
        base.ShowScreen();

        // Update the message label
        m_MessageLabel.text = "How can I help you?";

        // Reset the confirm button text to the default value
        // Assuming "Confirm" is the default value, change it as needed
        m_ConfirmButton.text = "1";

        // Clear the selected item in list-view
        m_ItemList.ClearSelection();

        // Fill the item list with items from the current shop inventory
        FillItemList();

        // Optionally, reset other UI elements related to item details if needed
        currentShopItem = null; // Clear the current selected shop item
        m_ItemIcon.style.backgroundImage = null; // Reset the item icon
        m_ItemName.text = ""; // Clear the item name
        m_ItemDesc.text = ""; // Clear the item description

        // todo Set the staff button
        m_StaffButton.visible = GameStateManager.Instance.CurrentLocation == "Lion Area";
    }
}
