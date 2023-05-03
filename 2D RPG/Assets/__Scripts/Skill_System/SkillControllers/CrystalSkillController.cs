using System;
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

    private Transform closestEnemy;
    private Player player;

    [SerializeField] private float growSpeed;

    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMove, float moveSpeed, Transform closestEnemy, Player player)
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();

        crystalTimer = crystalDuration;
        this.canExplode = canExplode;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
        this.closestEnemy = closestEnemy;
        this.player = player;
    }

    private void Update()
    {
        crystalTimer -= Time.deltaTime;
        if (crystalTimer <= 0)
        {
            FinishCrystal();
        }

        if (canMove && closestEnemy != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestEnemy.position) < 1f)
            {
                FinishCrystal();
                canMove = false;
            }
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
                player.CharacterStats.DoMagicDamage(enemy.CharacterStats);
                ItemDataEquipment equipedAmulat = Inventory.Instance.GetEquipment(EquipmentType.Amulet);
                if (equipedAmulat != null)
                {
                    equipedAmulat.Effect(enemy.transform);
                }
            }
        }
    }

    public void ChooseRandomEnemy(float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                if (enemy.IsDead) return; 

                enemiesInRange.Add(enemy);
            }
        }

        if (enemiesInRange.Count > 0)
        {
            closestEnemy = enemiesInRange[UnityEngine.Random.Range(0, enemiesInRange.Count)].transform;
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
