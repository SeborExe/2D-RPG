using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect_Buff_", menuName = "Inventory/Item Effect/Buff")]
public class BuffEffect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private int buffDuration;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        stats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        if (buffType == StatType.Strength) return stats.Strength;
        else if (buffType == StatType.Agility) return stats.Agility;
        else if (buffType == StatType.Inteligence) return stats.Intelligence;
        else if (buffType == StatType.Vitality) return stats.Vitality;
        else if (buffType == StatType.Damage) return stats.Damage;
        else if (buffType == StatType.CritChance) return stats.CriticChance;
        else if (buffType == StatType.CritPower) return stats.CriticPower;
        else if (buffType == StatType.Health) return stats.MaxHealth;
        else if (buffType == StatType.Armor) return stats.Armor;
        else if (buffType == StatType.Evasion) return stats.Evasion;
        else if (buffType == StatType.MaicRes) return stats.MagicResistance;
        else if (buffType == StatType.FireDamage) return stats.FireDamage;
        else if (buffType == StatType.IceDamage) return stats.IceDamage;
        else if (buffType == StatType.LightingDamage) return stats.LightingDamage;

        return null;
    }
}
