using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float speed;
    private CharacterStats targetStats;
    private int damage;

    private bool triggered;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetUp(int damage, CharacterStats targetStats)
    {
        this.damage = damage;
        this.targetStats = targetStats;
    }

    private void Update()
    {
        if (!targetStats) return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.2f && !triggered)
        {
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            anim.transform.localPosition = new Vector3(0, 0.5f);

            Invoke(nameof(DamageAndSelfDestroy), 0.2f);
            triggered = true;
            anim.SetTrigger(Resources.Hit);
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, 0.4f);
    }
}
