using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [field: SerializeField] public Stat Strength { get; private set; }
    [field: SerializeField] public Stat Damage { get; private set; }
    [field: SerializeField] public Stat MaxHealth { get; private set;}

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = MaxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {
        int totalDamage = Damage.GetValue() + Strength.GetValue();

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
}
