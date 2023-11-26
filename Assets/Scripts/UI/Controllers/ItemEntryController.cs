using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemEntryController
{
    private VisualElement rootElement;
    private Item itemData;
    private Action onItemClickedCallback; // Callback action

    public void SetVisualElement(VisualElement element)
    {
        rootElement = element;
        // Initialize other UI elements here
    }

    public void SetItemData(Item item, Action onClickedCallback)
    {
        itemData = item;
        onItemClickedCallback = onClickedCallback;

        if (rootElement.childCount > 0 && rootElement[0] is Button button)
        {
            button.text = item.Name;
            button.clicked += OnItemClicked;
        }
        else
        {
            Debug.LogError("ItemEntryAsset does not contain a button as the first child.");
        }
    }

    private void OnItemClicked()
    {
        GameStateManager.Instance.SelectedItem = itemData;
        onItemClickedCallback?.Invoke(); // Call the callback
    }
}
