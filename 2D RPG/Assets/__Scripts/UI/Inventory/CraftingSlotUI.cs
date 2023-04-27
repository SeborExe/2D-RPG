using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlotUI : ItemSlotUI
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemDataEquipment craftData = item.data as ItemDataEquipment;
        if (Inventory.Instance.CanCraft(craftData, craftData.craftingMaterials)) 
        {
            Debug.Log("Crafting success");
        }
    }
}
