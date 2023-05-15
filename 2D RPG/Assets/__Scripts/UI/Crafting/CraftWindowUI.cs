using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftWindowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;
    [SerializeField] private Image[] materialImage;

    public void SetUpCraftWindow(ItemDataEquipment itemData)
    {
        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TMP_Text>().color = Color.clear;
        }

        for (int i = 0; i < itemData.craftingMaterials.Count; i++)
        {
            materialImage[i].sprite = itemData.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;
            materialImage[i].GetComponentInChildren<TMP_Text>().color = Color.white;
            materialImage[i].GetComponentInChildren<TMP_Text>().text = itemData.craftingMaterials[i].stackSize.ToString();
        }

        itemIcon.sprite = itemData.icon;
        itemName.text = itemData.itemName;
        itemDescription.text = itemData.GetDiscription();

        craftButton.onClick.AddListener(() => Inventory.Instance.CanCraft(itemData, itemData.craftingMaterials));
    }
}
