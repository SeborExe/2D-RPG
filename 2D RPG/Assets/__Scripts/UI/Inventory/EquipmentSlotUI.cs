using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : ItemSlotUI
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = $"EquipmentSlot_{slotType}";
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null) return;

        Inventory.Instance.UnequipItem(item.data as ItemDataEquipment);
        Inventory.Instance.AddItem(item.data as ItemDataEquipment);
        CleanUpSlot();
    }
}
