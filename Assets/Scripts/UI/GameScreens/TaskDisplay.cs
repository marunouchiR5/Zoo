using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskDisplay : HUD
{
    [Header("Task Asset")]
    [Tooltip("Task Asset to instantiate")]
    [SerializeField] VisualTreeAsset m_TaskAsset;

    // string IDs
    const string k_TaskList = "task-list";

    ListView m_TaskList;

    private void OnEnable()
    {
        GameStateManager.TaskUpdated += OnTaskUpdated;
    }

    private void OnDisable()
    {
        GameStateManager.TaskUpdated -= OnTaskUpdated;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_TaskList = m_Screen.Q<ListView>(k_TaskList);
    }

    // event-handling methods
    private void OnTaskUpdated()
    {
        FillTaskList(GameStateManager.Instance.CurrentTasks);
    }

    private void FillTaskList(List<Task> taskList)
    {
        // Set up a make item function for a task entry
        m_TaskList.makeItem = () =>
        {
            var newTaskEntry = m_TaskAsset.Instantiate();

            // If you have a specific controller or logic for each task entry
            var newTaskEntryLogic = new TaskEntryController();
            newTaskEntry.userData = newTaskEntryLogic;
            newTaskEntryLogic.SetVisualElement(newTaskEntry);

            return newTaskEntry;
        };

        // Set up bind function for a specific task entry
        m_TaskList.bindItem = (item, index) =>
        {
            (item.userData as TaskEntryController).SetTaskData(taskList[index]);
        };

        m_TaskList.selectionType = SelectionType.None;

        // Optionally set a fixed item height
        m_TaskList.fixedItemHeight = 50;

        // Set the actual item's source list/array
        m_TaskList.itemsSource = taskList;
    }
}
