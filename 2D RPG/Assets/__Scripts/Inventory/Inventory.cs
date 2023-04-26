using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : SingletonMonobehaviour<Inventory>
{
    public event Action OnItemPickUp;

    public List<InventoryItem> inventory = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> stash = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> stashDictionary = new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> equipment = new List<InventoryItem>();
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stshSlotParent;

    private ItemSlotUI[] inventoryItemSlot;
    private ItemSlotUI[] stashItemSlot;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
        stashItemSlot = stshSlotParent.GetComponentsInChildren<ItemSlotUI>();
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
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item.itemType == ItemType.Equipment)
        {
            AddToInventory(item);
        }

        else if (item.itemType == ItemType.Material)
        {
            AddToStash(item);
        }

        OnItemPickUp?.Invoke();
    }

    public void RemoveItem(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        if (stashDictionary.TryGetValue(item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        OnItemPickUp?.Invoke();
    }

    private void AddToInventory(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventory.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }

    private void AddToStash(ItemData item)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }

    public void EquipItem(ItemData item)
    {
        ItemDataEquipment newEquipment = item as ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment itemToRemove = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> itemToCheck in equipmentDictionary)
        {
            if (itemToCheck.Key.equipmentType == newEquipment.equipmentType)
            {
                itemToRemove = itemToCheck.Key;
            }
        }

        if (itemToRemove != null)
            UnequipItem(itemToRemove);

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
    }

    private void UnequipItem(ItemDataEquipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
        }
    }
}
