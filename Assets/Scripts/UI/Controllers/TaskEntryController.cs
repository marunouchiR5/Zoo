using UnityEngine;
using UnityEngine.UIElements;

public class TaskEntryController
{
    private VisualElement rootElement;

    public void SetVisualElement(VisualElement element)
    {
        rootElement = element;
        // Initialize other UI elements here
    }

    public void SetTaskData(Task task)
    {
        // Bind task data to UI elements in rootElement
        if (rootElement.childCount > 0 && rootElement[0] is Label label)
        {
            if (task.HasProgress)
            {
                task.Progress = GameStateManager.Instance.VisitedAreas.Count;
                label.text = task.Name + " (" + task.Progress.ToString() + "/" + task.ProgressGoal.ToString() + ")";
            }
            else
            {
                label.text = task.Name; // Set the text of the label
            }
        }
        else
        {
            Debug.LogError("TaskEntryAsset does not contain a label as the first child.");
        }
    }
}
