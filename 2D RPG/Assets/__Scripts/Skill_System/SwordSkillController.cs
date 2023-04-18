using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D collider;
    private Player player;

    [SerializeField] private float returnSpeed = 12f;
    [SerializeField] private float distanceToDisapear = 0.5f;

    private bool canRotate = true;
    private bool isReturning;
    private SwordType swordType;

    [Header("Bounce Info")]
    private bool isBouncing;
    private int bounceAmount;
    private float bouncingRadius = 8f;
    private float bounceSpeed;
    private int targetIndex;
    private List<Transform> enemyTarget = new List<Transform>();

    [Header("Pierce Info")]
    private int pierceAmount;

    [Header("Spin Info")]
    private bool isSpining;
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private float hitTimer;
    private float hitCooldown;
    private float spinDamageRadius = 1f;
    private float spinDirection;
    private float spinMoveSpeed;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(Vector2 direction, float gravityScale, Player player, SwordType swordType)
    {
        rb.velocity = direction;
        rb.gravityScale = gravityScale;
        this.player = player;
        this.swordType = swordType;

        if (swordType != SwordType.Pirce)
            animator.SetBool(Resources.Rotation, true);

        spinDirection = Math.Clamp(rb.velocity.x, -1, 1);
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        HandleReturning();
        HandleBounce();
        HandleSpin();
    }

    public void SetUpBounce(int bounceAmount, float bounceSpeed)
    {
        isBouncing = swordType == SwordType.Bounce;
        this.bounceAmount = bounceAmount;
        this.bounceSpeed = bounceSpeed;
    }

    public void SetUpPierce(int pierceAmount)
    {
        this.pierceAmount = pierceAmount;
    }

    public void SetUpSpin(float maxTravelDistance, float spinDuration, float hitCooldown, float spinMoveSpeed)
    {
        isSpining = swordType == SwordType.Spin;
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.hitCooldown = hitCooldown;
        this.spinMoveSpeed = spinMoveSpeed;
    }

    private void HandleReturning()
    {
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < distanceToDisapear)
            {
                player.CatchSword();
            }
        }
    }

    private void HandleBounce()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                enemyTarget[targetIndex].GetComponent<Enemy>()?.Damage();

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void HandleSpin()
    {
        if (isSpining)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpeening();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                //Move Spin sword into enemy 
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y),
                    spinMoveSpeed * Time.deltaTime);

                if (spinTimer <= 0)
                {
                    isReturning = true;
                    isSpining= false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, spinDamageRadius);

                    foreach (Collider2D hit in colliders)
                    {
                        if (hit.TryGetComponent(out Enemy enemy))
                            enemy.Damage();
                    }
                }
            }
         }
    }

    private void StopWhenSpeening()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) return;

        collision.GetComponent<Enemy>()?.Damage();
        SetUpTargetsForBounce(collision);

        StickTheSword(collision);
    }

    private void SetUpTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bouncingRadius);

                foreach (Collider2D hit in colliders)
                {
                    if (hit.TryGetComponent(out Enemy enemy))
                        enemyTarget.Add(enemy.transform);
                }
            }
        }
    }

    private void StickTheSword(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return; 
        }

        if (isSpining)
        {
            StopWhenSpeening();
            return;
        }

        canRotate = false;
        collider.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0) return;

        transform.parent = collision.transform;
        animator.SetBool(Resources.Rotation, false);
    }
}
