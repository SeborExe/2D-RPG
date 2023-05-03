using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect_FreezEnemies_", menuName = "Inventory/Item Effect/Freez Enemies")]
public class FreezEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;
    [SerializeField] private float minHealthPercentToActiveEffect;
    [SerializeField] private float freezAreaRadius;

    public override void ExecuteEffect(Transform transform)
    {
        if (!Inventory.Instance.CanUseArmor()) return;

        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        if (playerStats.CurrentHealth > playerStats.GetMaxHealthValue() * minHealthPercentToActiveEffect) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, freezAreaRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                enemy.FreezTimeFor(duration);
            }
        }
    }
}
