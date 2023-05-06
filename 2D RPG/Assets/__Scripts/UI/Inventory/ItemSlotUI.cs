using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemText;
    [SerializeField] private Sprite defaultImage;

    private MainGameUI mainGameUI;

    public InventoryItem item;

    private void Start()
    {
        mainGameUI = GetComponentInParent<MainGameUI>();
    }

    public void UpdateSlot(InventoryItem item)
    {
        this.item = item;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.Instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.Instance.EquipItem(item.data);
        }
    }

    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = defaultImage;

        itemText.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        mainGameUI.itemTooltipUI.ShowTooltip(item.data as ItemDataEquipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;

        mainGameUI.itemTooltipUI.HideTooltip();
    }
}
