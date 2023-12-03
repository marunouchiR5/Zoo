using UnityEngine.UIElements;

public class ShopItemEntryController
{
    private VisualElement rootElement;
    private Label itemNameLabel;
    private Label itemStockLabel;
    private Label itemPriceLabel;

    public void SetVisualElement(VisualElement element)
    {
        rootElement = element;
        itemNameLabel = rootElement.Q<Label>("item-name"); // Assuming you have an item-name label in your VisualTreeAsset
        itemStockLabel = rootElement.Q<Label>("item-stock"); // Assuming you have an item-stock label
        itemPriceLabel = rootElement.Q<Label>("item-price"); // Assuming you have an item-price label
    }

    public void SetItemData(Item item, int stock)
    {
        if (itemNameLabel != null) itemNameLabel.text = item.Name;
        if (itemStockLabel != null) itemStockLabel.text = $"{stock}";
        if (itemPriceLabel != null) itemPriceLabel.text = $"{item.Price}";
    }
}
