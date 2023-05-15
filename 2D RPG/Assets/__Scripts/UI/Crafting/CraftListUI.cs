using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftListUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemDataEquipment> craftEquipment = new List<ItemDataEquipment>();

    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<CraftListUI>().SetUpCraftList();
        SetUpDefaultCraftWindow();
    }

    public void SetUpCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<CraftingSlotUI>().SetUpCraftSlot(craftEquipment[i]);
        }
    }

    public void SetUpDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
            GetComponentInParent<MainGameUI>().craftWindowUI.SetUpCraftWindow(craftEquipment[0]);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetUpCraftList();
    }
}
