using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerItemDrop : ItemDrop
{
    [Header("Players Drop")]
    [SerializeField] private float chanceToLooseItems;
    [SerializeField] private float chanceToLooseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.Instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> stashToLoose = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemDataEquipment);
        }

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                DropItem(item.data);
                stashToLoose.Add(item);
            }
        }

        for (int i = 0; i < stashToLoose.Count; i++)
        {
            inventory.RemoveItem(itemsToUnequip[i].data);
        }
    }
}
