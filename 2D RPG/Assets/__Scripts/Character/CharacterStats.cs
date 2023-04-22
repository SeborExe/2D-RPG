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
    [field: SerializeField] public Stat MagicResistance { get; private set; }

    [field: Header("Offensive stats")]
    [field: SerializeField] public Stat Damage { get; private set; }
    [field: SerializeField] public Stat CriticChance { get; private set; }
    [field: SerializeField] public Stat CriticPower { get; private set; } //In percent for example 150

    [field: Header("Magic stats")]
    [field: SerializeField] public Stat FireDamage { get; private set; }
    [field: SerializeField] public Stat IceDamage { get; private set; }
    [field: SerializeField] public Stat LightingDamage { get; private set; }

    [field: Header("Bools")]
    [field: SerializeField] public bool IsIgnited { get; private set; }
    [field: SerializeField] public bool IsChilled { get; private set; }
    [field: SerializeField] public bool IsSchocked { get; private set; }

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

        //targetStats.TakeDamage(totalDamage);
        DoMagicDamage(targetStats);
    }


    public virtual void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (currentHealth == 0)
            Die();
    }

    public virtual void DoMagicDamage(CharacterStats targetStats)
    {
        int fireDamage = FireDamage.GetValue();
        int iceDamage = IceDamage.GetValue();
        int lightingDamage = LightingDamage.GetValue();

        int totalMagicalDamage = CalculateMagicDamage(targetStats, fireDamage, iceDamage, lightingDamage);

        targetStats.TakeDamage(Mathf.Max(1, totalMagicalDamage));

        if (Mathf.Max(fireDamage, iceDamage, lightingDamage) <= 0) return;

        bool canApplyIgnite = fireDamage > iceDamage && fireDamage > lightingDamage;
        bool canAppllyChild = iceDamage > fireDamage && iceDamage > lightingDamage;
        bool canApplyShock = lightingDamage > fireDamage && lightingDamage > iceDamage;

        while (!canApplyIgnite && !canAppllyChild && !canApplyShock)
        {
            if (UnityEngine.Random.value < 0.4f && iceDamage > 0)
            {
                canApplyIgnite = true;
                targetStats.ApplyAilments(canApplyIgnite, canAppllyChild, canApplyShock);
                return;
            }

            if (UnityEngine.Random.value < 0.5f && fireDamage > 0)
            {
                canAppllyChild = true;
                targetStats.ApplyAilments(canApplyIgnite, canAppllyChild, canApplyShock);
                return;
            }

            if (UnityEngine.Random.value < 0.5f && lightingDamage > 0)
            {
                canApplyShock = true;
                targetStats.ApplyAilments(canApplyIgnite, canAppllyChild, canApplyShock);
                return;
            }
        }

        targetStats.ApplyAilments(canApplyIgnite, canAppllyChild, canApplyShock);
    }

    private int CalculateMagicDamage(CharacterStats targetStats, int fireDamage, int iceDamage, int lightingDamage)
    {
        int totalMagicalDamage = fireDamage + iceDamage + lightingDamage + Intelligence.GetValue();
        totalMagicalDamage -= targetStats.MagicResistance.GetValue() + (targetStats.Intelligence.GetValue() * 3);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool ignite, bool chill, bool schock)
    {
        if (IsIgnited || IsChilled || IsSchocked) return;

        IsIgnited = ignite;
        IsChilled = chill;
        IsSchocked = schock;
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
