using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BringerOfDeathSpellHand : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Vector3 shakePower;

    private CharacterStats stats;

    public void SetUpSpell(CharacterStats stats)
    {
        this.stats = stats;
    }

    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerStats characterStats))
            {
                //player.CharacterStats.DoMagicDamage(enemy.CharacterStats);
                characterStats.GetComponent<Entity>().SetUpKnockbackDir(transform);
                stats.DoDamage(characterStats);
                PlayerManager.Instance.player.PlayerFX.ScreenShake(shakePower);
            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
    }
}
