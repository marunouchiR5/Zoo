using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameStateManager : MonoBehaviour
{
    // events
    public static event Action TaskUpdated;
    public static event Action InventoryUpdated;
    public static event Action TokenUpdated;
    public static event Action EquipmentChanged;
    public static event Action SanityChanged;
    public static event Action RuleSetCollected;
    public static event Action BecameAware;
    public static event Action GameOver;
    public static event Action NewGameStarted;

    public static GameStateManager Instance { get; private set; }

    public const int MaxSanity = 4;

    // HUD related
    public List<Task> CurrentTasks;
    public List<Item> CurrentInventory;
    public Equipment CurrentHeadEquipment;
    public Equipment CurrentBodyEquipment;
    public List<Symptom> CurrentSymptoms;
    public List<RuleSet> CollectedRuleSets;
    public int CurrentSanity { get; set; }
    public int CurrentToken { get; set; }
    public Sprite CurrentProfile { get; set; }

    // system related
    public bool Aware { get; set; }
    public bool ZooGateMapCollected { get; set; }
    public List<string> VisitedAreas;
    public bool BlackClothesCollected { get; set; }
    public bool ZooDirectorRoomMapCollected { get; set; }
    public bool JellyfishLightUsed { get; set; }
    public bool MapCornerFed { get; set; }
    public bool TicketSold { get; set; }

    // specific view related
    public ConversationData ConversationData;
    public RuleSet SelectedRuleSet;
    public Item SelectedItem;   // in HUD - Equipment

    // shop related
    public Dictionary<string, List<ItemEntry>> ShopInventories = new Dictionary<string, List<ItemEntry>>();
    public Dictionary<string, List<ItemEntry>> CopiedShopInventories = new Dictionary<string, List<ItemEntry>>();
    public string currentShopInventoryId;
    public ShopInventory ShopInventoryLionArea;
    public ShopInventory ShopInventoryAquariumInside;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            StartNewGame();
        }
    }

    public void StartNewGame()
    {
        // Initialization
        // task
        CurrentTasks = new List<Task> { /* ... */ };
        string taskPath = $"GameData/Tasks/Unaware";
        Task initialTask = Resources.Load<Task>(taskPath);
        CurrentTasks.Add(initialTask);
        VisitedAreas = new List<String> { /* ... */ };
        TaskUpdated?.Invoke();

        // inventory
        CurrentInventory = new List<Item> { /* ... */ };
        string itemPath = $"GameData/Items/EnergyDrink";
        Item initialItem = Resources.Load<Item>(itemPath);
        CurrentInventory.Add(initialItem);
        InventoryUpdated?.Invoke();

        // token
        CurrentToken = 5;
        TokenUpdated?.Invoke();

        // equipment
        CurrentHeadEquipment = null;
        CurrentBodyEquipment = null;
        EquipmentChanged?.Invoke();

        // sanity
        CurrentSanity = 4;
        SanityChanged?.Invoke();

        // player profile
        CurrentProfile = null;

        // symptoms
        CurrentSymptoms = new List<Symptom> { /* ... */ };

        // rule sets
        CollectedRuleSets = new List<RuleSet> { /* ... */ };

        // gameplay related
        Aware = false;
        ZooGateMapCollected = false;
        BlackClothesCollected = false;
        ZooDirectorRoomMapCollected = false;
        JellyfishLightUsed = false;
        MapCornerFed = false;
        TicketSold = false;
        NewGameStarted?.Invoke();

        // shop related
        InitializeShopInventories();
        CopyShopInventories();
    }

    private void InitializeShopInventories()
    {
        // Use the ShopInventory scriptable objects to initialize the shop inventories
        ShopInventories["LionArea"] = ConvertShopInventoryToList(ShopInventoryLionArea);
        ShopInventories["AquariumInside"] = ConvertShopInventoryToList(ShopInventoryAquariumInside);
    }

    private List<ItemEntry> ConvertShopInventoryToList(ShopInventory shopInventory)
    {
        return new List<ItemEntry>(shopInventory.items);
    }

    private void CopyShopInventories()
    {
        CopiedShopInventories.Clear();
        foreach (var keyValuePair in ShopInventories)
        {
            // Deep copy each List<ItemEntry>
            List<ItemEntry> copiedList = new List<ItemEntry>();
            foreach (var itemEntry in keyValuePair.Value)
            {
                // Assuming ItemEntry is a class - create a new instance (deep copy)
                ItemEntry newItemEntry = new ItemEntry
                {
                    item = itemEntry.item, // Assuming item is a reference type
                    quantity = itemEntry.quantity
                };
                copiedList.Add(newItemEntry);
            }

            CopiedShopInventories.Add(keyValuePair.Key, copiedList);
        }
    }

    public void SetActiveConversationData(string sceneName, string buttonName)
    {
        string assetPath = $"GameData/Conversations/{sceneName}_{buttonName}";
        ConversationData = Resources.Load<ConversationData>(assetPath);

        if (ConversationData == null)
        {
            Debug.LogError("Failed to load ConversationData for path: " + assetPath);
        }
    }

    public void UpdateSanity(int updatedSanity)
    {
        CurrentSanity = Mathf.Min(updatedSanity, MaxSanity);
        // game over when current sanity reaches zero
        if (CurrentSanity <= 0)
        {
            GameOver?.Invoke();
            return;
        }

        SanityChanged?.Invoke();

        if (!Aware && CurrentSanity < MaxSanity)
        {
            Aware = true;
            BecameAware?.Invoke();

            // task related
            string taskPath = $"GameData/Tasks/Aware";
            Task initialTask = Resources.Load<Task>(taskPath);
            CurrentTasks.Clear();
            CurrentTasks.Add(initialTask);
            TaskUpdated?.Invoke();
        }

        // player profile related
    }

    public void AddRuleSet(RuleSet ruleSetToAdd)
    {
        // Check if the ruleSetToAdd is already in the CollectedRuleSets list
        if (!CollectedRuleSets.Contains(ruleSetToAdd))
        {
            // If not, add it to the list
            CollectedRuleSets.Add(ruleSetToAdd);
            Debug.Log("Added RuleSet: " + ruleSetToAdd.SetName);

            RuleSetCollected?.Invoke();
        }
        else
        {
            Debug.Log("RuleSet already collected: " + ruleSetToAdd.SetName);
        }
    }

    public void UpdateVisitedAreas(string areaName)
    {
        if (!VisitedAreas.Contains(areaName))
        {
            VisitedAreas.Add(areaName);
            Debug.Log("First visit: " + areaName);
            TaskUpdated?.Invoke();
        }
        else
        {
            Debug.Log("Area already visited: " + areaName);
        }
    }

    public void AddItem(string itemPath)
    {
        if (string.IsNullOrEmpty(itemPath))
        {
            Debug.LogError("Invalid item path provided.");
            return;
        }

        Item newItem = Resources.Load<Item>(itemPath);
        if (newItem == null)
        {
            Debug.LogError($"Failed to load item at path: {itemPath}");
            return;
        }

        // optional: Check if the item is already in the inventory
        CurrentInventory.Add(newItem);
        Debug.Log($"Added item to inventory: {newItem.name}");
        InventoryUpdated?.Invoke();
    }

    public bool HasItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Invalid item name provided.");
            return false;
        }

        // Check if the item is in the inventory
        foreach (var item in CurrentInventory)
        {
            if (item.name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Invalid item name provided.");
            return;
        }

        // Attempt to find the item in the inventory
        for (int i = 0; i < CurrentInventory.Count; i++)
        {
            if (CurrentInventory[i].name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            {
                CurrentInventory.RemoveAt(i);
                Debug.Log($"Item removed from inventory: {itemName}");
                InventoryUpdated?.Invoke();
                return;
            }
        }

        Debug.LogWarning($"Item not found in inventory: {itemName}");
    }

    public bool IsCurrentBodyEquipment(string equipmentName)
    {
        if (CurrentBodyEquipment == null)
        {
            return false;
        }

        return CurrentBodyEquipment.Name.Equals(equipmentName, StringComparison.OrdinalIgnoreCase);
    }

    public void BuyItem(string itemName)
    {
        string itemPath = $"GameData/Items/" + itemName;
        if (string.IsNullOrEmpty(itemPath))
        {
            Debug.LogError("Invalid item path provided.");
            return;
        }

        Item itemToBuy = Resources.Load<Item>(itemPath);
        if (itemToBuy == null)
        {
            Debug.LogError($"Failed to load item at path: {itemPath}");
            return;
        }

        if (CurrentToken >= itemToBuy.Price)
        {
            // Deduct the price from the current tokens
            CurrentToken -= itemToBuy.Price;

            // Add the item to the inventory
            CurrentInventory.Add(itemToBuy);
            Debug.Log($"Purchased and added item to inventory: {itemToBuy.name}");

            // Invoke the inventory updated event
            InventoryUpdated?.Invoke();
            // Optionally, invoke the token updated event
            TokenUpdated?.Invoke();
        }
        else
        {
            Debug.LogWarning($"Not enough tokens to buy item: {itemToBuy.name}");
        }
    }

    // Method to set the current shop inventory identifier
    public void SetCurrentShopInventoryId(string shopId)
    {
        currentShopInventoryId = shopId;
        // Optionally, trigger an event or update if needed
    }

    // Method to get the current shop's inventory
    public List<ItemEntry> GetCurrentShopInventory()
    {
        if (CopiedShopInventories.ContainsKey(currentShopInventoryId))
        {
            return CopiedShopInventories[currentShopInventoryId];
        }
        else
        {
            Debug.LogWarning("Shop inventory not found for id: " + currentShopInventoryId);
            return new List<ItemEntry>(); // Return empty list or handle as needed
        }
    }

    public int GetItemStock(string itemName, string shopName)
    {
        if (ShopInventories.ContainsKey(shopName))
        {
            List<ItemEntry> shopInventory = ShopInventories[shopName];
            foreach (var shopItem in shopInventory)
            {
                if (shopItem.item.name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    // Assuming each shopItem has a Quantity property
                    return shopItem.quantity;
                }
            }
        }

        return 0; // Item not found in the specified shop inventory
    }
}
