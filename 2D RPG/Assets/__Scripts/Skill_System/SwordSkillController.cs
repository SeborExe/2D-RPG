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

    [Header("Bouncing")]
    public bool isBouncing = true;
    public int amountOfBounce = 4;
    public List<Transform> enemyTarget = new List<Transform>();
    private int targetIndex;
    private float bouncingRadius = 8f;
    private float bouncingSpeed = 20f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(Vector2 direction, float gravityScale, Player player)
    {
        rb.velocity = direction;
        rb.gravityScale = gravityScale;
        this.player = player;

        animator.SetBool(Resources.Rotation, true);
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        HandleReturning();
        HandleBounce();
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
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bouncingSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
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

        if (collision.TryGetComponent(out Enemy enemy))
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bouncingRadius);

                foreach (Collider2D hit in colliders)
                {
                    if (hit.TryGetComponent(out enemy))
                        enemyTarget.Add(enemy.transform);
                }
            }
        }

        StickTheSword(collision);
    }

    private void StickTheSword(Collider2D collision)
    {
        canRotate = false;
        collider.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0) return;

        transform.parent = collision.transform;
        animator.SetBool(Resources.Rotation, false);
    }
}
