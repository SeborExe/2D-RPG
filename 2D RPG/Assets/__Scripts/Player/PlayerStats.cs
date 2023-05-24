using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        if (player.IsDead) return;

        base.Die();

        player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int damage)
    {
        base.DecreaseHealthBy(damage);

        ItemDataEquipment currentArmor = Inventory.Instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor != null)
        {
            currentArmor.Effect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.SkillManager.DodgeSkill.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats targetStats, float multiplier)
    {
        if (AvoidAttack(targetStats))
            return;

        int totalDamage = CalculateDamage(targetStats);

        if (multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * multiplier);

        if (CheckCritical())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        targetStats.TakeDamage(totalDamage);

        DoMagicDamage(targetStats); //Apply magic damage on normal attack
    }
}
