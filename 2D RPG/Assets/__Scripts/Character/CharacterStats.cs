using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action OnHealthChanged;

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
    [field: SerializeField] public bool IsIgnited { get; private set; } //Damage over time
    [field: SerializeField] public bool IsChilled { get; private set; } //Decrease armor and slowdown
    [field: SerializeField] public bool IsSchocked { get; private set; } //Reduce accurecy

    private EntityFX entityFX;

    [Space, Header("Default Values")]
    [SerializeField] private float igniteDamageCooldown = 0.3f;
    [SerializeField] private int defaultCriticalPower = 150;
    [SerializeField] private float aligmentsDuration = 3f;
    [SerializeField] private float slowPercentageWhenChill = 0.2f;

    private float ignitedTimer;
    private float chilledTimer;
    private float schockedTimer;

    private float igniteDamageTimer;
    private int igniteDamage;

    public int CurrentHealth { get; private set; }

    protected virtual void Start()
    {
        entityFX = GetComponent<EntityFX>();

        CurrentHealth = GetMaxHealthValue();
        CriticPower.SetDefaultValue(defaultCriticalPower);
        OnHealthChanged?.Invoke();
    }

    protected virtual void Update()
    {
        UpdateTimers();
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
        DoMagicDamage(targetStats);
    }


    public virtual void TakeDamage(int damage)
    {
        DecreaseHealthBy(damage);

        if (CurrentHealth == 0)
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

        if (canApplyIgnite)
            targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDamage * 0.15f));

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

        if (ignite)
        {
            IsIgnited = ignite;
            ignitedTimer = 2f;
            igniteDamageTimer = igniteDamageCooldown;

            entityFX.IgniteFX(aligmentsDuration);
        }

        if (chill)
        {
            IsChilled = chill;
            chilledTimer = 2f;

            entityFX.ChillFX(aligmentsDuration);
            GetComponent<Entity>().SlowEntity(slowPercentageWhenChill, aligmentsDuration);
        }

        if (schock)
        {
            IsSchocked = schock;
            schockedTimer = 2f;

            entityFX.SchockFX(aligmentsDuration);
        }
    }

    protected virtual void Die()
    {
        
    }

    protected virtual void DecreaseHealthBy(int damage)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

        OnHealthChanged?.Invoke();
    }

    private bool AvoidAttack(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.Evasion.GetValue() + targetStats.Agility.GetValue();

        if (IsSchocked)
            totalEvasion += 20;

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private int CalculateDamage(CharacterStats targetStats)
    {
        int totalDamage = Damage.GetValue() + Strength.GetValue();

        if (targetStats.IsChilled)
            totalDamage = Mathf.Max(1, totalDamage - Mathf.RoundToInt(targetStats.Armor.GetValue() * 0.8f));
        else
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

    public void SetupIgniteDamage(int damage) => igniteDamage = damage;

    private void UpdateTimers()
    {
        if (ignitedTimer > 0)
        {
            ignitedTimer -= Time.deltaTime;
            if (ignitedTimer <= 0)
            {
                ignitedTimer = 0;
                IsIgnited = false;
            }
        }

        if (igniteDamageTimer > 0)
        {
            igniteDamageTimer -= Time.deltaTime;
            if (igniteDamageTimer <= 0 && IsIgnited)
            {
                DealIgniteDamage();
                igniteDamageTimer = igniteDamageCooldown;
            }
        }

        if (chilledTimer > 0)
        {
            chilledTimer -= Time.deltaTime;
            if (chilledTimer <= 0)
            {
                chilledTimer = 0;
                IsChilled = false;
            }
        }

        if (schockedTimer > 0)
        {
            schockedTimer -= Time.deltaTime;
            if (schockedTimer <= 0)
            {
                schockedTimer = 0;
                IsSchocked = false;
            }
        }
    }

    private void DealIgniteDamage()
    {
        DecreaseHealthBy(igniteDamage);

        if (CurrentHealth == 0)
            Die();
    }

    public int GetMaxHealthValue()
    {
        return MaxHealth.GetValue() + Vitality.GetValue() * 5;
    }
}
