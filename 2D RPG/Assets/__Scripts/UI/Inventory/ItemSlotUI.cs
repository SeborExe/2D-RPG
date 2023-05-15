using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TMP_Text itemAmountText;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] protected GameObject itemAmountBackground;

    protected MainGameUI mainGameUI;

    public InventoryItem item;

    protected virtual void Start()
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
                itemAmountBackground.SetActive(true);
                itemAmountText.text = item.stackSize.ToString();

            }
            else
            {
                itemAmountBackground.SetActive(false);
                itemAmountText.text = "";
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

        itemAmountText.text = "";
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
