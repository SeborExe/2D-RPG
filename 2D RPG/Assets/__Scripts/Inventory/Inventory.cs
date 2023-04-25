using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonMonobehaviour<Inventory>
{
    public event Action OnItemPickUp;

    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;

    private ItemSlotUI[] itemSlots;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        itemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
    }

    private void OnEnable()
    {
        OnItemPickUp += UpdateSlotsUI;
    }

    private void OnDisable()
    {
        OnItemPickUp -= UpdateSlotsUI;
    }

    private void UpdateSlotsUI()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            itemSlots[i].UpdateSlot(inventoryItems[i]);
        }
    }

    public void AddItem(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }

        OnItemPickUp?.Invoke();
    }

    public void RemoveItem(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        OnItemPickUp?.Invoke();
    }
}
