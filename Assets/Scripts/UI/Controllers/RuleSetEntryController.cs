using UnityEngine;
using UnityEngine.UIElements;

public class RuleSetEntryController
{
    private VisualElement rootElement;

    public void SetVisualElement(VisualElement element)
    {
        rootElement = element;
        // Initialize other UI elements here
    }

    public void SetRuleSetData(RuleSet ruleSet)
    {
        // Bind ruleSet data to UI elements in rootElement
        if (rootElement.childCount > 0 && rootElement[0] is Label label)
        {
            label.text = ruleSet.SetName; // Set the text of the label
            //todo
        }
        else
        {
            Debug.LogError("TaskEntryAsset does not contain a label as the first child.");
        }
    }
}
