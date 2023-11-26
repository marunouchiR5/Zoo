using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EquipmentDisplay : HUD
{
    const string k_HeadName = "head-name";
    const string k_BodyName = "body-name";

    Button m_HeadName;
    Button m_BodyName;

    private void OnEnable()
    {
        GameStateManager.EquipmentChanged += OnEquipmentChanged;
        Equipment.EquipmentChanged += OnEquipmentChanged;
    }

    private void OnDisable()
    {
        GameStateManager.EquipmentChanged -= OnEquipmentChanged;
        Equipment.EquipmentChanged -= OnEquipmentChanged;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_HeadName = m_Screen.Q<Button>(k_HeadName);
        m_BodyName = m_Screen.Q<Button>(k_BodyName);
    }

    protected override void RegisterButtonCallbacks()
    {
        m_HeadName?.RegisterCallback<ClickEvent>(ClickHeadEquipment);
        m_BodyName?.RegisterCallback<ClickEvent>(ClickBodyEquipment);
    }

    private void ClickHeadEquipment(ClickEvent evt)
    {
        if (GameStateManager.Instance.CurrentHeadEquipment == null) return;

        GameStateManager.Instance.SelectedItem = GameStateManager.Instance.CurrentHeadEquipment;
        m_GameViewManager.ShowItemInspectView();
    }

    private void ClickBodyEquipment(ClickEvent evt)
    {
        if (GameStateManager.Instance.CurrentBodyEquipment == null) return;

        GameStateManager.Instance.SelectedItem = GameStateManager.Instance.CurrentBodyEquipment;
        m_GameViewManager.ShowItemInspectView();
    }

    // event-handling methods
    private void OnEquipmentChanged()
    {
        if (m_HeadName == null || m_BodyName == null)
        {
            Debug.LogError("UI elements are not initialized properly.");
            return;
        }

        if (GameStateManager.Instance.CurrentHeadEquipment != null)
        {
            m_HeadName.text = GameStateManager.Instance.CurrentHeadEquipment.Name;
        }
        else
        {
            m_HeadName.text = "Empty Slot";
        }

        if (GameStateManager.Instance.CurrentBodyEquipment != null)
        {
            m_BodyName.text = GameStateManager.Instance.CurrentBodyEquipment.Name;
        }
        else
        {
            m_BodyName.text = "Empty Slot";
        }
    }
}
