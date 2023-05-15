using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftListUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemDataEquipment> craftEquipment = new List<ItemDataEquipment>();
    [SerializeField] private List<CraftingSlotUI> craftSlots = new List<CraftingSlotUI>();

    private void Start()
    {
        AssignCraftSlots();
    }

    private void AssignCraftSlots()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<CraftingSlotUI>());
        }
    }

    public void SetUpCraftList()
    {
        for (int i = 0; i < craftSlots.Count; i++)
        {
            Destroy(craftSlots[i].gameObject);
        }

        craftSlots = new List<CraftingSlotUI>();

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<CraftingSlotUI>().SetUpCraftSlot(craftEquipment[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetUpCraftList();
    }
}
