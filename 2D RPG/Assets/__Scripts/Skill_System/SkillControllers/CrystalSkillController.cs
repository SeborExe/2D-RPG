using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator animator;
    private CircleCollider2D collider;

    private float crystalTimer;
    private float moveSpeed;
    private bool canMove;
    private bool canExplode;
    private bool canGrow;

    [SerializeField] private float growSpeed;

    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMove, float moveSpeed)
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();

        crystalTimer = crystalDuration;
        this.canExplode = canExplode;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
    }

    private void Update()
    {
        crystalTimer -= Time.deltaTime;
        if (crystalTimer <= 0)
        {
            FinishCrystal();
        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector3(3, 3), growSpeed * Time.deltaTime);
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            animator.SetTrigger(Resources.Explode);
        }         
        else
            SelfDestroy();
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, collider.radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                enemy.Damage();
            }
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
