using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlotUI : ItemSlotUI
{
    [SerializeField] private TMP_Text itemNameText;

    protected override void Start()
    {
        base.Start();
    }

    public void SetUpCraftSlot(ItemDataEquipment itemData)
    {
        if (itemData == null) return;

        item.data = itemData;

        itemImage.sprite = itemData.icon;
        itemNameText.text = itemData.itemName;
        itemAmountBackground.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        mainGameUI.craftWindowUI.SetUpCraftWindow(item.data as ItemDataEquipment);
    }
}
