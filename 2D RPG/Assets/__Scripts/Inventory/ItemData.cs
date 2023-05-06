using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    [Range(0f, 100f)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    public virtual string GetDiscription()
    {
        return "";
    }
}
