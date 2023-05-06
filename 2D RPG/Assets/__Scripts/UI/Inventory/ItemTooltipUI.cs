using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltipUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemTypeText;
    [SerializeField] private TMP_Text itemDescriptionText;

    public void ShowTooltip(ItemDataEquipment item)
    {
        if (item == null) return;

        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescriptionText.text = item.GetDiscription();

        gameObject.SetActive(true);
    }

    public void HideTooltip() => gameObject.SetActive(false);
}
