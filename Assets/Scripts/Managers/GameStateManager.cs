using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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

    // specific view related
    public ConversationData ConversationData;
    public RuleSet SelectedRuleSet;
    public Item SelectedItem;   // in HUD - Equipment

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional, to persist across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Optional, destroys any duplicate instances
        }
    }

    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        // Initialization
        // task
        CurrentTasks = new List<Task> { /* ... */ };
        string taskPath = $"GameData/Tasks/Unaware";
        Task initialTask = Resources.Load<Task>(taskPath);
        CurrentTasks.Add(initialTask);
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

        // system related
        VisitedAreas = new List<String> { /* ... */ };
        Aware = false;
        ZooGateMapCollected = false;
        BlackClothesCollected = false;
        ZooDirectorRoomMapCollected = false;
        JellyfishLightUsed = false;
        MapCornerFed = false;
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
}
