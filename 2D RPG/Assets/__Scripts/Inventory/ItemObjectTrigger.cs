using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject itemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (player.IsDead) return;

            itemObject.PickUpItem();
        }
    }
}
