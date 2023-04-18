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

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < distanceToDisapear)
            {
                player.CatchSword();
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

        canRotate = false;
        collider.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;

        animator.SetBool(Resources.Rotation, false);
    }
}
