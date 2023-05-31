using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment_", menuName = "Inventory/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique Effects")]
    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Main Stats")]
    public int stregth;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive Stats")]
    public int damage;
    public int criticalChance;
    public int criticalPower;

    [Header("Defensive Stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirments")]
    public List<InventoryItem> craftingMaterials;

    public void AddModifier()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        playerStats.Strength.AddModifiers(stregth);
        playerStats.Agility.AddModifiers(agility);
        playerStats.Intelligence.AddModifiers(intelligence);
        playerStats.Vitality.AddModifiers(vitality);
        playerStats.Damage.AddModifiers(damage);
        playerStats.CriticChance.AddModifiers(criticalChance);
        playerStats.CriticPower.AddModifiers(criticalPower);
        playerStats.MaxHealth.AddModifiers(health);
        playerStats.Armor.AddModifiers(armor);
        playerStats.Evasion.AddModifiers(evasion);
        playerStats.MagicResistance.AddModifiers(magicResistance);
        playerStats.FireDamage.AddModifiers(fireDamage);
        playerStats.IceDamage.AddModifiers(iceDamage);
        playerStats.LightingDamage.AddModifiers(lightingDamage);
    }

    public void RemoveModifier()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        playerStats.Strength.RemoveModifiers(stregth);
        playerStats.Agility.RemoveModifiers(agility);
        playerStats.Intelligence.RemoveModifiers(intelligence);
        playerStats.Vitality.RemoveModifiers(vitality);
        playerStats.Damage.RemoveModifiers(damage);
        playerStats.CriticChance.RemoveModifiers(criticalChance);
        playerStats.CriticPower.RemoveModifiers(criticalPower);
        playerStats.MaxHealth.RemoveModifiers(health);
        playerStats.Armor.RemoveModifiers(armor);
        playerStats.Evasion.RemoveModifiers(evasion);
        playerStats.MagicResistance.RemoveModifiers(magicResistance);
        playerStats.FireDamage.RemoveModifiers(fireDamage);
        playerStats.IceDamage.RemoveModifiers(iceDamage);
        playerStats.LightingDamage.RemoveModifiers(lightingDamage);
    }

    public void Effect(Transform enemyPosition)
    {
        foreach (ItemEffect effect in itemEffects)
        {
            effect.ExecuteEffect(enemyPosition);
        }
    }

    public override string GetDiscription()
    {
        sb.Length = 0;

        AddItemDescription(stregth, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(criticalChance, "Crit.Chance");
        AddItemDescription(criticalPower, "Crit.Power");

        AddItemDescription(health, "Health");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(armor, "Armor");
        AddItemDescription(magicResistance, "Magic Resist.");

        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightingDamage, "Lighting Dam.");

        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].effectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Unique: " + itemEffects[i].effectDescription);
            }
        }

        return sb.ToString();
    }

    private void AddItemDescription(int value, string name)
    {
        if (value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (value> 0) sb.Append($"+ {value} {name}");
        }
    }
}
