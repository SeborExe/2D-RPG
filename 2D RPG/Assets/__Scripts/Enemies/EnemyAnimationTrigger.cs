using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    [SerializeField] private int attackSoundIndex = -1;

    private Enemy enemy => GetComponentInParent<Enemy>();

    private void AnimationTrigger() => enemy.AnimationFinishTrigger();

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        if (attackSoundIndex != -1)
            AudioManager.Instance.PlaySFX(attackSoundIndex, enemy.transform);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Player player))
            {
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                enemy.CharacterStats.DoDamage(playerStats);            }
        }
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();

    private void CloseCounterAttackWindow() => enemy?.CloseCounterAttackWindow();
}
