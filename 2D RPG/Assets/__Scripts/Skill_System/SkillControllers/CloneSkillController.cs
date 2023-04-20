using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float cloneTimer;
    private float colorLoosingSpeed;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;

    private Transform closestEnemy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetupClone(Transform newTransform, float cloneDuration, float colorLoosingSpeed, bool canAttack, Vector3 offset, Transform closestEnemy)
    {
        transform.position = newTransform.position + offset;
        cloneTimer = cloneDuration;
        this.colorLoosingSpeed = colorLoosingSpeed;
        this.closestEnemy = closestEnemy;

        if (canAttack)
        {
            animator.SetInteger(Resources.AttackNumber, Random.Range(1, 3));
        }

        FaceClosestTarget();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer <= 0)
        {
            cloneTimer = 0;
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - (Time.deltaTime * colorLoosingSpeed));

            if (spriteRenderer.color.a <= 0) 
                Destroy(gameObject);
        }
    }

    private void AnimationTrigger() => cloneTimer = -1f;

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                enemy.Damage();
            }
        }
    }

    private void FaceClosestTarget()
    { 
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
}
