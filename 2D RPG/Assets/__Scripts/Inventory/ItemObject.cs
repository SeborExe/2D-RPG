using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private ItemData itemData;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnValidate()
    {
        SetUpVisuals();
    }

    private void SetUpVisuals()
    {
        if (itemData == null) return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = $"ItemObject_{itemData.name}";
    }

    public void SetUpItem(ItemData itemData, Vector2 velocity)
    {
        this.itemData = itemData;
        rb.velocity = velocity;

        SetUpVisuals();
    }

    public void PickUpItem()
    {
        Inventory.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
