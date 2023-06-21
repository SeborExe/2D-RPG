using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
    public event Action OnFlipped;

    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public CharacterStats CharacterStats { get; private set; }
    public CapsuleCollider2D CapsuleCollider { get; private set;}

    #region CollisionInfo
    [field: Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    public Transform attackCheck;
    public float attackCheckRadius;
    #endregion

    #region Direction Info
    [field: Header("Direction Info")]
    public int FacingDir { get; private set; } = 1;
    protected bool FacingRight { get; private set; } = true;
    #endregion

    #region Knockback
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;
    public int KnockbackDir { get; private set; }
    #endregion

    #region Dead
    public bool IsDead { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        CharacterStats = GetComponent<CharacterStats>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        Animator = GetComponentInChildren<Animator>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public void SetVelocity(float xVelocity, float yVelicoty)
    {
        if (isKnocked) return;

        Rigidbody2D.velocity = new Vector2(xVelocity, yVelicoty);
        FlipController(xVelocity);
    }

    public virtual void SlowEntity(float slowPercentage, float slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        Animator.speed = 1f;
    }

    public virtual void Flip()
    {
        FacingRight = !FacingRight;
        FacingDir *= -1;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        OnFlipped?.Invoke();
    }

    public virtual void FlipController(float move)
    {
        if (move > 0 && !FacingRight)
            Flip();
        else if (move < 0 && FacingRight)
            Flip();
    }

    public virtual void DamageImpact()
    {
        StartCoroutine(HitKnockback());
    }

    public virtual void SetUpKnockbackDir(Transform damageDirection)
    {
        if (damageDirection.position.x > transform.position.x)
            KnockbackDir = -1;
        else if (damageDirection.position.x < transform.position.x)
            KnockbackDir = 1;
    }

    public void SetUpKnockbackPower(Vector2 knockbackPower) => this.knockbackPower = knockbackPower;

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        Rigidbody2D.velocity = new Vector2(knockbackPower.x * KnockbackDir, knockbackPower.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
        SetUpZeroKnockbackPower();
    }

    protected virtual void SetUpZeroKnockbackPower() { }

    public virtual void Die() 
    {
        IsDead = true;

        CapsuleCollider.offset = new Vector2(0, -0.73f);
        CapsuleCollider.size = new Vector2(0.5f, 0.5f);
    }

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
}
