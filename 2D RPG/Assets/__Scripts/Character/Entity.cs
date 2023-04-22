using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public EntityFX EntityFX { get; private set; }
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
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;
    #endregion

    protected virtual void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        EntityFX = GetComponent<EntityFX>();
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

    public virtual void Flip()
    {
        FacingRight = !FacingRight;
        FacingDir *= -1;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public virtual void FlipController(float move)
    {
        if (move > 0 && !FacingRight)
            Flip();
        else if (move < 0 && FacingRight)
            Flip();
    }

    public virtual void DamageEffect()
    {
        StartCoroutine(HitKnockback());
        StartCoroutine(EntityFX.FlashFX());
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        Rigidbody2D.velocity = new Vector2(knockbackDirection.x * -FacingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
            SpriteRenderer.color = Color.clear;
        else
            SpriteRenderer.color = Color.white;
    }

    public virtual void Die() { }

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
}