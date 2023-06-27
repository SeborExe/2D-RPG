using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyExplosion : MonoBehaviour
{
    private CharacterStats stats;
    private float growSpeed = 15f;
    private float maxSize = 6f;
    private float explosionRadius;
    private Vector3 shakePower = new Vector3(2, 2, 0);
    private Animator anim;

    private bool canGrow = true;

    private void Update()
    {
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (maxSize - transform.localScale.x < 0.5f)
        {
            canGrow = false;
            anim.SetTrigger(Resources.Explode);
        }
    }

    public void SetupExplosion(CharacterStats stats, float growSpeed, float maxSize, float radius)
    {
        anim = GetComponent<Animator>();

        this.stats = stats;
        this.growSpeed = growSpeed;
        this.maxSize = maxSize;
        explosionRadius = radius;
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out CharacterStats characterStats))
            {
                //player.CharacterStats.DoMagicDamage(enemy.CharacterStats);
                characterStats.GetComponent<Entity>().SetUpKnockbackDir(transform);
                stats.DoDamage(characterStats);
                PlayerManager.Instance.player.PlayerFX.ScreenShake(shakePower);
            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);
}
