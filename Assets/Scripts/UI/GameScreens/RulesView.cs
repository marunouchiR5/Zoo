using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RulesView : BaseView
{
    [Header("Entry Asset")]
    [Tooltip("Entry Asset to instantiate")]
    [SerializeField] VisualTreeAsset m_EntryAsset;

    // locates elements to update
    const string k_RulesList = "rules-list-view";
    const string k_ReadButton = "read--button";
    const string k_BackButton = "back--button";

    ListView m_RulesList;
    Button m_ReadButton;
    Button m_BackButton;

    private RuleSet selectedRuleSet;

    private void OnEnable()
    {
        if (m_RulesList != null)
        {
            m_RulesList.selectionChanged += OnRuleSelected;
        }
        else
        {
            Debug.LogError("m_RulesList is not initialized");
        }
    }

    private void OnDisable()
    {
        if (m_RulesList != null)
        {
            m_RulesList.selectionChanged -= OnRuleSelected;
        }
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_RulesList = m_Screen.Q<ListView>(k_RulesList);
        m_ReadButton = m_Screen.Q<Button>(k_ReadButton);
        m_BackButton = m_Screen.Q<Button>(k_BackButton);

        m_ReadButton.SetEnabled(false);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_ReadButton?.RegisterCallback<ClickEvent>(ReadRule);
        m_BackButton?.RegisterCallback<ClickEvent>(HideLogView);
    }

    private void ReadRule(ClickEvent evt)
    {
        if (selectedRuleSet != null)
        {
            GameStateManager.Instance.SelectedRuleSet = selectedRuleSet; // Assuming you have such a property
                                                                         // Logic to open LogView and display the selected rules
            m_GameViewManager.ShowLogView();
        }
        else
        {
            Debug.LogError("No rule set selected");
        }
    }

    private void HideLogView(ClickEvent evt)
    {
        HideScreen();
    }

    public override void ShowScreen()
    {
        base.ShowScreen();

        FillRulesList(GameStateManager.Instance.CollectedRuleSets);

        // Clear the selection in the ListView
        if (m_RulesList != null)
        {
            m_RulesList.selectedIndex = -1;
        }

        selectedRuleSet = null;
        m_ReadButton.SetEnabled(false);
    }

    private void FillRulesList(List<RuleSet> rulesList)
    {
        // Set up a make item function for a rule entry
        m_RulesList.makeItem = () =>
        {
            var newRuleSetEntry = m_EntryAsset.Instantiate();

            // If you have a specific controller or logic for each rule entry
            var newRuleSetEntryLogic = new RuleSetEntryController();
            newRuleSetEntry.userData = newRuleSetEntryLogic;
            newRuleSetEntryLogic.SetVisualElement(newRuleSetEntry);

            return newRuleSetEntry;
        };

        // Set up bind function for a specific rule entry
        m_RulesList.bindItem = (ruleSet, index) =>
        {
            (ruleSet.userData as RuleSetEntryController).SetRuleSetData(rulesList[index]);
        };

        // Optionally set a fixed item height
        m_RulesList.fixedItemHeight = 80;

        // Set the actual item's source list/array
        m_RulesList.itemsSource = rulesList;
    }

    // event-handling methods
    private void OnRuleSelected(IEnumerable<object> selectedItems)
    {
        selectedRuleSet = selectedItems.FirstOrDefault() as RuleSet;
        m_ReadButton.SetEnabled(true);
    }
}
