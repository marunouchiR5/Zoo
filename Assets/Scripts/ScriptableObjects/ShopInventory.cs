using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewShopInventory", menuName = "Game Data/Shop Inventory")]
public class ShopInventory : ScriptableObject
{
    public List<ItemEntry> items = new List<ItemEntry>();
}

[System.Serializable]
public class ItemEntry
{
    public Item item;
    public int quantity;
}
