using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect_", menuName = "Inventory/Item Effect")]
public class ItemEffect : ScriptableObject
{
    [TextArea] public string effectDescription;

    public virtual void ExecuteEffect(Transform enemyPosition)
    {

    }
}
