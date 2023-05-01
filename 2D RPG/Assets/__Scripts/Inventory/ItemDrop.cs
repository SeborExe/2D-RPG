using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private float chanceToDropItem;
    [SerializeField] private int amountOfItems;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }

        for (int i = 0; i < amountOfItems; i++)
        {
            if (Random.Range(0, 100) >= chanceToDropItem) continue;

            if (dropList.Count > 0)
            {
                ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

                if (randomItem != null)
                {
                    dropList.Remove(randomItem);
                    DropItem(randomItem);
                }
            }
        }
    }

    protected virtual void DropItem(ItemData itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelicty = new Vector2(Random.Range(-5f, 5f), Random.Range(10f, 18f));

        newDrop.GetComponent<ItemObject>().SetUpItem(itemData, randomVelicty);
    }
}
