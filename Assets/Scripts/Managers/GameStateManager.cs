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
    public bool ZooGateMapCollected { get; set; }
    public List<string> VisitedAreas;

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
        //string itemsFolderPath = "GameData/Items";
        //Item[] items = Resources.LoadAll<Item>(itemsFolderPath);
        //foreach (var item in items)
        //{
        //    CurrentInventory.Add(item);
        //}
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

    public void UpdateSanity(int currentSanity)
    {
        CurrentSanity = currentSanity;
        SanityChanged?.Invoke();
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
}
