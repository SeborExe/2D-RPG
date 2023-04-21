using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [field: Header("Major stats")]
    [field: SerializeField] public Stat Strength { get; private set; } //1 point to damage and critical chance
    [field: SerializeField] public Stat Agility { get; private set; } //1 point evasion and critical chance
    [field: SerializeField] public Stat Intelligence { get; private set; } //1 point magic damage and 3 magic resistance
    [field: SerializeField] public Stat Vitality { get; private set; } //1 point increase health by 3/5

    [field: Header("Defensive stats")]
    [field: SerializeField] public Stat MaxHealth { get; private set; }
    [field: SerializeField] public Stat Armor { get; private set; }
    [field: SerializeField] public Stat Evasion { get; private set; } //Chance to avoid attack

    [field: Header("Offensive stats")]
    [field: SerializeField] public Stat Damage { get; private set; }
    [field: SerializeField] public Stat CriticChance { get; private set; }
    [field: SerializeField] public Stat CriticPower { get; private set; } //In percent for example 150

    [SerializeField] private int currentHealth;
    private int defaultCriticalPower = 150;

    protected virtual void Start()
    {
        currentHealth = MaxHealth.GetValue();
        CriticPower.SetDefaultValue(defaultCriticalPower);
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {
        if (AvoidAttack(targetStats))
            return;

        int totalDamage = CalculateDamage(targetStats);

        if (CheckCritical())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        targetStats.TakeDamage(totalDamage);
    }


    public virtual void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (currentHealth == 0)
            Die();
    }

    protected virtual void Die()
    {
        
    }
    private bool AvoidAttack(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.Evasion.GetValue() + targetStats.Agility.GetValue();

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }
    private int CalculateDamage(CharacterStats targetStats)
    {
        int totalDamage = Damage.GetValue() + Strength.GetValue();
        totalDamage = Mathf.Max(1, totalDamage - targetStats.Armor.GetValue());
        return totalDamage;
    }
    private bool CheckCritical()
    {
        int totalCriticalChance = CriticChance.GetValue() + Agility.GetValue();

        if (UnityEngine.Random.Range(0, 100) < totalCriticalChance)
        {
            return true;
        }

        return false;
    }
    private int CalculateCriticalDamage(int damage)
    {
        float totalCriticalPower = (CriticPower.GetValue() + Strength.GetValue()) / 100;
        float criticalDamage = damage * totalCriticalPower;

        return Mathf.RoundToInt(criticalDamage);
    }
}
