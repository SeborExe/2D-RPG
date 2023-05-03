using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect_ThunderStrike", menuName = "Inventory/Item Effect/Thunder Strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }
}
