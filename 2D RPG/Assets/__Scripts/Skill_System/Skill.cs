using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.Instance.player;
    }

    protected virtual void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0) cooldownTimer = 0;
        }
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    public virtual void UseSkill()
    {

    }

    protected virtual Transform FindClosestEnemy(Transform checkTransform)
    {
        float detectionRadius = 10f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkTransform.position, detectionRadius);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy) && !enemy.IsDead)
            {
                float distanceToEnemy = Vector2.Distance(checkTransform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemy.transform;
                }
            }
        }

        return closestEnemy;
    }

    protected virtual Transform ChooseRandomEnemy(Transform checkTransform)
    {
        float detectionRadius = 10f;
        Transform closestEnemy = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkTransform.position, detectionRadius);
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }

        if (enemiesInRange.Count > 0)
        {
            closestEnemy = enemiesInRange[Random.Range(0, enemiesInRange.Count)].transform;
        }

        return closestEnemy;
    }
}
