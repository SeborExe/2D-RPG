using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger() => player.AnimationTrigger();

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                player.CharacterStats.DoDamage(enemyStats);;

                ItemDataEquipment currentWeapon = Inventory.Instance.GetEquipment(EquipmentType.Weapon);
                if (currentWeapon != null)
                    currentWeapon.Effect(enemyStats.transform);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.Instance.SwordSkill.CreateSword();
    }
}
