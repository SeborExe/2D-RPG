using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect_Heal_", menuName = "Inventory/Item Effect/Heal")]
public class HealEffect : ItemEffect
{
    [SerializeField, Range(0, 100)] private float healPercent;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * (healPercent / 100f));

        playerStats.IncreaseHealthBy(healAmount);
    }
}
