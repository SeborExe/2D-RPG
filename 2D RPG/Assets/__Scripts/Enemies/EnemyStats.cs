using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop itemDrop;

    [Header("Level Details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f), SerializeField] private float percentageModifier = 0.3f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        itemDrop = GetComponent<ItemDrop>();
    }

    protected override void Start()
    {
        ApplyLevelModifier();

        base.Start();
    }

    private void ApplyLevelModifier()
    {
        Modifier(Strength);
        Modifier(Agility);
        Modifier(Intelligence);
        Modifier(Vitality);

        Modifier(Damage);
        Modifier(CriticChance);
        Modifier(CriticPower);

        Modifier(MaxHealth);
        Modifier(Armor);
        Modifier(Evasion);
        Modifier(MagicResistance);

        Modifier(FireDamage);
        Modifier(IceDamage);
        Modifier(LightingDamage);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    private void Modifier(Stat stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = stat.GetValue() * percentageModifier;
            stat.AddModifiers(Mathf.RoundToInt(modifier));
        }
    }

    protected override void Die()
    {
        if (enemy.IsDead) return;

        base.Die();
        enemy.Die();

        itemDrop.GenerateDrop();
    }
}
